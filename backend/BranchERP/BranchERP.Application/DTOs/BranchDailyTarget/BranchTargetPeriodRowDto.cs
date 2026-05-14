using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BranchDailyTarget
{
    public class BranchTargetPeriodRowDto
    {
        public DateTime TargetDate { get; set; }
        public decimal TotalTarget { get; set; }
        public decimal TotalAchieved { get; set; }
        public decimal AchievementPercentage { get; set; }
    }
}
