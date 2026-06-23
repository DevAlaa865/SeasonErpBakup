using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.ExpenseVoucher
{
    public class ExpenseVoucherDto
    {
        public int Id { get; set; }

        public string VoucherNumber { get; set; } = string.Empty;
        public DateTime VoucherDate { get; set; }

        public int CashBoxId { get; set; }
        public decimal TotalAmount { get; set; }

        public string CreatedByUserId { get; set; } = string.Empty;
        public string? CreatedByUserName { get; set; }

        public string? ApprovedByUserId { get; set; }
        public string? ApprovedByUserName { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public string? Description { get; set; }
        public string? AttachmentUrl { get; set; }

        public string Status { get; set; } = string.Empty;

        public List<ExpenseVoucherLineDto> Lines { get; set; }
            = new List<ExpenseVoucherLineDto>();
    }
}
