using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{
    public class EmployeePersonalAchievement : BaseEntity
    {
        public int EmployeePersonalTargetId { get; set; }
        public EmployeePersonalTarget EmployeePersonalTarget { get; set; } = null!;

        public decimal AchievedAmount { get; set; }
        public int? InvoicesCount { get; set; }
        public int? ItemsCount { get; set; }

        public decimal AchievementPercentage { get; set; }
        public bool IsTargetAchieved { get; set; }

        public string? AttachmentPath { get; set; }   // مسار السند/الصورة

        public decimal CommissionAmount { get; set; } // العمولة المحسوبة

        public string? EnteredBy { get; set; }
        public DateTime EnteredAt { get; set; } = DateTime.UtcNow;
    }
}
