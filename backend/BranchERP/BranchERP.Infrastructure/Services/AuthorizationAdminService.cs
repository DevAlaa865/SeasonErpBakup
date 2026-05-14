using BranchERP.Application.DTOs.Auth;

using BranchERP.Application.DTOs.Common;
using BranchERP.Application.Interfaces;
using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.Enums;
using BranchERP.Infrastructure.Data;
using BranchERP.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BranchERP.Infrastructure.Services
{
    public class AuthorizationAdminService : IAuthorizationAdminService
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthorizationAdminService(
            AppDbContext context,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<ApiResponse<List<PermissionDto>>> GetAllPermissionsAsync()
        {
            var data = await _context.Permissions
                .Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code
                })
                .ToListAsync();

            return ApiResponse<List<PermissionDto>>.Ok(data);
        }

        public async Task<ApiResponse<List<RoleDto>>> GetAllRolesAsync()
        {
            var data = await _context.Roles
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name !
                })
                .ToListAsync();

            return ApiResponse<List<RoleDto>>.Ok(data);
        }

        public async Task<ApiResponse<RolePermissionsDto>> GetRolePermissionsAsync(string roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
                return ApiResponse<RolePermissionsDto>.Fail("Role not found");

            var permissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .Select(rp => new PermissionDto
                {
                    Id = rp.Permission.Id,
                    Name = rp.Permission.Name,
                    Code = rp.Permission.Code
                })
                .ToListAsync();

            var dto  = new RolePermissionsDto
            {
                RoleId = role.Id,
                RoleName = role.Name !,
                Permissions = permissions
            };

            return ApiResponse<RolePermissionsDto>.Ok(dto);
        }

        public async Task<ApiResponse<bool>> UpdateRolePermissionsAsync(RolePermissionsUpdateDto dto)
        {
            var role = await _context.Roles.FindAsync(dto.RoleId);
            if (role == null)
                return ApiResponse<bool>.Fail("Role not found");

            var oldPermissions = _context.RolePermissions
                .Where(rp => rp.RoleId == dto.RoleId);

            _context.RolePermissions.RemoveRange(oldPermissions);

            foreach (var permId in dto.PermissionIds)
            {
                _context.RolePermissions.Add(new RolePermission
                {
                    RoleId = dto.RoleId,
                    PermissionId = permId
                });
            }

            await _context.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Permissions updated successfully");
        }

        public async Task<ApiResponse<List<string>>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ApiResponse<List<string>>.Fail("User not found");

            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            return ApiResponse<List<string>>.Ok(roles);
        }

        public async Task<ApiResponse<bool>> UpdateUserRolesAsync(UserRolesUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return ApiResponse<bool>.Fail("User not found");

            var currentRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRolesAsync(user, dto.RoleNames);

            return ApiResponse<bool>.Ok(true, "User roles updated successfully");
        }

        // ✅ الجديد: إرجاع كل المستخدمين
        public async Task<ApiResponse<List<UserDto>>> GetAllUsersAsync()
        {
            var users = await _userManager.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName!,
                    DisplayName = u.DisplayName ?? u.UserName!,
                    Email = u.Email!,
                    BranchId = u.BranchId,
                    CityId = u.CityId,
                    UserType = u.UserType,
                    DepartmentId = u.DepartmentId,
                    IsActive = u.IsActive   // 🔥 جديد
                })
                .ToListAsync();

            return ApiResponse<List<UserDto>>.Ok(users);
        }


        public async Task<ApiResponse<bool>> UpdateUserDataAsync(UpdateUserDataDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id !);
            if (user == null)
                return ApiResponse<bool>.Fail("User not found");

            // تحديث البيانات الأساسية
            user.DisplayName = dto.DisplayName !;
            user.Email = dto.Email;
            user.IsActive = dto.IsActive;
            user.UserType = (UserType)dto.UserType;
            user.BranchId = dto.BranchId;

            // تعديل اسم الدخول بالطريقة الصحيحة
            if (!string.IsNullOrWhiteSpace(dto.UserName) && dto.UserName != user.UserName)
            {
                var setUserNameResult = await _userManager.SetUserNameAsync(user, dto.UserName);
                if (!setUserNameResult.Succeeded)
                    return ApiResponse<bool>.Fail("Failed to update username");

                // 🔥 مهم جداً: تحديث NormalizedUserName
                user.NormalizedUserName = _userManager.NormalizeName(dto.UserName);
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
                return ApiResponse<bool>.Fail(errorMessage);
            }

            return ApiResponse<bool>.Ok(true, "User data updated successfully");
        }




    }
}
