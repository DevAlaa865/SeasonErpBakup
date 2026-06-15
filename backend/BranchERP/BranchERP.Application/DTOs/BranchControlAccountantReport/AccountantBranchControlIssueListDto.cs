public class AccountantBranchControlIssueListDto
{
    public int Id { get; set; }
    public int BranchId { get; set; }
    public int BranchNumber { get; set; }
    public string BranchName { get; set; }
    public DateTime SalesDate { get; set; }
    public decimal DifferenceAmount { get; set; }

    public bool IsManagerApproved { get; set; }
}
