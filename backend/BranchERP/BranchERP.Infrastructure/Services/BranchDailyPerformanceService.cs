using AutoMapper;
using BranchERP.Application.DTOs.BranchDailyPerformance;
using BranchERP.Application.DTOs.Reports;
using BranchERP.Application.Interfaces;
using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.Enums;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Infrastructure.Services
{
    public class BranchDailyPerformanceService : IBranchDailyPerformanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BranchDailyPerformanceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BranchDailyPerformanceDto?> GetAsync(int branchId, DateTime targetDate)
        {
            var repo = _unitOfWork.Repository<BranchDailyPerformance>();

            var entity = await repo.GetAsync(
                 x => x.BranchId == branchId
                   && x.TargetDate.Year == targetDate.Year
                   && x.TargetDate.Month == targetDate.Month
                   && x.TargetDate.Day == targetDate.Day,
                 include: q => q.Include(x => x.Branch)
             );


            if (entity == null)
                return null;

            return _mapper.Map<BranchDailyPerformanceDto>(entity);
        }

        public async Task<BranchDailyPerformanceDto> CreateOrUpdateTargetAsync(BranchDailyPerformanceCreateUpdateDto dto)
        {
            var repo = _unitOfWork.Repository<BranchDailyPerformance>();

            var entity = await repo.GetAsync(
                x => x.BranchId == dto.BranchId && x.TargetDate.Date == dto.TargetDate.Date
            );

            if (entity == null)
            {
                entity = _mapper.Map<BranchDailyPerformance>(dto);
                entity.BranchTargetAmount = dto.BranchTargetAmount ?? 0;

                await repo.AddAsync(entity);
            }
            else
            {
                entity.BranchTargetAmount = dto.BranchTargetAmount ?? entity.BranchTargetAmount;
                entity.UpdatedAt = DateTime.UtcNow;

                repo.Update(entity);
            }

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<BranchDailyPerformanceDto>(entity);
        }

        public async Task<BranchDailyPerformanceDto> CreateOrUpdateAchievementAsync(BranchDailyPerformanceCreateUpdateDto dto)
        {
            var repo = _unitOfWork.Repository<BranchDailyPerformance>();

            var entity = await repo.GetAsync(
                x => x.BranchId == dto.BranchId && x.TargetDate.Date == dto.TargetDate.Date
            );

            if (entity == null)
            {
                entity = _mapper.Map<BranchDailyPerformance>(dto);
                entity.BranchAchievedAmount = dto.BranchAchievedAmount ?? 0;

                await repo.AddAsync(entity);
            }
            else
            {
                entity.BranchAchievedAmount = dto.BranchAchievedAmount ?? entity.BranchAchievedAmount;
                entity.BranchInvoicesCountAchieved = dto.BranchInvoicesCountAchieved ?? entity.BranchInvoicesCountAchieved;
                entity.BranchItemsCountAchieved = dto.BranchItemsCountAchieved ?? entity.BranchItemsCountAchieved;

                // حساب نسبة الإنجاز
                if (entity.BranchTargetAmount > 0 && entity.BranchAchievedAmount.HasValue)
                {
                    entity.AchievementPercentage =
                        Math.Round((entity.BranchAchievedAmount.Value / entity.BranchTargetAmount) * 100, 2);
                }

                entity.UpdatedAt = DateTime.UtcNow;

                repo.Update(entity);
            }

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<BranchDailyPerformanceDto>(entity);
        }

        public async Task ImportDailyTargetsFromExcelAsync(Stream fileStream)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var reader = ExcelReaderFactory.CreateReader(fileStream);
            var result = reader.AsDataSet();
            var table = result.Tables[0];

            var branchRepo = _unitOfWork.Repository<Branch>();
            var perfRepo = _unitOfWork.Repository<BranchDailyPerformance>();

            for (int i = 1; i < table.Rows.Count; i++) // نتخطى الهيدر
            {
                var row = table.Rows[i];

                var branchNumberStr = row[0]?.ToString()?.Trim();
                var targetDateStr = row[1]?.ToString()?.Trim();
                var targetAmountStr = row[2]?.ToString()?.Trim();

                if (string.IsNullOrWhiteSpace(branchNumberStr) ||
                    string.IsNullOrWhiteSpace(targetDateStr) ||
                    string.IsNullOrWhiteSpace(targetAmountStr))
                    continue;

                if (!DateTime.TryParse(targetDateStr, out var targetDate))
                    continue;

                if (!decimal.TryParse(targetAmountStr, out var targetAmount))
                    continue;

                // لو BranchNumber عندك int غيّر Parse هنا
                if (!int.TryParse(branchNumberStr, out var branchNumber))
                    continue;

                var branch = await branchRepo.GetAsync(b => b.BranchNumber == branchNumber);
                if (branch == null)
                    continue;

                var entity = await perfRepo.GetAsync(
                    x => x.BranchId == branch.Id && x.TargetDate.Date == targetDate.Date
                );

                if (entity == null)
                {
                    entity = new BranchDailyPerformance
                    {
                        BranchId = branch.Id,
                        TargetDate = targetDate.Date,
                        BranchTargetAmount = targetAmount,
                        BranchAchievedAmount = 0,
                        BranchInvoicesCountAchieved = 0,
                        BranchItemsCountAchieved = 0,
                        CreatedAt = DateTime.UtcNow
                    };

                    await perfRepo.AddAsync(entity);
                }
                else
                {
                    entity.BranchTargetAmount = targetAmount;
                    entity.UpdatedAt = DateTime.UtcNow;
                    perfRepo.Update(entity);
                }
            }

            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<BranchDailyPerformanceReportRowDto>> GetBranchDailyPerformanceReportAsync(BranchDailyPerformanceReportFilterDto filter)
        {
            var perfRepo = _unitOfWork.Repository<BranchDailyPerformance>();
            var commissionRepo = _unitOfWork.Repository<CommissionRule>();

            var date = filter.Date == default ? DateTime.Today : filter.Date;

            var query = perfRepo.Query()
                .Include(x => x.Branch)
                    .ThenInclude(b => b.City)
                .Where(x => x.TargetDate.Date == date.Date);

            if (filter.CityId.HasValue)
                query = query.Where(x => x.Branch.CityId == filter.CityId.Value);

            if (filter.BranchId.HasValue)
                query = query.Where(x => x.BranchId == filter.BranchId.Value);

            var list = await query.ToListAsync();

            // 🔥 نجيب كل القواعد مرة واحدة
            var rules = await commissionRepo.Query()
                .Where(r => r.IsActive && r.Type == CommissionType.Branch)
                .OrderBy(r => r.MinPercentage)
                .ToListAsync();

            var result = new List<BranchDailyPerformanceReportRowDto>();

            foreach (var item in list)
            {
                decimal target = item.BranchTargetAmount;
                decimal achieved = item.BranchAchievedAmount ?? 0;

                decimal percentage = 0;
                if (target > 0)
                    percentage = Math.Round((achieved / target) * 100, 2);

                // 🔥 اختيار القاعدة حسب التاريخ + النسبة
                // 🔥 اختيار القاعدة حسب التاريخ + النسبة
                var rule = rules
                    .Where(r => r.CreatedAt.Date <= item.TargetDate.Date)
                    .Where(r => percentage >= r.MinPercentage &&
                                (r.MaxPercentage == null || percentage <= r.MaxPercentage))
                    .OrderByDescending(r => r.CreatedAt)
                    .FirstOrDefault();


                decimal commission = 0;

                if (rule != null && rule.FixedBonusAmount.HasValue)
                    commission += rule.FixedBonusAmount.Value;

                result.Add(new BranchDailyPerformanceReportRowDto
                {
                    BranchId = item.BranchId,
                    BranchName = item.Branch?.BranchName ?? "غير محدد",
                    CityName = item.Branch?.City?.CityName ?? "غير محددة",
                    TargetDate = item.TargetDate,
                    TargetAmount = target,
                    AchievedAmount = achieved,
                    AchievementPercentage = percentage,
                    CommissionAmount = commission
                });
            }

            return result;
        }

        public async Task<byte[]> ExportBranchDailyPerformanceReportToExcelAsync(BranchDailyPerformanceReportFilterDto filter)
        {
            // نجيب نفس بيانات التقرير العادي
            var data = await GetBranchDailyPerformanceReportAsync(filter);

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("BranchDailyReport");

            // الهيدر
            ws.Cell(1, 1).Value = "الفرع";
            ws.Cell(1, 2).Value = "المدينة";
            ws.Cell(1, 3).Value = "التاريخ";
            ws.Cell(1, 4).Value = "التارجت";
            ws.Cell(1, 5).Value = "المنجز";
            ws.Cell(1, 6).Value = "نسبة الإنجاز";
            ws.Cell(1, 7).Value = "العمولة";

            var row = 2;
            foreach (var item in data)
            {
                ws.Cell(row, 1).Value = item.BranchName;
                ws.Cell(row, 2).Value = item.CityName;
                ws.Cell(row, 3).Value = item.TargetDate.ToString("yyyy-MM-dd");
                ws.Cell(row, 4).Value = item.TargetAmount;
                ws.Cell(row, 5).Value = item.AchievedAmount;
                ws.Cell(row, 6).Value = item.AchievementPercentage;
                ws.Cell(row, 7).Value = item.CommissionAmount;
                row++;
            }

            // شوية تنسيقات بسيطة
            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public async Task<List<BranchPerformanceChartDto>> GetBranchPerformanceChartAsync(BranchDailyPerformanceReportFilterDto filter)
        {
            var report = await GetBranchDailyPerformanceReportAsync(filter);

            return report.Select(r => new BranchPerformanceChartDto
            {
                BranchName = r.BranchName,
                AchievementPercentage = r.AchievementPercentage,
                TargetAmount = r.TargetAmount,
                AchievedAmount = r.AchievedAmount,
                CommissionAmount = r.CommissionAmount
            }).ToList();
        }


    }
}
