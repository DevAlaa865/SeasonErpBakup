using BranchERP.Application.DTOs.EmployeePersonalAchievement;
using BranchERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeePersonalAchievementController : ControllerBase
    {
        private readonly IEmployeePersonalAchievementService _service;

        public EmployeePersonalAchievementController(IEmployeePersonalAchievementService service)
        {
            _service = service;
        }

        // GET: api/EmployeePersonalAchievement/by-target/5
        [HttpGet("by-target/{personalTargetId:int}")]
        public async Task<IActionResult> GetByPersonalTarget(int personalTargetId)
        {
            var result = await _service.GetByPersonalTargetIdAsync(personalTargetId);
            return Ok(new { success = true, data = result });
        }

        // POST: api/EmployeePersonalAchievement
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate([FromBody] EmployeePersonalAchievementCreateDto dto)
        {
            var result = await _service.CreateOrUpdateAsync(dto);
            return Ok(new { success = true, data = result });
        }
    }
}
