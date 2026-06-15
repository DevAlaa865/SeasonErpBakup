using BranchERP.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BranchControlIssues
{
    public class CreateBranchControlIssueDto
    {
        public int BranchId { get; set; }
        public int SalesDailyId { get; set; }
        public DateTime SalesDate { get; set; }
        public decimal DifferenceAmount { get; set; }

        public string? SentByUser { get; set; }

        public DifferenceDirection DifferenceDirection { get; set; }
    }
}
