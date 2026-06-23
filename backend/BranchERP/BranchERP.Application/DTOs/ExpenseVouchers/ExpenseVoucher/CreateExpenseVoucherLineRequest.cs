using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.ExpenseVoucher
{
    public class CreateExpenseVoucherLineRequest
    {
        public int ExpenseTypeId { get; set; }

        public decimal Amount { get; set; }

        public int? BranchId { get; set; }

        public int? PettyHolderId { get; set; }

        public string? Description { get; set; }

        public string? AttachmentUrl { get; set; }
    }
}
