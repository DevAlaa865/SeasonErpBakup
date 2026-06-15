namespace BranchERP.Application.DTOs.City
{
    public class CityDto
    {
        public int Id { get; set; }
        public string CityName { get; set; } = string.Empty;

        public int RegionId { get; set; }
        public string RegionName { get; set; } = string.Empty;
    }
}
