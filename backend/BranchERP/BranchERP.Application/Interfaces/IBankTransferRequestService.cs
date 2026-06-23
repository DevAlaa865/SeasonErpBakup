using BranchERP.Application.DTOs.BankTransferRequests;
using BranchERP.Application.DTOs.Common;

namespace BranchERP.Application.Interfaces
{
    public interface IBankTransferRequestService
    {
        Task<ApiResponse<BankTransferRequestDto>> CreateAsync(
            CreateBankTransferRequestDto dto,
            string createdBy);

        Task<ApiResponse<BankTransferRequestDto>> GetByIdAsync(int id);

        Task<ApiResponse<IReadOnlyList<BankTransferRequestDto>>> GetPendingAsync();

        Task<ApiResponse<IReadOnlyList<BankTransferRequestDto>>> SearchAsync(
            BankTransferRequestFilterDto filter);

        Task<ApiResponse<bool>> UpdateStatusAsync(
            UpdateTransferStatusDto dto,
            string processedBy);

        Task UpdateAttachmentAsync(UpdateAttachmentDto dto);
    }
}