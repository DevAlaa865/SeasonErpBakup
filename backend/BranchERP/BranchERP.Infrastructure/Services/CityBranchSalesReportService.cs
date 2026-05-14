using BranchERP.Application.DTOs.CityBranchSales;
using BranchERP.Application.Interfaces;
using BranchERP.Domain.Enums;
using BranchERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Infrastructure.Services
{
    public class CityBranchSalesReportService : ICityBranchSalesReportService
    {
        private readonly AppDbContext _db;

        public CityBranchSalesReportService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<CityBranchSalesRowDto>> GetCityBranchSalesSummaryAsync(CityBranchSalesFilterDto filter)
        {
            var query = _db.BranchSalesDailies
                .AsNoTracking()
                .Include(x => x.Branch)
                    .ThenInclude(b => b.City)
                .Include(x => x.Supervisor)
                .Where(x =>
                    x.SalesDate.Date >= filter.FromDate.Date &&
                    x.SalesDate.Date <= filter.ToDate.Date
                );

            // فلترة المدن
            if (filter.CityIds != null && filter.CityIds.Any())
            {
                query = query.Where(x => filter.CityIds.Contains(x.Branch.CityId));
            }


            // فلترة نوع النشاط
            if (filter.ActivityTypeId.HasValue)
            {
                query = query.Where(x => x.Branch.ActivityTypeId == filter.ActivityTypeId.Value);
            }

            // فلترة نوع الفرع (محل / كشك / الكل)
            // فلترة نوع الفرع (محل / كشك / الكل)
            if (!string.IsNullOrWhiteSpace(filter.BranchType) &&
                !string.Equals(filter.BranchType, "All", StringComparison.OrdinalIgnoreCase))
            {
                if (Enum.TryParse<BranchType>(filter.BranchType, out var branchTypeEnum))
                {
                    query = query.Where(x => x.Branch.BranchType == branchTypeEnum);
                }
            }



            var result = await query
                .Select(x => new CityBranchSalesRowDto
                {
                    CityId = x.Branch.CityId,
                    CityName = x.Branch.City.CityName,

                    BranchId = x.BranchId,
                    BranchName = x.Branch.BranchName,

                    TotalSales = x.TotalSales ?? 0,
                    CreditAmount = x.CreditAmount ?? 0,
                    InvoicesCount = x.TotalInvoicesCount ?? 0,
                    QuantitiesCount = x.TotalQuantities ?? 0,

                    SupervisorName = x.Supervisor != null ? x.Supervisor.FullName : "—"
                })
                .OrderBy(x => x.CityName)
                .ThenBy(x => x.BranchName)
                .ToListAsync();

            return result;
        }

        public async Task<List<CitySalesChartDto>> GetCitySalesChartAsync(CityBranchSalesFilterDto filter)
        {
            var rows = await GetCityBranchSalesSummaryAsync(filter);

            var grouped = rows
                .GroupBy(x => new { x.CityId, x.CityName })
                .Select(g => new CitySalesChartDto
                {
                    CityId = g.Key.CityId,
                    CityName = g.Key.CityName,
                    TotalSales = g.Sum(r => r.TotalSales)
                })
                .OrderBy(x => x.CityName)
                .ToList();

            return grouped;
        }
    }
}
