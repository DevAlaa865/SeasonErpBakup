using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces.ExpenseVouchers
{
    public interface IUserBranchService
    {
        Task<List<BranchDto>> GetMyBranchesAsync(string userId);
    }
}
