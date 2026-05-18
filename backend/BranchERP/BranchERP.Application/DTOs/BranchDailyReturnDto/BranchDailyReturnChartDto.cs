using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BranchDailyReturnDto
{
    public class BranchDailyReturnChartDto
    {
        public string BranchName { get; set; } = string.Empty;

        public decimal Cash { get; set; }
        public decimal Replacement { get; set; }
        public decimal Tabby { get; set; }
        public decimal Tamara { get; set; }
    }
}
