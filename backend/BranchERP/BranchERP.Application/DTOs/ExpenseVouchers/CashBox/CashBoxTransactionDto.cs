using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.CashBox
{
    public class CashBoxTransactionDto
    {
        public int Id { get; set; }

        public int CashBoxId { get; set; }

        public string Direction { get; set; } = string.Empty;   // IN / OUT

        public decimal Amount { get; set; }

        public string Type { get; set; } = string.Empty;        // Expense / Transfer / BranchDeposit ...

        public DateTime TransactionDate { get; set; }

        public string? Description { get; set; }

        public int? ExpenseVoucherId { get; set; }
        public string? ExpenseVoucherNumber { get; set; }

        public int? BranchId { get; set; }
        public string? BranchName { get; set; }

        public int? PettyHolderId { get; set; }
        public string? PettyHolderName { get; set; }
    }
}
