using BranchERP.Application.DTOs.ExpenseVouchers.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces.ExpenseVouchers
{
    public interface IDepositCollectorService
    {
        Task<DepositCollectorDto> CreateAsync(CreateDepositCollectorDto dto);

        Task<bool> UpdateAsync(UpdateDepositCollectorDto dto);

        Task<DepositCollectorDto?> GetByIdAsync(int id);

        Task<List<DepositCollectorDto>> GetAllAsync(bool? isActive = null);

        Task<bool> SetActiveAsync(int id, bool isActive);
    }
}
