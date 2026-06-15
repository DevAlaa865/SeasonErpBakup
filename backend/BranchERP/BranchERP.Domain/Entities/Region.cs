namespace BranchERP.Domain.Entities
{
    public class Region : BaseEntity
    {
        public string RegionName { get; set; } = string.Empty;

        // الربط مع Country (المنطقة تتبع دولة)
        public int CountryId { get; set; }
        public Country Country { get; set; }

        // المدن التابعة للمنطقة
        public ICollection<City> Cities { get; set; } = new List<City>();
    }
}
