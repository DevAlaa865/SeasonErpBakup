using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.CashTransaction
{
    public class BranchDepositRequest
    {
        public int CashBoxId { get; set; }
        public int BranchId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Description { get; set; }
        public string? ReferenceNumber { get; set; }
    }
}
