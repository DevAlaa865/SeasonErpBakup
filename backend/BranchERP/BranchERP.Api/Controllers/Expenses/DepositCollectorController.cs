using BranchERP.Application.DTOs.ExpenseVouchers.Users;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers.Expenses
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepositCollectorController : ControllerBase
    {
        private readonly IDepositCollectorService _service;

        public DepositCollectorController(IDepositCollectorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<DepositCollectorDto>>> GetAll([FromQuery] bool? isActive = null)
        {
            var result = await _service.GetAllAsync(isActive);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepositCollectorDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<DepositCollectorDto>> Create([FromBody] CreateDepositCollectorDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDepositCollectorDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var success = await _service.UpdateAsync(dto);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var success = await _service.SetActiveAsync(id, true);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var success = await _service.SetActiveAsync(id, false);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
