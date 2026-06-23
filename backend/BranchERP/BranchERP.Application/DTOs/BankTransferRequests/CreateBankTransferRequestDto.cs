using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BankTransferRequests
{
    public class CreateBankTransferRequestDto
    {
        public int BranchId { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public decimal InvoiceAmount { get; set; }

        public int TransferType { get; set; }

        public decimal TransferAmount { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerMobile { get; set; } = string.Empty;

        public string BankName { get; set; } = string.Empty;

        public string? AttachmentPath { get; set; }
        public string Iban { get; set; } = string.Empty;

        public string ApplicantSignature { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}
