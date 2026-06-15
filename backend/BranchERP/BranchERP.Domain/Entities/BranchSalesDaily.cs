using System.ComponentModel.DataAnnotations.Schema;

namespace BranchERP.Domain.Entities
{
    public class BranchSalesDaily : BaseEntity
    {
        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public int? SupervisorId { get; set; }
        public Employee? Supervisor { get; set; }

        public DateTime SalesDate { get; set; }
        public decimal? GrandTotal { get; set; }
        public bool NoSalesToday { get; set; } = false;
        public string? AttachmentPath { get; set; }

        public decimal? TotalSales { get; set; } = 0;
        public decimal? CashAmount { get; set; } = 0;
        public decimal? NetworkAmount { get; set; } = 0;
        public decimal? CreditAmount { get; set; } = 0;

        public decimal? Difference { get; set; } = 0;

        public bool? IsBalanced { get; set; }
        public bool? HasShortage { get; set; }

        public string? SupervisorNotes { get; set; }
        public string? AccountingNotes { get; set; }
        public string? AuditNotes { get; set; }
        public string? FinanceNotes { get; set; }
      
        [Column("SalesDeptNotes")]
        public string? DataEntryUserName { get; set; }
        public string? ReturnsDeptNotes { get; set; }
        public string? DiscountsDeptNotes { get; set; }

        // الحقول الجديدة
        public int? TotalInvoicesCount { get; set; } = 0;
        public int? TotalQuantities { get; set; } = 0;

        public ICollection<BranchSalesShortageDetail> ShortageDetails { get; set; }
            = new List<BranchSalesShortageDetail>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
