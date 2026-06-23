using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BankTransferRequests
{
    public class UpdateAttachmentDto
    {
        public int RequestId { get; set; }
        public string AttachmentPath { get; set; }
    }
}
