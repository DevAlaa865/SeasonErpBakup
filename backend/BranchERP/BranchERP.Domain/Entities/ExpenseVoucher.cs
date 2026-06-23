using BranchERP.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{
    public class ExpenseVoucher : BaseEntity
    {
        public string VoucherNumber { get; set; }
        public DateTime VoucherDate { get; set; }

        public int CashBoxId { get; set; }
        public decimal TotalAmount { get; set; }

        public string CreatedByUserId { get; set; }
        public VoucherStatus Status { get; set; }

        public string ApprovedByUserId { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public string Description { get; set; }
        public string AttachmentUrl { get; set; }

        public CashBox CashBox { get; set; }
        public ICollection<ExpenseVoucherLine> Lines { get; set; } = new List<ExpenseVoucherLine>();
    }
}
