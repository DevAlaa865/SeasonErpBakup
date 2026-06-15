using BranchERP.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.Reports.DailyReports
{
    public class AccountsReturnsDiscountsReportFilterDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int? CityId { get; set; }
        public List<int>? BranchIds { get; set; }

        public int? ShortageTypeId { get; set; }

        public ReturnsDiscountsApprovalStatus Status { get; set; } = ReturnsDiscountsApprovalStatus.All;
    }
}
