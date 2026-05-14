using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{
    namespace BranchERP.Domain.Entities
    {
        public class BranchDailyPerformance : BaseEntity
        {
            public int BranchId { get; set; }
            public Branch Branch { get; set; } = null!;

            public DateTime TargetDate { get; set; }

            public decimal BranchTargetAmount { get; set; }          // تارجت الفرع الكلي
            public decimal? BranchAchievedAmount { get; set; }       // المنجز الفعلي

            public int? BranchInvoicesCountTarget { get; set; }
            public int? BranchInvoicesCountAchieved { get; set; }

            public int? BranchItemsCountTarget { get; set; }
            public int? BranchItemsCountAchieved { get; set; }

            public decimal? AchievementPercentage { get; set; }

            public string? CreatedBy { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            public string? UpdatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }
    }
}
