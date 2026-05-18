using BranchERP.Application.DTOs.Reports.DailyReports;
using BranchERP.Application.Interfaces.Reports.DailyReports;
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
            if (filter.CityId.HasValue)
                query = query.Where(x => x.Branch.CityId == filter.CityId.Value);

            // فلترة الفروع
            if (filter.BranchIds != null && filter.BranchIds.Any())
                query = query.Where(x => filter.BranchIds.Contains(x.BranchId));

            // فلترة رقم الفرع
            if (filter.BranchNumber.HasValue)
                query = query.Where(x => x.Branch.BranchNumber == filter.BranchNumber.Value);

            // استبعاد الفرق = صفر
            query = query.Where(x => x.Difference != 0);

            // 🔥 فلترة الفرق الجديدة

            // 1) عجز مسموح به: من -35 إلى -1
            if (filter.IsAllowedShortage == true)
                query = query.Where(x => x.Difference < 0 && x.Difference >= -35);

            // 2) عجز كبير: أقل من -35
            if (filter.IsBigShortage == true)
                query = query.Where(x => x.Difference < -35);

            // 3) زيادة: أكبر من 0
            if (filter.IsIncrease == true)
                query = query.Where(x => x.Difference > 0);

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
                NetworkAmount = x.NetworkAmount ?? 0
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
    }
}
