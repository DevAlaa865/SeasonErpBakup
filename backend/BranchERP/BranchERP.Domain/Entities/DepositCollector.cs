namespace BranchERP.Domain.Entities
{
    public class DepositCollector : BaseEntity
    {
        // يربط مع AspNetUsers
        public string UserId { get; set; }

        // المدينة الأساسية
        public int CityId { get; set; }

        // المنطقة (اختياري)
        public int? RegionId { get; set; }

        // حالة التفعيل
        public bool IsActive { get; set; }

        // Navigation Properties
        public City City { get; set; }
        public Region? Region { get; set; }

        // الصناديق المرتبطة بمسؤول الإيداع
        public ICollection<CashBox> CashBoxes { get; set; }
            = new List<CashBox>();
    }
}
