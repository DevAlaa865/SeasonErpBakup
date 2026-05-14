using BranchERP.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{
    public class CommissionRule : BaseEntity
    {
        public decimal MinPercentage { get; set; }
        public decimal? MaxPercentage { get; set; }

        public decimal? FixedBonusAmount { get; set; }    // مبلغ العمولة فقط

        public CommissionType Type { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
    }

}
