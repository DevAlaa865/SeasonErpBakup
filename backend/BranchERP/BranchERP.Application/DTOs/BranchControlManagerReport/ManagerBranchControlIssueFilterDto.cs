using BranchERP.Domain.Entities.Enums;

public class ManagerBranchControlIssueFilterDto
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    // الحالة (تحت المراجعة أو تم الحل)
    public BranchControlIssueStatus? Status { get; set; }
}
