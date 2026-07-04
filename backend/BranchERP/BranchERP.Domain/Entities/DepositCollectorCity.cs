using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{
    public class DepositCollectorCity:BaseEntity
    {
     
        public int DepositCollectorId { get; set; }
        public int CityId { get; set; }

        public DepositCollector DepositCollector { get; set; }
        public City City { get; set; }
    }
}
