using BranchERP.Application.DTOs.ExpenseVouchers.CashBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.Users
{
    public class DepositCollectorDto
    {
        public int Id { get; set; }

        // من AspNetUsers
        public string UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // من Entity
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;

        public int? RegionId { get; set; }
        public string? RegionName { get; set; }

        public bool IsActive { get; set; }

        public List<CashBoxDto> CashBoxes { get; set; }
            = new List<CashBoxDto>();
    }
}
