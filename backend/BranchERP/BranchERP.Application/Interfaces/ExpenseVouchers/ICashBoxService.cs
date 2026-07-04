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
        Task<CashBoxDto> CreateAsync(CreateCashBoxDto dto);
        Task<bool> UpdateAsync(UpdateCashBoxDto dto);

        Task<CashBoxDto?> GetByIdAsync(int id);
        Task<List<CashBoxDto>> GetAllAsync(bool? isActive = null);

        Task<decimal> GetBalanceAsync(int cashBoxId);
        Task<List<CashBoxTransactionDto>> GetTransactionsAsync(int cashBoxId);

        Task<bool> SetActiveAsync(int id, bool isActive);

        // لو هتستخدمهم لاحقًا
        Task<List<CashBoxTransactionDto>> GetTransactionsAsync(int cashBoxId, DateTime? fromDate, DateTime? toDate);
        Task<decimal> GetCurrentBalanceAsync(int cashBoxId);
        Task<bool> ActivateAsync(int id);
        Task<bool> DeactivateAsync(int id);
        Task<bool> AddManualTransactionAsync(int cashBoxId, decimal amount, string direction, string type, string? description = null);
        // الجديد — الصناديق الخاصة بالمستخدم
        Task<List<CashBoxDto>> GetCashBoxesForUserAsync(string userId);
    }
}
