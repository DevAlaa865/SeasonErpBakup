using BranchERP.Application.DTOs.ExpenseVouchers;
using BranchERP.Application.Interfaces;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.Enums;
using BranchERP.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BranchERP.Infrastructure.Services.ExpenseVouchers
{
    public class CashPostingService : ICashPostingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;   // 🔥 السطر الناقص

        public CashPostingService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager
        )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;   // 🔥 دلوقتي بقى له مكان
        }

        public async Task<CashPostingResultDto> PostDailyCashForUserAsync(string userId, DateTime date)
        {
            var result = new CashPostingResultDto
            {
                UserId = userId,
                Date = date
            };

            // 1) المدن المرتبطة باليوزر
            var userCashCityRepo = _unitOfWork.Repository<UserCashCity>();
            var userCities = await userCashCityRepo.GetAllAsync(
                x => x.UserId == userId,
                include: q => q.Include(u => u.City)
            );

            if (!userCities.Any())
                return result;

            var branchRepo = _unitOfWork.Repository<Branch>();
            var dailyRepo = _unitOfWork.Repository<BranchSalesDaily>();
            var collectorRepo = _unitOfWork.Repository<DepositCollector>();
            var cashBoxRepo = _unitOfWork.Repository<CashBox>();
            var transactionRepo = _unitOfWork.Repository<CashBoxTransaction>();

            foreach (var city in userCities)
            {
                var cityResult = new CashPostingCityResultDto
                {
                    CityId = city.CityId,
                    CityName = city.City.CityName
                };

                // 2) مسؤول الإيداع الخاص بالمدينة نفسها فقط
                var collector = await collectorRepo.GetAsync(
                    x => x.DepositCollectorCities.Any(c => c.CityId == city.CityId)
                );

                if (collector == null)
                {
                    cityResult.CashBoxName = "لا يوجد مسؤول إيداع";
                    result.Cities.Add(cityResult);
                    continue;
                }

                // 3) صندوق مسؤول الإيداع الخاص بالمدينة
                var cashBox = await cashBoxRepo.GetAsync(
                    x => x.DepositCollectorId == collector.Id
                );

                if (cashBox == null)
                {
                    cityResult.CashBoxName = "لا يوجد صندوق مسؤول إيداع";
                    result.Cities.Add(cityResult);
                    continue;
                }

                // 4) فروع المدينة
                var branches = await branchRepo.GetAllAsync(b => b.CityId == city.CityId);
                if (!branches.Any())
                {
                    result.Cities.Add(cityResult);
                    continue;
                }

                var branchIds = branches.Select(b => b.Id).ToList();

                // 5) يوميات المدينة
                var dailyCashList = await dailyRepo.GetAllAsync(
                    x => branchIds.Contains(x.BranchId) && x.SalesDate.Date == date.Date
                );

                // 🔥 تحديد الفروع الناقصة
                var postedBranchIds = dailyCashList.Select(x => x.BranchId).Distinct().ToList();
                var missingBranches = branches
                    .Where(b => !postedBranchIds.Contains(b.Id))
                    .Select(b => b.BranchName)
                    .ToList();

                if (missingBranches.Any())
                {
                    cityResult.HasMissingBranches = true;
                    cityResult.MissingBranches = missingBranches;
                }

                // لو مفيش يوميات خالص
                if (!dailyCashList.Any())
                {
                    result.Cities.Add(cityResult);
                    continue;
                }

                // 6) إجمالي النقدية
                var totalCash = dailyCashList.Sum(x => x.CashAmount ?? 0);
                cityResult.TotalDailyCash = totalCash;

                // 7) منع الترحيل المكرر للمدينة نفسها
                var alreadyPosted = await transactionRepo.GetAsync(
                    x => x.CashBoxId == cashBox.Id &&
                         x.TransactionType == TransactionType.DailyCash &&
                         x.TransactionDate.Date == date.Date &&
                         x.Description.Contains(city.City.CityName)
                );

                if (alreadyPosted != null)
                {
                    cityResult.AlreadyPosted = true;
                    result.Cities.Add(cityResult);
                    continue;
                }

                // 8) إضافة حركة IN
                var transaction = new CashBoxTransaction
                {
                    CashBoxId = cashBox.Id,
                    Amount = totalCash,
                    Direction = TransactionDirection.IN,
                    TransactionType = TransactionType.DailyCash,
                    BranchId = null,

                    Description = $"ترحيل يوميات فروع مدينة {city.City.CityName} بتاريخ {date:yyyy-MM-dd}",

                    TransactionDate = date,        // SalesDate
                    CreatedAt = DateTime.Now       // تاريخ الترحيل الفعلي
                };

                await transactionRepo.AddAsync(transaction);

                // 9) تحديث رصيد الصندوق
                cashBox.CurrentBalance += totalCash;
                cashBoxRepo.Update(cashBox);

                await _unitOfWork.CompleteAsync();

                cityResult.CashBoxId = cashBox.Id;
                cityResult.CashBoxName = cashBox.Name;

                result.Cities.Add(cityResult);
            }

            return result;
        }

        public async Task<ManualPostingResultDto> ManualPostAsync(ManualPostingRequestDto dto)
        {
            var result = new ManualPostingResultDto
            {
                Date = dto.Date,
                Amount = dto.Amount   // 🔥 المبلغ اليدوي
            };

            var branchRepo = _unitOfWork.Repository<Branch>();
            var collectorRepo = _unitOfWork.Repository<DepositCollector>();
            var cashBoxRepo = _unitOfWork.Repository<CashBox>();
            var transactionRepo = _unitOfWork.Repository<CashBoxTransaction>();

            // 1) هات الفرع
            var branch = await branchRepo.GetAsync(
                x => x.Id == dto.BranchId,
                include: q => q.Include(b => b.City)
            );

            if (branch == null)
                return result;

            result.BranchName = branch.BranchName;
            result.CityName = branch.City.CityName;

            // 🔥 استخدم المبلغ اليدوي مباشرة
            result.Amount = dto.Amount;

            // 2) هات مسؤول الإيداع
            DepositCollector? collector;

            if (dto.DepositCollectorId.HasValue)
            {
                collector = await collectorRepo.GetByIdAsync(dto.DepositCollectorId.Value);
            }
            else
            {
                collector = await collectorRepo.GetAsync(
                    x => x.DepositCollectorCities.Any(c => c.CityId == branch.CityId)
                );
            }

            if (collector == null)
                return result;

            // 3) هات الصندوق الرئيسي لمسؤول الإيداع
            var cashBox = await cashBoxRepo.GetAsync(
                x => x.DepositCollectorId == collector.Id
            );

            if (cashBox == null)
                return result;

            result.CashBoxName = cashBox.Name;

            // 4) منع الترحيل المكرر
            var alreadyPosted = await transactionRepo.GetAsync(
                x => x.CashBoxId == cashBox.Id &&
                     x.TransactionType == TransactionType.DailyCash &&
                     x.TransactionDate.Date == dto.Date.Date &&
                     x.BranchId == dto.BranchId
            );

            if (alreadyPosted != null)
                return result;

            // 5) إضافة الحركة اليدوية
            var transaction = new CashBoxTransaction
            {
                CashBoxId = cashBox.Id,
                Amount = dto.Amount,                   // 🔥 المبلغ اليدوي
                Direction = TransactionDirection.IN,
                TransactionType = TransactionType.DailyCash,

                BranchId = branch.Id,

                TransactionDate = dto.Date,            // 🔥 تاريخ اليوميات
                CreatedAt = DateTime.Now,              // 🔥 تاريخ الترحيل الفعلي

                Description = $"ترحيل يدوي لفرع {branch.BranchName} بمبلغ {dto.Amount} بتاريخ {dto.Date:yyyy-MM-dd}"
            };

            await transactionRepo.AddAsync(transaction);

            // 6) تحديث رصيد الصندوق
            cashBox.CurrentBalance += dto.Amount;      // 🔥 المبلغ اليدوي
            cashBoxRepo.Update(cashBox);

            await _unitOfWork.CompleteAsync();

            result.Success = true;
            return result;
        }



        public async Task<List<PostingHistoryDto>> GetPostingHistoryAsync(DateTime date)
        {
            var transactionRepo = _unitOfWork.Repository<CashBoxTransaction>();

            // نجيب كل الحركات اليومية حسب تاريخ اليوميات (TransactionDate)
            var transactions = await transactionRepo.GetAllAsync(
                x => x.TransactionType == TransactionType.DailyCash &&
                     x.TransactionDate.Date == date.Date,
                include: q => q
                    .Include(t => t.CashBox)
                        .ThenInclude(cb => cb.DepositCollector)
                    .Include(t => t.Branch)
                        .ThenInclude(b => b.City)
            );

            var result = new List<PostingHistoryDto>();

            foreach (var t in transactions)
            {
                // اسم مسؤول الإيداع
                string collectorName = "";
                if (t.CashBox?.DepositCollector?.UserId != null)
                {
                    var user = await _userManager.FindByIdAsync(t.CashBox.DepositCollector.UserId);
                    collectorName = user?.UserName ?? "";
                }

                result.Add(new PostingHistoryDto
                {
                    Id = t.Id,

                    // 🔥 TransactionDate بقى هو SalesDate
                    Date = t.TransactionDate,

                    Amount = t.Amount,
                    CashBoxName = t.CashBox!.Name,
                    CollectorName = collectorName,
                    BranchName = t.Branch?.BranchName ?? "",
                    CityName = t.Branch?.City?.CityName ?? ""
                });
            }

            return result;
        }



        public async Task<PostingDetailsDto?> GetPostingDetailsAsync(int id)
        {
            var repo = _unitOfWork.Repository<CashBoxTransaction>();

            var t = await repo.GetAsync(
                x => x.Id == id,
                include: q => q
                    .Include(x => x.CashBox)
                        .ThenInclude(cb => cb.DepositCollector)
                    .Include(x => x.Branch)
                        .ThenInclude(b => b.City)
            );

            if (t == null)
                return null;

            string collectorName = "";

            if (t.CashBox?.DepositCollector?.UserId != null)
            {
                var user = await _userManager.FindByIdAsync(t.CashBox.DepositCollector.UserId);
                collectorName = user?.UserName ?? "";
            }

            return new PostingDetailsDto
            {
                Id = t.Id,

                // 🔥 TransactionDate = SalesDate
                Date = t.TransactionDate,

                // 🔥 CreatedAt = تاريخ الترحيل الفعلي
                PostedAt = t.CreatedAt,

                Amount = t.Amount,
                CashBoxName = t.CashBox!.Name,
                CollectorName = collectorName,
                BranchName = t.Branch?.BranchName ?? "",
                CityName = t.Branch?.City?.CityName ?? "",
                Description = t.Description!,
                Direction = t.Direction.ToString(),
                Type = t.TransactionType.ToString()
            };
        }

    }
}
