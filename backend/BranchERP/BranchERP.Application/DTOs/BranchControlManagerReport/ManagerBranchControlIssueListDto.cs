using BranchERP.Domain.Entities.Enums;

public class ManagerBranchControlIssueListDto
{
    public int Id { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; }
    public int BranchNumber { get; set; }
    public DateTime SalesDate { get; set; }
    public decimal DifferenceAmount { get; set; }

    public string? ControlNotes { get; set; } // ملاحظات الرقابة
    public ResolutionType? ResolutionType { get; set; } // قرار الرقابة

    public bool IsManagerApproved { get; set; } // حالة الاعتماد
    public string? ManagerSignature { get; set; } // توقيع المدير
    public string? ManagerNotes { get; set; } // ملاحظات المدير
}
