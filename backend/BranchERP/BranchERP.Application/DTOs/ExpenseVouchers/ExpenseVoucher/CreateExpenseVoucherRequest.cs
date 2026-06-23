using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.ExpenseVoucher
{
    public class CreateExpenseVoucherRequest
    {
        public DateTime VoucherDate { get; set; }

        public int CashBoxId { get; set; }

        public string CreatedByUserId { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? AttachmentUrl { get; set; }

        public List<CreateExpenseVoucherLineRequest> Lines { get; set; }
            = new List<CreateExpenseVoucherLineRequest>();

        public bool Submit { get; set; }   // true = Submitted, false = Draft
    }
}
