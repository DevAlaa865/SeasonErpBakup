using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.Users
{
    public class DepositCashSummaryDto
    {
        public decimal TotalBranchCash { get; set; }
        public decimal TotalReturns { get; set; }
        public decimal TotalDeposited { get; set; }
        public decimal RemainingCash { get; set; }
    }

}
