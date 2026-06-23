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

        public int CityId { get; set; }
        public int? RegionId { get; set; }

        public bool IsActive { get; set; }

        // Navigation
        public City City { get; set; }
        public Region? Region { get; set; }

        public ICollection<CashBox> CashBoxes { get; set; }
            = new List<CashBox>();

    }
}
