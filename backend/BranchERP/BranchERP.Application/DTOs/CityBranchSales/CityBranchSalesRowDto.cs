using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.CityBranchSales
{
    public class CityBranchSalesRowDto
    {
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;

        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;

        public decimal TotalSales { get; set; }
        public decimal CreditAmount { get; set; }
        public int InvoicesCount { get; set; }
        public int QuantitiesCount { get; set; }

        public string SupervisorName { get; set; } = string.Empty;
    }

}
