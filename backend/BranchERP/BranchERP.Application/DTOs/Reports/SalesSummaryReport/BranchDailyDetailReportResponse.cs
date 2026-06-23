using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.Reports.SalesSummaryReport
{
    public class BranchDailyDetailReportResponse
    {
        public List<BranchDailyDetailDto> Items { get; set; } = new();

        public decimal TotalSales { get; set; }
        public decimal TotalReturns { get; set; }
        public decimal NetSales { get; set; }
        public int InvoiceCount { get; set; }
        public int QuantityCount { get; set; }
        public decimal AvgInvoice { get; set; }
        public decimal AvgPieces { get; set; }
    }
}
