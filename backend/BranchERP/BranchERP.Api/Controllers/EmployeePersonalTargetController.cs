using BranchERP.Application.DTOs.EmployeePersonalTarget;
using BranchERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeePersonalTargetController : ControllerBase
    {
        private readonly IEmployeePersonalTargetService _service;

        public EmployeePersonalTargetController(IEmployeePersonalTargetService service)
        {
            _service = service;
        }

        // GET: api/EmployeePersonalTarget/by-shift/5
        [HttpGet("by-shift/{shiftHeaderId:int}")]
        public async Task<IActionResult> GetByShift(int shiftHeaderId)
        {
            var result = await _service.GetByShiftHeaderAsync(shiftHeaderId);
            return Ok(new { success = true, data = result });
        }

        // POST: api/EmployeePersonalTarget
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeePersonalTargetCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(new { success = true, data = result });
        }

        // PUT: api/EmployeePersonalTarget/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeePersonalTargetCreateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return Ok(new { success = true, data = result });
        }

        // DELETE: api/EmployeePersonalTarget/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);

            if (!success)
                return NotFound(new { success = false, message = "Target not found" });

            return Ok(new { success = true });
        }
    }
}
