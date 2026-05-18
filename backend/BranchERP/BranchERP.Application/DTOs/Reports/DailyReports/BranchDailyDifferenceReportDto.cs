using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.Reports.DailyReports
{
    public class BranchDailyDifferenceReportDto
    {
        public int BranchId { get; set; }
        public int BranchNumber { get; set; }     // 🔥 جديد
        public string BranchName { get; set; }
        public DateTime SalesDate { get; set; }   // 🔥 جديد
        public decimal Difference { get; set; }
        public decimal NetworkAmount { get; set; }
    }
}
