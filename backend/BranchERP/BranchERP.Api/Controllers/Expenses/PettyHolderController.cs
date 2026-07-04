using BranchERP.Application.DTOs.ExpenseVouchers.Users;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers.Expenses
{
    [ApiController]
    [Route("api/[controller]")]
    public class PettyHolderController : ControllerBase
    {
        private readonly IPettyHolderService _service;

        public PettyHolderController(IPettyHolderService service)
        {
            _service = service;
        }

        // ============================================================
        // GET ALL
        // ============================================================
        [HttpGet]
        public async Task<ActionResult<List<PettyHolderDto>>> GetAll([FromQuery] bool? isActive = null)
        {
            var result = await _service.GetAllAsync(isActive);
            return Ok(result);
        }

        // ============================================================
        // GET BY ID
        // ============================================================
        [HttpGet("{id}")]
        public async Task<ActionResult<PettyHolderDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // ============================================================
        // CREATE
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<PettyHolderDto>> Create([FromBody] CreatePettyHolderDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // ============================================================
        // UPDATE
        // ============================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePettyHolderDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var success = await _service.UpdateAsync(dto);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // ============================================================
        // ACTIVATE
        // ============================================================
        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var success = await _service.SetActiveAsync(id, true);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // ============================================================
        // DEACTIVATE
        // ============================================================
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
