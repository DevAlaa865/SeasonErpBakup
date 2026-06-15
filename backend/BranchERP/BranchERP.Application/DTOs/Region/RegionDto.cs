namespace BranchERP.Application.DTOs.Region
{
    public class RegionDto
    {
        public int Id { get; set; }
        public string RegionName { get; set; } = string.Empty;

        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
    }
}
