using BranchERP.Application.DTOs.ExpenseVouchers.CashBox;
using BranchERP.Application.DTOs.ExpenseVouchers.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces.ExpenseVouchers
{
    public interface IUserService
    {
        Task<DepositCollectorDto?> GetDepositCollectorByIdAsync(int id);
        Task<List<DepositCollectorDto>> GetAllDepositCollectorsAsync(bool? isActive = null);

        Task<PettyHolderDto?> GetPettyHolderByIdAsync(int id);
        Task<List<PettyHolderDto>> GetAllPettyHoldersAsync(bool? isActive = null);

        Task<List<CashBoxDto>> GetUserCashBoxesAsync(int userId);

        Task<bool> AssignCashBoxToUserAsync(int userId, int cashBoxId);
    }
}
