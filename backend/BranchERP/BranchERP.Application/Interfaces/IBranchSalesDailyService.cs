using BranchERP.Application.DTOs.BranchSalesDaily;
using BranchERP.Application.DTOs.Common;
using BranchERP.Application.DTOs.Reports;

namespace BranchERP.Application.Interfaces
{
    public interface IBranchSalesDailyService
    {
        Task<ApiResponse<BranchSalesDailyDto>> GetByIdAsync(int id);

        Task<ApiResponse<IReadOnlyList<BranchSalesDailyDto>>> GetByBranchAndDateAsync(
            int branchId,
            DateTime date);

        Task<ApiResponse<BranchSalesDailyDto>> CreateAsync(BranchSalesDailyCreateUpdateDto model);
        Task<ApiResponse<BranchSalesDailyDto>> UpdateAsync(int id, BranchSalesDailyCreateUpdateDto model);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        public  Task<bool> ExistsAsync(int branchId, DateTime date);
        Task<List<BranchDailySummaryRowDto>> GetBranchDailySummaryAsync(BranchDailySummaryFilterDto filter);
        Task<List<ReturnsDiscountsManagementRowDto>> GetReturnsDiscountsManagementAsync(ReturnsDiscountsManagementFilterDto filter);

        Task<ApiResponse<IReadOnlyList<BranchDailySummaryRowDto>>> GetSummaryReportAsync(BranchDailySummaryFilterDto filter);

        Task<ApiResponse<bool>> UpdateShortagesApprovalsAsync(List<ShortageApprovalUpdateDto> items);
        Task<List<BranchNetworkShortageReportRowDto>> GetBranchNetworkShortagesAsync(BranchNetworkShortageFilterDto filter);

        Task<List<BranchSalesDailyListRowDto>> SearchAsync(BranchSalesDailySearchFilterDto filter);

    }
}
