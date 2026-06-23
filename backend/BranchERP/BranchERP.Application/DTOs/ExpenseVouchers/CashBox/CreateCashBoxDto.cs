using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.CashBox
{
    public class CreateCashBoxDto
    {
        public string Name { get; set; } = string.Empty;

        public int? DepositCollectorId { get; set; }
        public int? PettyHolderId { get; set; }

        public decimal OpeningBalance { get; set; }
    }
}
