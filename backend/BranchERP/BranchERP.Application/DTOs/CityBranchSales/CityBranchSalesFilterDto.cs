using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.CityBranchSales
{
    public class CityBranchSalesFilterDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public List<int>? CityIds { get; set; }


        public int? ActivityTypeId { get; set; }

        // All / Shop / Kiosk
        public string BranchType { get; set; } = "All";
    }

}
