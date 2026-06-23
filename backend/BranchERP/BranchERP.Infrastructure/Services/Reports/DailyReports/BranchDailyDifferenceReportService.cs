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

        public async Task<List<BranchDailyDifferenceReportDto>> GetBranchDailyDifferenceReportAsync(
            BranchDailyDifferenceReportFilterDto filter)
        {
            // 1) الفروع أولاً (عشان نضمن ظهور الكل)
            var branchesQuery = _context.Branches.AsQueryable();

            if (filter.CityIds != null && filter.CityIds.Any())
                branchesQuery = branchesQuery.Where(x => filter.CityIds.Contains(x.CityId));

            if (filter.BranchIds != null && filter.BranchIds.Any())
                branchesQuery = branchesQuery.Where(x => filter.BranchIds.Contains(x.Id));

            if (filter.BranchNumber.HasValue)
                branchesQuery = branchesQuery.Where(x => x.BranchNumber == filter.BranchNumber.Value);

            var branches = await branchesQuery.ToListAsync();

            // 2) تحديد فترة التاريخ
            var fromDate = filter.FromDate?.Date;
            var toDate = filter.ToDate?.Date;

            var dates = new List<DateTime>();

            if (fromDate.HasValue && toDate.HasValue)
            {
                for (var d = fromDate.Value; d <= toDate.Value; d = d.AddDays(1))
                    dates.Add(d);
            }
            else
            {
                // لو مفيش فلترة تاريخ، خد اليوم الحالي فقط (أو عدل حسب احتياجك)
                dates.Add(DateTime.Today);
            }

            // 3) تحميل اليوميات مرة واحدة
            var salesQuery = _context.BranchSalesDailies.AsQueryable();

            if (fromDate.HasValue)
                salesQuery = salesQuery.Where(x => x.SalesDate >= fromDate);

            if (toDate.HasValue)
                salesQuery = salesQuery.Where(x => x.SalesDate <= toDate);

            var sales = await salesQuery.ToListAsync();

            // 4) LEFT JOIN (فرع × أيام)
            var result = (
                from b in branches
                from d in dates
                join s in sales
                    on new { b.Id, Date = d.Date }
                    equals new { Id = s.BranchId, Date = s.SalesDate.Date }
                    into gj
                from s in gj.DefaultIfEmpty()
                select new BranchDailyDifferenceReportDto
                {
                    BranchId = b.Id,
                    BranchNumber = b.BranchNumber,
                    BranchName = b.BranchName,
                    SalesDate = d,

                    Difference = filter.IsNetworkReport == true
                        ? (s?.Difference ?? 0)
                        : (s?.Difference ?? 0),

                    NetworkAmount = s?.NetworkAmount ?? 0,
                    SalesDailyId = s?.Id ?? 0
                }
            ).ToList();

            // 5) الفلاتر بتاعة الفرق (بعد ما ضمنّا وجود الصفوف)
            if (filter.IsNetworkReport != true)
            {
                if (filter.IsAllowedShortage == true)
                    result = result.Where(x => x.Difference < 0 && x.Difference >= -35).ToList();

                if (filter.IsBigShortage == true)
                    result = result.Where(x => x.Difference < -35).ToList();

                if (filter.IsSmallIncrease == true)
                    result = result.Where(x => x.Difference > 0 && x.Difference <= 35).ToList();

                if (filter.IsBigIncrease == true)
                    result = result.Where(x => x.Difference > 35).ToList();

                if (filter.IsAllowedShortage != true &&
                    filter.IsBigShortage != true &&
                    filter.IsSmallIncrease != true &&
                    filter.IsBigIncrease != true)
                {
                    result = result.Where(x => x.Difference != 0).ToList();
                }
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
