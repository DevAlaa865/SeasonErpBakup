using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.Users
{
    public class CreateDepositCollectorDto
    {
        public string UserId { get; set; }

        public int CityId { get; set; }
        public int? RegionId { get; set; }
    }
}
