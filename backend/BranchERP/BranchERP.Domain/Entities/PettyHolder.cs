using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{

    public class PettyHolder : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        // ❌ هنشيل CityId و City (مش هيبقوا موجودين)
        // public int CityId { get; set; }
        // public City City { get; set; }

        public int? RegionId { get; set; }
        public bool IsActive { get; set; }

        public Region? Region { get; set; }

        // 🔥 الجديد: ربطه باليوزر (لو موجود)
        public string? UserId { get; set; }

        public List<CashBox> CashBoxes { get; set; }
            = new List<CashBox>();

        // 🔥 علاقة Many-to-Many مع المدن
        public ICollection<PettyHolderCity> PettyHolderCities { get; set; }
            = new List<PettyHolderCity>();
    }
}
