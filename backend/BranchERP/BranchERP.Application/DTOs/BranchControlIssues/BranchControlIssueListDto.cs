using BranchERP.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BranchControlIssues
{
    public class BranchControlIssueListDto
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int BranchNumber { get; set; }
        public DateTime SalesDate { get; set; }
        public decimal DifferenceAmount { get; set; }

        public string SentByUser { get; set; }
        public DateTime SentAt { get; set; }

        public BranchControlIssueStatus Status { get; set; }
        public ResolutionType? ResolutionType { get; set; }
        public string? ControlNotes { get; set; }
        public DateTime? ResolvedAt { get; set; }

        // ⭐ جديد
        public DifferenceDirection DifferenceDirection { get; set; }
    }
}
