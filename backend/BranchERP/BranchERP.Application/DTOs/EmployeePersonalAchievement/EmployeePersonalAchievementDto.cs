using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.EmployeePersonalAchievement
{
    public class EmployeePersonalAchievementDto
    {
        public int Id { get; set; }

        public int EmployeePersonalTargetId { get; set; }

        public decimal AchievedAmount { get; set; }
        public int? InvoicesCount { get; set; }
        public int? ItemsCount { get; set; }

        public decimal AchievementPercentage { get; set; }
        public bool IsTargetAchieved { get; set; }

        public string? AttachmentPath { get; set; }

        public decimal CommissionAmount { get; set; }
    }
}
