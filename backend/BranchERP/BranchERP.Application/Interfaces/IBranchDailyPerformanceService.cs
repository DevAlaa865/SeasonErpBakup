using BranchERP.Application.DTOs.BranchDailyPerformance;
using BranchERP.Application.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces
{
    public interface IBranchDailyPerformanceService
    {
        Task<BranchDailyPerformanceDto?> GetAsync(int branchId, DateTime targetDate);

        Task<BranchDailyPerformanceDto> CreateOrUpdateTargetAsync(BranchDailyPerformanceCreateUpdateDto dto);

        Task<BranchDailyPerformanceDto> CreateOrUpdateAchievementAsync(BranchDailyPerformanceCreateUpdateDto dto);
        Task ImportDailyTargetsFromExcelAsync(Stream fileStream);
        Task<List<BranchDailyPerformanceReportRowDto>> GetBranchDailyPerformanceReportAsync(BranchDailyPerformanceReportFilterDto filter);
        Task<byte[]> ExportBranchDailyPerformanceReportToExcelAsync(BranchDailyPerformanceReportFilterDto filter);

        Task<List<BranchPerformanceChartDto>> GetBranchPerformanceChartAsync(BranchDailyPerformanceReportFilterDto filter);
    }
}
