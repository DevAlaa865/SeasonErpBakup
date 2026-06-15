using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BankTransferRequests
{
    public class BankTransferRequestDto
    {
        public int Id { get; set; }

        public string RequestNumber { get; set; } = string.Empty;

        public DateTime RequestDate { get; set; }

        public int BranchId { get; set; }

        public int BranchNumber { get; set; }

        public string BranchName { get; set; } = string.Empty;

        public string InvoiceNumber { get; set; } = string.Empty;

        public decimal InvoiceAmount { get; set; }

        public int TransferType { get; set; }

        public decimal TransferAmount { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerMobile { get; set; } = string.Empty;

        public string BankName { get; set; } = string.Empty;

        public string Iban { get; set; } = string.Empty;

        public int Status { get; set; }

        public DateTime? TransferDate { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public string? ProcessedBy { get; set; }

        public string ApplicantSignature { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}
