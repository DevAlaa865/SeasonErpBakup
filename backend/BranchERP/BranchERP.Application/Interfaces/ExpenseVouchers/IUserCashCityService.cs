using BranchERP.Application.DTOs.City;
using BranchERP.Application.DTOs.ExpenseVouchers.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces.ExpenseVouchers
{
    public interface IUserCashCityService
    {
        Task<List<UserCashCityDto>> GetUserCashCitiesAsync(string userId);
        Task<bool> SaveUserCashCitiesAsync(SaveUserCashCityRequest request);
        Task<List<CityDto>> GetCitiesAsync();
        Task<List<AppUserMinDto>> GetCentralUsersAsync();
    }
}
