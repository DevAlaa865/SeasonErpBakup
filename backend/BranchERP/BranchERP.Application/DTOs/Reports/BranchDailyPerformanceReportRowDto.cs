using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.Reports
{
    public class BranchDailyPerformanceReportRowDto
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;

        public DateTime TargetDate { get; set; }

        public decimal TargetAmount { get; set; }
        public decimal AchievedAmount { get; set; }

        public decimal AchievementPercentage { get; set; }   // نسبة الإنجاز
        public decimal CommissionAmount { get; set; }        // عمولة الفرع
    }
}
