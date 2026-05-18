using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BranchSalesDaily
{
    public class BranchSalesDailyListRowDto
    {
        public int Id { get; set; }

        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;

        public DateTime SalesDate { get; set; }

        public decimal? CashAmount { get; set; }
        public decimal? NetworkAmount { get; set; }
        public decimal? CreditAmount { get; set; }

        public decimal? TotalSales { get; set; }
        public decimal? GrandTotal { get; set; }
        public decimal? Difference { get; set; }
    }
}
