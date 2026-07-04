using BranchERP.Application.DTOs.ExpenseVouchers.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces.ExpenseVouchers
{
    public interface IExpenseTypeService
    {
        Task<List<ExpenseTypeDto>> GetAllAsync(bool? isActive = null);
        Task<ExpenseTypeDto?> GetByIdAsync(int id);
        Task<ExpenseTypeDto> CreateAsync(ExpenseTypeDto dto);
        Task<bool> UpdateAsync(ExpenseTypeDto dto);
        Task<bool> SetActiveAsync(int id, bool isActive);
    }
}
