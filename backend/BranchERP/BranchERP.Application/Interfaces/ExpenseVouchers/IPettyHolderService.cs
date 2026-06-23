using BranchERP.Application.DTOs.ExpenseVouchers.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces.ExpenseVouchers
{
    public interface IPettyHolderService
    {
        Task<PettyHolderDto> CreateAsync(CreatePettyHolderDto dto);

        Task<bool> UpdateAsync(UpdatePettyHolderDto dto);

        Task<PettyHolderDto?> GetByIdAsync(int id);

        Task<List<PettyHolderDto>> GetAllAsync(bool? isActive = null);

        Task<bool> SetActiveAsync(int id, bool isActive);
    }
}
