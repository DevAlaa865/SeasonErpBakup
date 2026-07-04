using BranchERP.Application.DTOs.ExpenseVouchers.CashBox;
using BranchERP.Application.DTOs.ExpenseVouchers.CashTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces.ExpenseVouchers
{
    public interface ICashBoxTransactionService
    {
        Task<bool> AddBranchDepositAsync(BranchDepositRequest dto);
        Task<bool> AssignPettyCashAsync(PettyCashAssignmentRequest dto);
        Task<bool> TransferAsync(CashBoxTransferRequest dto);
        Task<bool> AdjustAsync(CashBoxAdjustmentRequest dto);
        Task<bool> AddAdminFundingAsync(AdminFundingRequest dto);
        Task<bool> AddAdminDeductionAsync(AdminDeductionRequest dto);

        Task<List<CashBoxTransactionDto>> GetTransactionsAsync(CashBoxTransactionFilter filter);
    }
}
