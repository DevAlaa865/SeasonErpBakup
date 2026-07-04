using BranchERP.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{
    public class UserCashCity : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public int CityId { get; set; }
        public CashRoleType RoleType { get; set; }
        // 🔥 لازم عشان Include يشتغل
        public City City { get; set; }
    }
}
