using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.Users
{
    public class UpdateDepositCollectorDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int CityId { get; set; }
        public int? RegionId { get; set; }

        public bool IsActive { get; set; }
    }
}
