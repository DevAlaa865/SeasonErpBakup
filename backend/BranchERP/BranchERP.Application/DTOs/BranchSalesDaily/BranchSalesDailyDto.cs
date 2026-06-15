// BranchSalesDailyDto
// ============================
namespace BranchERP.Application.DTOs.BranchSalesDaily
{
    public class BranchSalesDailyDto
    {
        public int Id { get; set; }

        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;

        public int? SupervisorId { get; set; }
        public string? SupervisorName { get; set; }

        public DateTime SalesDate { get; set; }

        public bool NoSalesToday { get; set; }

        // 🔥 الهيدر: مسار صورة اليومية (اختياري)
        public string? AttachmentPath { get; set; }

        public decimal? TotalSales { get; set; }
        public decimal? GrandTotal { get; set; }
        public decimal? CashAmount { get; set; }
        public decimal? NetworkAmount { get; set; }
        public decimal? CreditAmount { get; set; }
        public decimal? Difference { get; set; }

        public bool? IsBalanced { get; set; }
        public bool? HasShortage { get; set; }

        public string? SupervisorNotes { get; set; }
        public string? AccountingNotes { get; set; }
        public string? AuditNotes { get; set; }
        public string? FinanceNotes { get; set; }
        public string? DataEntryUserName { get; set; }
        public string? ReturnsDeptNotes { get; set; }
        public string? DiscountsDeptNotes { get; set; }

        // الحقول الجديدة
        public int? TotalInvoicesCount { get; set; }
        public int? TotalQuantities { get; set; }

        public IReadOnlyList<BranchSalesShortageDetailDto> ShortageDetails { get; set; }
            = new List<BranchSalesShortageDetailDto>();
    }
}