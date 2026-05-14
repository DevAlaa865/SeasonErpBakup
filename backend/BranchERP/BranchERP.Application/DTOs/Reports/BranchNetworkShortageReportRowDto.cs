using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.Reports
{
    public class BranchNetworkShortageReportRowDto
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public decimal? NetworkAmount { get; set; }
        public List<BranchShortageSummaryDto> Shortages { get; set; } = new();
    }






}
