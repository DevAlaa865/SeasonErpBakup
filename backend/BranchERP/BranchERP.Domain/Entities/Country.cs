namespace BranchERP.Domain.Entities
{
    public class Country : BaseEntity
    {
        public string CountryName { get; set; } = string.Empty;

        // المناطق التابعة للبلد
        public ICollection<Region> Regions { get; set; } = new List<Region>();
    }
}
