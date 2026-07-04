using BranchERP.Application.DTOs.ExpenseVouchers.Users;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers.Expenses
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserCashCityController : ControllerBase
    {
        private readonly IUserCashCityService _service;

        public UserCashCityController(IUserCashCityService service)
        {
            _service = service;
        }

        [HttpGet("cities")]
        public async Task<IActionResult> GetCities()
        {
            var result = await _service.GetCitiesAsync();
            return Ok(new { success = true, data = result });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetCentralUsers()
        {
            var result = await _service.GetCentralUsersAsync();
            return Ok(new { success = true, data = result });
        }

        [HttpGet("get/{userId}")]
        public async Task<IActionResult> GetUserCashCities(string userId)
        {
            var result = await _service.GetUserCashCitiesAsync(userId);
            return Ok(new { success = true, data = result });
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveUserCashCities([FromBody] SaveUserCashCityRequest request)
        {
            var result = await _service.SaveUserCashCitiesAsync(request);
            return Ok(new { success = result });
        }
    }

}
