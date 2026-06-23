using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{
    public class ExpenseVoucherLine : BaseEntity
    {
        public int ExpenseVoucherId { get; set; }
        public int LineNumber { get; set; }

        public int ExpenseTypeId { get; set; }
        public decimal Amount { get; set; }

        public int? BranchId { get; set; }
        public int? PettyHolderId { get; set; }

        public string Description { get; set; }
        public string AttachmentUrl { get; set; }

        public ExpenseVoucher ExpenseVoucher { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public Branch Branch { get; set; }
        public PettyHolder PettyHolder { get; set; }
    }
}
