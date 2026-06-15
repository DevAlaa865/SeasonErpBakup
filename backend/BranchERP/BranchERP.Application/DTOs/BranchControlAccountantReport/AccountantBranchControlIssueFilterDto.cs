public class AccountantBranchControlIssueFilterDto
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    // المعتمد / غير المعتمد / الكل
    public bool? IsManagerApproved { get; set; }
}
