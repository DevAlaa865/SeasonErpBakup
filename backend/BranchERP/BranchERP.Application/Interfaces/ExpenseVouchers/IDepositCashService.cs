using BranchERP.Application.DTOs.ExpenseVouchers.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces.ExpenseVouchers
{
    public interface IDepositCashService
    {
        Task<DepositCashSummaryDto> GetMyCashAsync(string userId);
    }
}
