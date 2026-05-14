using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.Reports
{
    public class BranchPerformanceChartDto
    {
        public string BranchName { get; set; } = "";
        public decimal AchievementPercentage { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal AchievedAmount { get; set; }
        public decimal CommissionAmount { get; set; }
    }
}
