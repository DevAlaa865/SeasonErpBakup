using BranchERP.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{
    public class CashBoxTransaction : BaseEntity
    {
        public int CashBoxId { get; set; }
        public TransactionDirection Direction { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }

        public int? ExpenseVoucherId { get; set; }
        public int? BranchId { get; set; }
        public int? PettyHolderId { get; set; }

        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }

        public CashBox CashBox { get; set; }
        public ExpenseVoucher ExpenseVoucher { get; set; }
        public Branch Branch { get; set; }
        public PettyHolder PettyHolder { get; set; }
    }
}
