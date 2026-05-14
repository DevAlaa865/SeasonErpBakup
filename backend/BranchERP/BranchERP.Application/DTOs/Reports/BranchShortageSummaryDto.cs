using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.Reports
{
    public class BranchShortageSummaryDto
    {
        public int ShortageTypeId { get; set; }
        public string ShortageTypeName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

}
