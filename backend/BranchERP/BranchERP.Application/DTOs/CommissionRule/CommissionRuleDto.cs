using BranchERP.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.CommissionRule
{
    public class CommissionRuleDto
    {
        public int Id { get; set; }

        public decimal MinPercentage { get; set; }
        public decimal? MaxPercentage { get; set; }

        public decimal? FixedBonusAmount { get; set; }
        public CommissionType Type { get; set; }
        public bool IsActive { get; set; }
    }

}
