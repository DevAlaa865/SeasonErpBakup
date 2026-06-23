using BranchERP.Domain.Entities.Enums;

namespace BranchERP.Application.DTOs.Auth
{
    public class RegisterDto
    {
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? RoleName { get; set; }
        public List<int>? CityIds { get; set; }
        // نوع المستخدم
        public UserType UserType { get; set; } = UserType.Branch;
        public int? BranchId { get; set; }
        public int? DepartmentId { get; set; }
    }
}
