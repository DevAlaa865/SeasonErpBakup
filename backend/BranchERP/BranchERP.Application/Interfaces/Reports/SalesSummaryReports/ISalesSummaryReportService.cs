using BranchERP.Application.DTOs.Reports.SalesSummaryReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces.Reports.SalesSummaryReports
{
    public interface ISalesSummaryReportService
    {
        Task<List<ReportResultDto>> GetReportAsync(ReportFilterDto filter);
    }
}
