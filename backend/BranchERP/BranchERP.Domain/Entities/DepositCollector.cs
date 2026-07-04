namespace BranchERP.Domain.Entities
{
    public class DepositCollector : BaseEntity
    {
        public string? UserId { get; set; }

        public int? RegionId { get; set; }

        public bool IsActive { get; set; }

        public Region? Region { get; set; }

        public List<CashBox> CashBoxes { get; set; }
            = new();

        // 🔥 علاقة Many-to-Many مع المدن
        public ICollection<DepositCollectorCity> DepositCollectorCities { get; set; }
            = new List<DepositCollectorCity>();
    }
}
