using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{
    public class PettyHolderCity : BaseEntity
    {
        public int PettyHolderId { get; set; }
        public int CityId { get; set; }

        public PettyHolder PettyHolder { get; set; }
        public City City { get; set; }
    }
}
