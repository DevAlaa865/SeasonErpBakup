using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.CashTransaction
{
    public class PettyCashAssignmentRequest
    {
        public int FromCashBoxId { get; set; }
        public int ToCashBoxId { get; set; }
        public int PettyHolderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Description { get; set; }
    }
}
