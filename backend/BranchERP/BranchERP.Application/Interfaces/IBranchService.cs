using BranchERP.Application.DTOs.Common;

namespace BranchERP.Application.Interfaces
{
    public interface IBranchService
    {
        Task<ApiResponse<IReadOnlyList<BranchDto>>> GetAllAsync();

        Task<ApiResponse<IReadOnlyList<BranchDto>>> GetPagedAsync(
            int pageIndex,
            int pageSize,
            string? search = null);

        Task<ApiResponse<BranchDto>> GetByIdAsync(int id);

        Task<ApiResponse<BranchDto>> CreateAsync(BranchCreateUpdateDto model);

        Task<ApiResponse<BranchDto>> UpdateAsync(int id, BranchCreateUpdateDto model);

        Task<ApiResponse<bool>> DeleteAsync(int id);

        Task<ApiResponse<IReadOnlyList<BranchDto>>> GetByCityIdAsync(int cityId);

        Task<ApiResponse<IReadOnlyList<BranchDto>>> GetByCityIdsAsync(List<int> cityIds);
    }
}
