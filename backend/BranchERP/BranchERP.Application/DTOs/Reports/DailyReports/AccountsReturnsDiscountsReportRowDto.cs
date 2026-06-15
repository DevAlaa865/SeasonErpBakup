using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.Reports.DailyReports
{
    public class AccountsReturnsDiscountsReportRowDto
    {
        public DateTime JournalDate { get; set; }

        public int BranchId { get; set; }
        public int BranchNumber { get; set; }
        public string BranchName { get; set; }

        public int ShortageTypeId { get; set; }
        public string ShortageTypeName { get; set; }

        public decimal Amount { get; set; }

        public bool IsApproved { get; set; }

        public string? ReturnNotes { get; set; }

    }
}
