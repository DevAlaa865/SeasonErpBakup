using BranchERP.Application.DTOs.Reports.SalesSummaryReport;
using BranchERP.Application.Interfaces;
using BranchERP.Application.Interfaces.Reports.SalesSummaryReports;
using BranchERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class SalesSummaryReportService : ISalesSummaryReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContext;

    public SalesSummaryReportService(IUnitOfWork unitOfWork, IUserContextService userContext)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<List<ReportResultDto>> GetReportAsync(ReportFilterDto filter)
    {
        var salesRepo = _unitOfWork.Repository<BranchSalesDaily>();
        var returnsRepo = _unitOfWork.Repository<BranchDailyReturn>();

        var from = filter.FromDate.Date;
        var to = filter.ToDate.Date.AddDays(1).AddTicks(-1);

        var salesQuery = salesRepo.Query()
            .Include(s => s.Branch)
                .ThenInclude(b => b.City)
            .Where(s => s.SalesDate >= from && s.SalesDate <= to);

        // ============================================
        // 🔥 فلترة حسب نوع المستخدم من التوكن
        // ============================================

        // 1) مستخدم فرع → يشوف فرعه فقط
        if (_userContext.UserType == "Branch")
        {
            salesQuery = salesQuery.Where(s => s.BranchId == _userContext.BranchId);
        }

        // 2) مدير منطقة → يشوف المدن اللي في التوكن فقط
        if (_userContext.UserType == "RegionManager")
        {
            var cityIds = _userContext.CityIds;

            if (cityIds != null && cityIds.Any())
            {
                salesQuery = salesQuery.Where(s => cityIds.Contains(s.Branch.CityId));
            }
        }

        // 3) مستخدم مركزي → يشوف كل شيء (لا فلترة)

        // ============================================
        // 🔥 الفلاتر الأصلية (RegionId – CityId – BranchId)
        // ============================================

        if (filter.RegionId.HasValue)
            salesQuery = salesQuery.Where(s => s.Branch.City.RegionId == filter.RegionId.Value);

        if (filter.CityId.HasValue)
            salesQuery = salesQuery.Where(s => s.Branch.CityId == filter.CityId.Value);

        if (filter.BranchId.HasValue)
            salesQuery = salesQuery.Where(s => s.BranchId == filter.BranchId.Value);

        // ============================================
        // 🔥 تنفيذ التقرير
        // ============================================

        var salesList = await salesQuery.ToListAsync();

        if (!salesList.Any())
            return new List<ReportResultDto>();

        var result = new List<ReportResultDto>();

        foreach (var group in salesList.GroupBy(s => s.BranchId))
        {
            var branchSales = group.ToList();
            var branch = branchSales.First().Branch;

            var totalSales = branchSales.Sum(x => x.GrandTotal ?? 0m);
            var invoiceCount = branchSales.Sum(x => x.TotalInvoicesCount ?? 0);
            var quantityCount = branchSales.Sum(x => x.TotalQuantities ?? 0);
            var activityType = branch.ActivityTypeId.ToString();

            var returnsList = await returnsRepo.Query()
                .Where(r => r.BranchId == branch.Id &&
                            r.ReturnDate >= from &&
                            r.ReturnDate <= to)
                .ToListAsync();

            var totalReturns = returnsList.Any()
                ? returnsList.Sum(r => r.ReturnAmount)
                : 0m;

            result.Add(new ReportResultDto
            {
                BranchId = branch.Id,
                BranchNumber = branch.BranchNumber,
                BranchName = branch.BranchName,
                TotalSales = totalSales,
                TotalReturns = totalReturns,
                InvoiceCount = invoiceCount,
                QuantityCount = quantityCount,
                ActivityType = activityType
            });
        }

        return result;
    }
}
