using BranchERP.Application.DTOs.Reports.DailyReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces.Reports.DailyReports
{
    public interface IBranchDailyDifferenceReportService
    {
        Task<List<BranchDailyDifferenceReportDto>> GetBranchDailyDifferenceReportAsync(BranchDailyDifferenceReportFilterDto filter);
    }
}
