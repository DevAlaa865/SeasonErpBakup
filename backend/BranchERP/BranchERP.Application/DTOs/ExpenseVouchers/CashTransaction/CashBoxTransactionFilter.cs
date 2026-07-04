using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.CashTransaction
{
    public class CashBoxTransactionFilter
    {
        public int CashBoxId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? TransactionType { get; set; }
        public int? BranchId { get; set; }
        public int? PettyHolderId { get; set; }
    }
}
