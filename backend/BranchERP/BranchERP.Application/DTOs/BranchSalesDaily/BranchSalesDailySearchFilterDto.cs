using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BranchSalesDaily
{
    public class BranchSalesDailySearchFilterDto
    {
        public int? BranchId { get; set; }
        public int? BranchNumber { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
