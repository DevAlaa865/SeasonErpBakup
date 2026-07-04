using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers
{
    public class PostingHistoryDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string CashBoxName { get; set; } = string.Empty;
        public string CollectorName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
    }
}
