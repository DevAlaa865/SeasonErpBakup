using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BankTransferRequests
{
    public class BankTransferRequestFilterDto
    {
        public string? RequestNumber { get; set; }

        public int? BranchId { get; set; }

        public string? InvoiceNumber { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerMobile { get; set; }

        public string? Iban { get; set; }

        public int? Status { get; set; }

        public DateTime? FromRequestDate { get; set; }

        public DateTime? ToRequestDate { get; set; }

        public DateTime? FromTransferDate { get; set; }

        public DateTime? ToTransferDate { get; set; }
    }
}
