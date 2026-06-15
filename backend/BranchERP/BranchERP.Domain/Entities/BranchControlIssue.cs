using System;
using BranchERP.Domain.Entities.Enums;

namespace BranchERP.Domain.Entities
{
    public class BranchControlIssue : BaseEntity
    {
        public int BranchId { get; set; }
        public int SalesDailyId { get; set; }

        public DateTime SalesDate { get; set; }
        public decimal DifferenceAmount { get; set; }

        public string SentByUser { get; set; } = null!;
        public DateTime SentAt { get; set; }

        public BranchControlIssueStatus Status { get; set; }

        public string? ControlNotes { get; set; }
        public ResolutionType? ResolutionType { get; set; }
        public DateTime? ResolvedAt { get; set; }

        // ⭐ جديد: نوع المبلغ (عجز / زيادة)
        public DifferenceDirection DifferenceDirection { get; set; }

        // ⭐ جديد: اعتماد المدير
        public bool IsManagerApproved { get; set; } = false;

        // ⭐ جديد: توقيع المدير (Base64 أو مسار)
        public string? ManagerSignature { get; set; }

        // ⭐ جديد: ملاحظات المدير
        public string? ManagerNotes { get; set; }

        public DateTime? ManagerApprovedAt { get; set; }
        // Navigation
        public Branch Branch { get; set; } = null!;
        public BranchSalesDaily SalesDaily { get; set; } = null!;
    }
}
