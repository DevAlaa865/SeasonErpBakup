using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.CityBranchSales
{
    public class CitySalesChartDto
    {
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public decimal TotalSales { get; set; }
    }

}
