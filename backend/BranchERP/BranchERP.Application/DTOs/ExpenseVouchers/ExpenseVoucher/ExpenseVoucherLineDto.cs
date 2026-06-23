using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.ExpenseVoucher
{
    public class ExpenseVoucherLineDto
    {
        public int Id { get; set; }

        public int LineNumber { get; set; }

        public int ExpenseTypeId { get; set; }
        public string ExpenseTypeName { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public int? BranchId { get; set; }
        public string? BranchName { get; set; }

        public int? PettyHolderId { get; set; }
        public string? PettyHolderName { get; set; }

        public string? Description { get; set; }
        public string? AttachmentUrl { get; set; }
    }
}
