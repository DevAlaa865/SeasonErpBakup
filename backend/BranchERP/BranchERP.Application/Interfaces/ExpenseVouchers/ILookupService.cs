using BranchERP.Application.DTOs.ExpenseVouchers.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces.ExpenseVouchers
{
    public interface ILookupService
    {
        Task<List<ExpenseTypeDto>> GetExpenseTypesAsync(bool? isActive = null);

        Task<List<BranchDto>> GetBranchesAsync(bool? isActive = null);

    }
}
