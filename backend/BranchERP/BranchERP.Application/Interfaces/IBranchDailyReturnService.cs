using BranchERP.Application.DTOs.BranchDailyReturnDto;

namespace BranchERP.Application.Interfaces
{
    public interface IBranchDailyReturnService
    {
        Task ImportFromExcelAsync(Stream fileStream, string fileName);

        // ============================================================
        // GET RETURNS (FINAL - MULTI FILTER ONLY)
        // ============================================================
        Task<List<BranchDailyReturnDto>> GetReturnsAsync(
            DateTime? fromDate,
            DateTime? toDate,
            List<int>? cityIds,
            List<int>? branchIds,
            int? returnType
        );

        Task<BranchDailyReturnDto?> GetByIdAsync(int id);

        Task<bool> UpdateAsync(int id, BranchDailyReturnUpdateDto dto, string userName);

        // ============================================================
        // EXPORT
        // ============================================================
        Task<byte[]> ExportToExcelAsync(
            DateTime? fromDate,
            DateTime? toDate,
            List<int>? cityIds,
            List<int>? branchIds,
            int? returnType
        );

        // ============================================================
        // CHART
        // ============================================================
        Task<List<BranchDailyReturnChartDto>> GetChartDataAsync(
            DateTime? fromDate,
            DateTime? toDate,
            List<int>? cityIds,
            List<int>? branchIds,
            int? returnType
        );
    }
}