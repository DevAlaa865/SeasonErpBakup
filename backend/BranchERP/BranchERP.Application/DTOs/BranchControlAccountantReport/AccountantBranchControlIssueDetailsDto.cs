using BranchERP.Domain.Entities.Enums;

public class AccountantBranchControlIssueDetailsDto
{
    public int Id { get; set; }

    public int BranchNumber { get; set; }
    public string BranchName { get; set; }
    public DateTime SalesDate { get; set; }
    public decimal DifferenceAmount { get; set; }

    // الرقابة
    public string ControlNotes { get; set; }
    public ResolutionType? ResolutionType { get; set; }

    // المدير
    public bool IsManagerApproved { get; set; }
    public string? ManagerNotes { get; set; }
    public string? ManagerSignature { get; set; }
}
