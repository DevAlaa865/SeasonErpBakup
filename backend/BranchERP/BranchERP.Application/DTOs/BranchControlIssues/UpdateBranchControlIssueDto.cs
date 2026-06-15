using BranchERP.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BranchControlIssues
{
    public class UpdateBranchControlIssueDto
    {
        public int Id { get; set; }
        public BranchControlIssueStatus Status { get; set; }
        public ResolutionType? ResolutionType { get; set; }
        public string? ControlNotes { get; set; }
    }
}
