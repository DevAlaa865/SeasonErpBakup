using AutoMapper;
using BranchERP.Application.DTOs.City;
using BranchERP.Application.DTOs.ExpenseVouchers.Users;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.Enums;
using BranchERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Infrastructure.Services.ExpenseVouchers
{
    public class UserCashCityService : IUserCashCityService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserCashCityService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ============================================================
        // Get Cities
        // ============================================================
        public async Task<List<CityDto>> GetCitiesAsync()
        {
            var cities = await _context.Cities.ToListAsync();
            return _mapper.Map<List<CityDto>>(cities);
        }

        // ============================================================
        // Get Central Users
        // ============================================================
        public async Task<List<AppUserMinDto>> GetCentralUsersAsync()
        {
            var users = await _context.Users
                .Where(x => x.UserType == UserType.Central) // مستخدم مركزي
                .ToListAsync();

            return users.Select(u => new AppUserMinDto
            {
                Id = u.Id,
                UserName = u.UserName,
                DisplayName = u.DisplayName
            }).ToList();
        }

        // ============================================================
        // Get User Cash Cities
        // ============================================================
        public async Task<List<UserCashCityDto>> GetUserCashCitiesAsync(string userId)
        {
            var data = await _context.UserCashCities
                .Where(x => x.UserId == userId)
                .Include(x => x.City)
                .ToListAsync();

            return _mapper.Map<List<UserCashCityDto>>(data);
        }

        // ============================================================
        // Save User Cash Cities
        // ============================================================
        public async Task<bool> SaveUserCashCitiesAsync(SaveUserCashCityRequest request)
        {
            // حذف القديم
            var old = _context.UserCashCities.Where(x => x.UserId == request.UserId);
            _context.UserCashCities.RemoveRange(old);

            // إضافة الجديد
            foreach (var cityId in request.CityIds)
            {
                _context.UserCashCities.Add(new UserCashCity
                {
                    UserId = request.UserId,
                    CityId = cityId,
                    RoleType = request.RoleType
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }

}
