using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.Enums;
using Microsoft.AspNetCore.Identity;

namespace BranchERP.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;

        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        // مدير المنطقه // ← ده هيبقى RegionId فعليًا 
        public int? CityId { get; set; }

        // 🔥 نوع المستخدم
        // Branch = مستخدم فرع
        // CityManager = مدير مدينة
        // Central = مستخدم مركزي
        public UserType UserType { get; set; } = UserType.Branch;
        public int? BranchId { get; set; }
        public int? DepartmentId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
