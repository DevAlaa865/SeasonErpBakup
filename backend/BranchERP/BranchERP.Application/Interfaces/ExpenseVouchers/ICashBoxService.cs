using BranchERP.Application.DTOs.ExpenseVouchers.CashBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces.ExpenseVouchers
{
    public interface ICashBoxService
    {
        Task<CashBoxDto?> GetByIdAsync(int id);

        Task<List<CashBoxDto>> GetAllAsync(bool? isActive = null);

        Task<List<CashBoxTransactionDto>> GetTransactionsAsync(
            int cashBoxId,
            DateTime? fromDate = null,
            DateTime? toDate = null
        );

        Task<decimal> GetCurrentBalanceAsync(int cashBoxId);

        Task<bool> ActivateAsync(int id);
        Task<bool> DeactivateAsync(int id);

        Task<bool> AddManualTransactionAsync(
            int cashBoxId,
            decimal amount,
            string direction,   // IN / OUT
            string type,        // Adjustment / Manual
            string? description = null
        );
    }
}
