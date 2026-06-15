namespace BranchERP.Application.DTOs.Region
{
    public class RegionCreateUpdateDto
    {
        public string RegionName { get; set; } = string.Empty;
        public int CountryId { get; set; }
    }
}
