namespace BranchERP.Application.DTOs.Reports
{
    public class ReturnsDiscountsManagementRowDto
    {
        public DateTime JournalDate { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }

        public int ShortageTypeId { get; set; }
        public string ShortageTypeName { get; set; }

        public decimal Amount { get; set; }
    }

}
