using BranchERP.Application.DTOs;
using BranchERP.Application.DTOs.PermissionDto;
using BranchERP.Application.Interfaces;
using BranchERP.Domain.Entities;
using BranchERP.Infrastructure.Data;
using BranchERP.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BranchERP.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public PermissionService(
            UserManager<ApplicationUser> userManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // ============================
        // 1) صلاحيات اليوزر (زي ما هي)
        // ============================
        public async Task<List<string>> GetUserPermissionsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<string>();

            var roles = await _userManager.GetRolesAsync(user);

            var roleIds = await _context.Roles
                .Where(r => roles.Contains(r.Name!))
                .Select(r => r.Id)
                .ToListAsync();

            var permissions = await _context.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission.Code)
                .ToListAsync();

            return permissions;
        }
        // ============================
        // 2) إدارة جدول Permissions
        // ============================

        public async Task<List<PermissionDto>> GetAllAsync()
        {
            return await _context.Permissions
                .Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code
                })
                .ToListAsync();
        }

        public async Task<PermissionDto> CreateAsync(PermissionCreateDto model)
        {
            var exists = await _context.Permissions
                .AnyAsync(p => p.Code == model.Code);

            if (exists)
                throw new System.Exception("Permission code already exists");

            var perm = new Permission
            {
                Name = model.Name,
                Code = model.Code
            };

            _context.Permissions.Add(perm);
            await _context.SaveChangesAsync();

            return new PermissionDto
            {
                Id = perm.Id,
                Name = perm.Name,
                Code = perm.Code
            };
        }

        public async Task<PermissionDto?> UpdateAsync(int id, PermissionCreateDto model)
        {
            var perm = await _context.Permissions.FindAsync(id);
            if (perm == null) return null;

            perm.Name = model.Name;
            perm.Code = model.Code;

            await _context.SaveChangesAsync();

            return new PermissionDto
            {
                Id = perm.Id,
                Name = perm.Name,
                Code = perm.Code
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var perm = await _context.Permissions.FindAsync(id);
            if (perm == null) return false;

            _context.Permissions.Remove(perm);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
