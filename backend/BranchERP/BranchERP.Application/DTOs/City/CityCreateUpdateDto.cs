namespace BranchERP.Application.DTOs.City
{
    public class CityCreateUpdateDto
    {
        public string CityName { get; set; } = string.Empty;
        public int RegionId { get; set; }
    }
}
