using BranchERP.Application.DTOs.Reports;

public class BranchDailySummaryRowDto
{
    public int BranchId { get; set; }
    public string BranchName { get; set; }
    public int BranchNumber { get; set; }

    public decimal CashAmount { get; set; }
    public decimal NetworkAmount { get; set; }
    public decimal CreditAmount { get; set; }

    public decimal TotalSales { get; set; }
    public decimal GrandTotal { get; set; }

    public decimal Difference { get; set; }

    // إجمالي العجز الكلي للفرع
    public decimal TotalShortageAmount { get; set; }

    // 🔹 قائمة ديناميك بأنواع العجز وقيمتها
    public List<BranchShortageSummaryDto> Shortages { get; set; } = new();
}