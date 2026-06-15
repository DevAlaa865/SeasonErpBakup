namespace BranchERP.Application.DTOs.Reports.SalesSummaryReport
{
    public class ReportResultDto
    {
        public int BranchId { get; set; }

        public int BranchNumber { get; set; }
        public string BranchName { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalReturns { get; set; }
        public decimal NetSales => TotalSales - TotalReturns;
        public int InvoiceCount { get; set; }
        public int QuantityCount { get; set; }
        public string ActivityType { get; set; }
    }
}
