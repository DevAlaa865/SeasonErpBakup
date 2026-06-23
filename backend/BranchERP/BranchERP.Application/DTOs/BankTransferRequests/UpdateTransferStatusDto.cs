using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BankTransferRequests
{
    public class UpdateTransferStatusDto
    {
        public int RequestId { get; set; }

        public int Status { get; set; }
        public string? TransferReferenceNumber { get; set; }
    }
}
