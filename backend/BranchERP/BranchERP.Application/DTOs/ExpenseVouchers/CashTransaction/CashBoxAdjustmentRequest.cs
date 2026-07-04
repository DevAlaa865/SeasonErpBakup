using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.CashTransaction
{
    public class CashBoxAdjustmentRequest
    {
        public int CashBoxId { get; set; }
        public decimal Amount { get; set; }
        public string Direction { get; set; } // "IN" or "OUT"
        public DateTime TransactionDate { get; set; }
        public string Reason { get; set; }
    }
}
