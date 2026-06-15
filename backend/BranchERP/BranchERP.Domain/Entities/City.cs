namespace BranchERP.Domain.Entities
{
    public class City : BaseEntity
    {
        public string CityName { get; set; } = string.Empty;

        // الربط مع Region (المدينة تتبع منطقة)
        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}
