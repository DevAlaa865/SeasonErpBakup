using BranchERP.Application.DTOs.Reports.DailyReports;
using BranchERP.Application.Interfaces.Reports.DailyReports;
using BranchERP.Domain.Entities.Enums;
using BranchERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BranchERP.Infrastructure.Services.Reports.DailyReports
{
    public class BranchDailyDifferenceReportService : IBranchDailyDifferenceReportService
    {
        private readonly AppDbContext _context;

        public BranchDailyDifferenceReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BranchDailyDifferenceReportDto>> GetBranchDailyDifferenceReportAsync(BranchDailyDifferenceReportFilterDto filter)
        {
            var query = _context.BranchSalesDailies
                .Include(x => x.Branch)
                .AsQueryable();

            // فلترة المدينة
            // فلترة المدن
            if (filter.CityIds != null && filter.CityIds.Any())
            {
                query = query.Where(x => filter.CityIds.Contains(x.Branch.CityId));
            }

            // فلترة الفروع
            if (filter.BranchIds != null && filter.BranchIds.Any())
                query = query.Where(x => filter.BranchIds.Contains(x.BranchId));

            // فلترة رقم الفرع
            if (filter.BranchNumber.HasValue)
                query = query.Where(x => x.Branch.BranchNumber == filter.BranchNumber.Value);

            // استبعاد الفرق = صفر فقط في حالة تقرير الفرق
            if (filter.IsNetworkReport != true)
            {
                query = query.Where(x => x.Difference != 0);
            }

            // 🔥 فلترة الفرق الجديدة

            // 1) عجز مسموح به: من -35 إلى -1
            // 🔥 فلترة الفرق الجديدة
            if (filter.IsNetworkReport != true)
            {
                // 1) عجز مسموح به: من -35 إلى -1
                if (filter.IsAllowedShortage == true)
                    query = query.Where(x => x.Difference < 0 && x.Difference >= -35);

                // 2) عجز كبير: أقل من -35
                if (filter.IsBigShortage == true)
                    query = query.Where(x => x.Difference < -35);

                // زيادة صغيرة (1 إلى 35)
                if (filter.IsSmallIncrease == true)
                    query = query.Where(x => x.Difference > 0 && x.Difference <= 35);

                // زيادة كبيرة (> 35)
                if (filter.IsBigIncrease == true)
                    query = query.Where(x => x.Difference > 35);
            }

            // فلترة التاريخ
            if (filter.FromDate.HasValue)
                query = query.Where(x => x.SalesDate >= filter.FromDate.Value.Date);

            if (filter.ToDate.HasValue)
                query = query.Where(x => x.SalesDate <= filter.ToDate.Value.Date);

            var data = await query.ToListAsync();

            // تجهيز النتيجة
            var result = data.Select(x => new BranchDailyDifferenceReportDto
            {
                BranchId = x.BranchId,
                BranchNumber = x.Branch.BranchNumber,
                BranchName = x.Branch.BranchName,
                SalesDate = x.SalesDate,
                Difference = x.Difference ?? 0,
                SalesDailyId = x.Id,
                NetworkAmount = x.NetworkAmount ?? 0,
                
            }).ToList();

            // لو تقرير شبكة فقط
            if (filter.IsNetworkReport == true)
            {
                result = result.Select(x => new BranchDailyDifferenceReportDto
                {
                    BranchId = x.BranchId,
                    BranchNumber = x.BranchNumber,
                    BranchName = x.BranchName,
                    SalesDate = x.SalesDate,
                    NetworkAmount = x.NetworkAmount
                }).ToList();
            }

            return result;
        }

        public async Task<List<AccountsReturnsDiscountsReportRowDto>> GetAccountsReturnsDiscountsReportAsync(
            AccountsReturnsDiscountsReportFilterDto filter)
        {
            var query = _context.BranchSalesShortageDetails
                .Include(s => s.BranchSalesDaily)
                    .ThenInclude(d => d.Branch)
                .Include(s => s.ShortageType)
                .AsQueryable();

            // تأمين ضد Null
            query = query.Where(s => s.BranchSalesDaily != null);

            // التاريخ
            query = query.Where(s =>
                s.BranchSalesDaily.SalesDate >= filter.FromDate &&
                s.BranchSalesDaily.SalesDate <= filter.ToDate);

            // المدينة
            if (filter.CityId.HasValue)
                query = query.Where(s =>
                    s.BranchSalesDaily.Branch != null &&
                    s.BranchSalesDaily.Branch.CityId == filter.CityId.Value);

            // الفروع
            if (filter.BranchIds != null && filter.BranchIds.Any())
                query = query.Where(s =>
                    s.BranchSalesDaily.Branch != null &&
                    filter.BranchIds.Contains(s.BranchSalesDaily.BranchId));

            // نوع العجز
            if (filter.ShortageTypeId.HasValue)
                query = query.Where(s => s.ShortageTypeId == filter.ShortageTypeId.Value);

            // الحالة
            if (filter.Status == ReturnsDiscountsApprovalStatus.Approved)
            {
                query = query.Where(s =>
                    (
                        (s.ShortageTypeId == 3 || s.ShortageTypeId == 6) &&
                        s.IsReturnApproved == true
                    )
                    ||
                    (
                        (s.ShortageTypeId != 3 && s.ShortageTypeId != 6) &&
                        s.IsDiscountApproved == true
                    )
                );
            }
            else if (filter.Status == ReturnsDiscountsApprovalStatus.NotApproved)
            {
                query = query.Where(s =>
                    (
                        (s.ShortageTypeId == 3 || s.ShortageTypeId == 6) &&
                        (s.IsReturnApproved == false || s.IsReturnApproved == null)
                    )
                    ||
                    (
                        (s.ShortageTypeId != 3 && s.ShortageTypeId != 6) &&
                        (s.IsDiscountApproved == false || s.IsDiscountApproved == null)
                    )
                );
            }

            var data = await query.ToListAsync();

            var result = data.Select(s => new AccountsReturnsDiscountsReportRowDto
            {
                JournalDate = s.BranchSalesDaily?.SalesDate ?? DateTime.MinValue,

                BranchId = s.BranchSalesDaily?.BranchId ?? 0,
                BranchNumber = s.BranchSalesDaily?.Branch?.BranchNumber ?? 0,
                BranchName = s.BranchSalesDaily?.Branch?.BranchName ?? "غير متوفر",

                ShortageTypeId = s.ShortageTypeId,
                ShortageTypeName = s.ShortageType?.ShortageName ?? "غير متوفر",

                Amount = s.Amount ?? 0,

                IsApproved =
                    (s.ShortageTypeId == 3 || s.ShortageTypeId == 6)
                        ? (s.IsReturnApproved == true)
                        : (s.IsDiscountApproved == true),

                // ⭐ ملاحظات إدارة المرتجعات
                ReturnNotes = s.ReturnNotes
            })
            .ToList();

            return result;
        }


    }
}
