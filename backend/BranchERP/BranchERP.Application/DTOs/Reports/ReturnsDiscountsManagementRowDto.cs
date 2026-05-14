namespace BranchERP.Application.DTOs.Reports
{
    public class ReturnsDiscountsManagementRowDto
    {
        public DateTime JournalDate { get; set; }

        public int BranchId { get; set; }
        public string? BranchName { get; set; }

        public decimal ReturnsAmount { get; set; }
        public decimal DiscountsAmount { get; set; }
    }
}
