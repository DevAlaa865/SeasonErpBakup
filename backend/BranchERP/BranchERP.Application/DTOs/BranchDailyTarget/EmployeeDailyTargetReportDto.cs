using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BranchDailyTarget
{
    public class EmployeeDailyTargetReportDto
    {
        public int HeaderId { get; set; }      // جديد
        public int DetailId { get; set; }      // جديد
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = "";

        public int BranchId { get; set; }
        public string BranchName { get; set; } = "";

        public int CityId { get; set; }
        public string CityName { get; set; } = "";

        public DateTime TargetDate { get; set; }

        public decimal? EmployeeTarget { get; set; }
        public decimal? EmployeeAchieved { get; set; }
        public decimal AchievementPercentage { get; set; }
    }
}
