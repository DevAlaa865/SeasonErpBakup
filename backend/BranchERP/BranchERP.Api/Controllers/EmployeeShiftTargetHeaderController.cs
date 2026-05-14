using BranchERP.Application.DTOs.EmployeeShiftTarget;
using BranchERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeShiftTargetHeaderController : ControllerBase
    {
        private readonly IEmployeeShiftTargetHeaderService _service;

        public EmployeeShiftTargetHeaderController(IEmployeeShiftTargetHeaderService service)
        {
            _service = service;
        }

        // GET: api/EmployeeShiftTargetHeader/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetAsync(id);
            return Ok(new { success = true, data = result });
        }

        // POST: api/EmployeeShiftTargetHeader
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeShiftTargetHeaderCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(new { success = true, data = result });
        }

        // DELETE: api/EmployeeShiftTargetHeader/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);

            if (!success)
                return NotFound(new { success = false, message = "Header not found" });

            return Ok(new { success = true });
        }
    }
}
