public class BranchDailySummaryFilterDto
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    public List<int>? CityIds { get; set; }
    public int? ActivityTypeId { get; set; }

    public string BranchType { get; set; } // All | Shop | Kiosk

    public bool OnlyWithShortage { get; set; }
}
