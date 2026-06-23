using BranchERP.Application.DTOs.Reports.SalesSummaryReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces.Reports.SalesSummaryReports
{
    public interface IBranchDailyDetailsService
    {
        Task<BranchDailyDetailReportResponse> GetBranchDailyDetailsAsync(
            int branchId, DateTime fromDate, DateTime toDate);
    }
}
