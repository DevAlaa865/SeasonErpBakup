using BranchERP.Application.DTOs.BranchDailyReturnDto;

namespace Application.Interfaces
{
    public interface IBranchDailyReturnService
    {
        Task ImportFromExcelAsync(Stream fileStream, string fileName);

        Task<List<BranchDailyReturnDto>> GetReturnsAsync(
            DateTime? fromDate,
            DateTime? toDate,
            int? branchId,
            int? branchNumber,
            int? cityId,
            int? returnType
        );

        Task<BranchDailyReturnDto?> GetByIdAsync(int id);

        Task<bool> UpdateAsync(int id, BranchDailyReturnUpdateDto dto, string userName);

        Task<byte[]> ExportToExcelAsync(
                DateTime? fromDate,
                DateTime? toDate,
                int? branchId,
                int? branchNumber,
                int? cityId,
                int? returnType
            );
        Task<List<BranchDailyReturnChartDto>> GetChartDataAsync(
                DateTime? fromDate,
                DateTime? toDate,
                int? branchId,
                int? branchNumber,
                int? cityId,
                int? returnType
            );
    }
}
