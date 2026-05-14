using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BranchDailyPerformance
{
    public class BranchDailyPerformanceCreateUpdateDto
    {
        public int BranchId { get; set; }
        public DateTime TargetDate { get; set; }

        public decimal? BranchTargetAmount { get; set; }
        public decimal? BranchAchievedAmount { get; set; }

        public int? BranchInvoicesCountTarget { get; set; }
        public int? BranchInvoicesCountAchieved { get; set; }

        public int? BranchItemsCountTarget { get; set; }
        public int? BranchItemsCountAchieved { get; set; }
    }
}
