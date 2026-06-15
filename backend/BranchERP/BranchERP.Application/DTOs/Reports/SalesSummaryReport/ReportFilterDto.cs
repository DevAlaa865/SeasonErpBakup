namespace BranchERP.Application.DTOs.Reports.SalesSummaryReport
{
    public class ReportFilterDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? RegionId { get; set; }
        public int? CityId { get; set; }
        public int? BranchId { get; set; }
    }
}
