using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.CashBox
{
    public class CashBoxDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal OpeningBalance { get; set; }
        public decimal CurrentBalance { get; set; }

        public bool IsActive { get; set; }

        public int? DepositCollectorId { get; set; }
        public string? DepositCollectorName { get; set; }

        public int? PettyHolderId { get; set; }
        public string? PettyHolderName { get; set; }

        public List<CashBoxTransactionDto> Transactions { get; set; }
            = new List<CashBoxTransactionDto>();
    }
}
