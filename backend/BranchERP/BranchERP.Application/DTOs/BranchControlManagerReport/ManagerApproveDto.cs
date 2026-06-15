public class ManagerApproveDto
{
    public int Id { get; set; }
    public bool IsManagerApproved { get; set; }
    public string? ManagerSignature { get; set; }
    public string? ManagerNotes { get; set; }
}
