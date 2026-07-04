using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{
    public class CashBox : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public int? DepositCollectorId { get; set; }
        public int? PettyHolderId { get; set; }

        public decimal OpeningBalance { get; set; }
        public decimal CurrentBalance { get; set; }

        public bool IsActive { get; set; }

        public DepositCollector? DepositCollector { get; set; }
        public PettyHolder? PettyHolder { get; set; }

        public ICollection<CashBoxTransaction> Transactions { get; set; } = new List<CashBoxTransaction>();
    }
}
