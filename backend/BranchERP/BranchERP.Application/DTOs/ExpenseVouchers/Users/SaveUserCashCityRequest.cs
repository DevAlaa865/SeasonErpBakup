using BranchERP.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.Users
{
    public class SaveUserCashCityRequest
    {
        public string UserId { get; set; } = string.Empty;
        public List<int> CityIds { get; set; } = new();
        public CashRoleType RoleType { get; set; }
    }
}
