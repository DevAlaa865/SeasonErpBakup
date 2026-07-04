using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers
{
    public class ManualPostingResultDto
    {
        public bool Success { get; set; }
        public string BranchName { get; set; }
        public string CityName { get; set; }
        public string CashBoxName { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
