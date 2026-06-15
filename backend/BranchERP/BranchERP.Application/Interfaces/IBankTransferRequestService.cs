using BranchERP.Application.DTOs.BankTransferRequests;
using BranchERP.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces
{
    public interface IBankTransferRequestService
    {
        Task<ApiResponse<BankTransferRequestDto>> CreateAsync(CreateBankTransferRequestDto dto);

        Task<ApiResponse<BankTransferRequestDto>> GetByIdAsync(int id);

        Task<ApiResponse<IReadOnlyList<BankTransferRequestDto>>> SearchAsync(
            BankTransferRequestFilterDto filter);

        Task<ApiResponse<bool>> UpdateStatusAsync(
            UpdateTransferStatusDto dto,
            string processedBy);
    }
}
