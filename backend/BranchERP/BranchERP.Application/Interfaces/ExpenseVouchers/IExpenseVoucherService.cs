using BranchERP.Application.DTOs.ExpenseVouchers.ExpenseVoucher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces.ExpenseVouchers
{
    public interface IExpenseVoucherService
    {
        Task<ExpenseVoucherDto> CreateAsync(CreateExpenseVoucherRequest request);

        Task<ExpenseVoucherDto> ApproveAsync(ApproveExpenseVoucherRequest request);

        Task<ExpenseVoucherDto?> GetByIdAsync(int id);

        Task<List<ExpenseVoucherDto>> GetAllAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int? cashBoxId = null,
            string? status = null
        );

        Task<bool> DeleteAsync(int id);

        Task<bool> SubmitAsync(int id);   // تحويل من Draft → Submitted
        Task<List<ExpenseVoucherDto>> GetMyVouchersAsync(string userId);
    }

}
