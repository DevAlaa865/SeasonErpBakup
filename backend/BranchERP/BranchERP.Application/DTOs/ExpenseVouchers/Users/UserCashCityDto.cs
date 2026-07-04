using BranchERP.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.Users
{
    public class UserCashCityDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public CashRoleType RoleType { get; set; }
        public string RoleTypeName => RoleType.ToString();
    }
}
