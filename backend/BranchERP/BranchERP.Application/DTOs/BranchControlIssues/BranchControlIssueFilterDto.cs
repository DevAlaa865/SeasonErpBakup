using BranchERP.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BranchControlIssues
{
    public class BranchControlIssueFilterDto
    {
        public int? BranchId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public BranchControlIssueStatus? Status { get; set; }
        public ResolutionType? ResolutionType { get; set; }

        // ⭐ جديد: فلتر نوع المبلغ (عجز / زيادة)
        public DifferenceDirection? DifferenceDirection { get; set; }
    }
}
