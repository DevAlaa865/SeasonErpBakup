using BranchERP.Application.DTOs.Auth;
using BranchERP.Application.DTOs.Common;
using BranchERP.Application.Interfaces;
using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.Enums;
using BranchERP.Infrastructure.Data;
using BranchERP.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BranchERP.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly AppDbContext _context;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            TokenService tokenService,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _context = context;
        }

        public async Task<ApiResponse<object>> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return ApiResponse<object>.Fail("Invalid username or password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return ApiResponse<object>.Fail("Invalid username or password");

            var roles = await _userManager.GetRolesAsync(user);
            var token = await _tokenService.GenerateToken(user, roles);

            var response = new
            {
                token,
                userName = user.UserName,
                roles
            };

            return ApiResponse<object>.Ok(response);
        }

        public async Task<ApiResponse<string>> RegisterAsync(RegisterDto model)
        {
            if (model.Password != model.ConfirmPassword)
                return ApiResponse<string>.Fail("Passwords do not match");

            var existingUser = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null)
                return ApiResponse<string>.Fail("Username already exists");

            // 🔥 مستخدم فرع
            if (model.UserType == UserType.Branch && model.BranchId == null)
                return ApiResponse<string>.Fail("BranchId is required for Branch users");

            // 🔥 مدير منطقة (لازم يختار مدن)
            if (model.UserType == UserType.RegionManager &&
                (model.CityIds == null || !model.CityIds.Any()))
            {
                return ApiResponse<string>.Fail("At least one city is required for RegionManager users");
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                DisplayName = model.DisplayName,

                BranchId = model.UserType == UserType.Branch ? model.BranchId : null,

                // ❗ CityId مش هنستخدمه لمدير المنطقة
                CityId = null,

                UserType = model.UserType,
                DepartmentId = model.DepartmentId,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
                return ApiResponse<string>.Fail(errorMessage);
            }

            // 🔥 حفظ المدن في جدول UserCities
            if (model.UserType == UserType.RegionManager)
            {
                foreach (var cityId in model.CityIds!)
                {
                    _context.UserCities.Add(new UserCity
                    {
                        UserId = user.Id,
                        CityId = cityId
                    });
                }

                await _context.SaveChangesAsync();
            }

            if (!string.IsNullOrWhiteSpace(model.RoleName))
                await _userManager.AddToRoleAsync(user, model.RoleName);

            return ApiResponse<string>.Ok("User registered successfully");
        }

        public async Task<ApiResponse<string>> AdminResetPasswordAsync(AdminResetPasswordDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return ApiResponse<string>.Fail("User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
                return ApiResponse<string>.Fail(errorMessage);
            }

            return ApiResponse<string>.Ok("Password reset successfully");
        }
    }
}
