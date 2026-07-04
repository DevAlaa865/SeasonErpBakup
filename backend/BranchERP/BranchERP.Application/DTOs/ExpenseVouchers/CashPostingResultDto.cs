using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers
{
    public class CashPostingResultDto
    {
        public string UserId { get; set; }
        public DateTime Date { get; set; }

        public List<CashPostingCityResultDto> Cities { get; set; } = new();
    }
    public class CashPostingCityResultDto
    {
        public int CityId { get; set; }
        public string CityName { get; set; }

        public int CashBoxId { get; set; }
        public string CashBoxName { get; set; }

        public decimal TotalDailyCash { get; set; }

        public bool AlreadyPosted { get; set; }

        public bool HasMissingBranches { get; set; }
        public List<string> MissingBranches { get; set; } = new();
    }

}
