using BranchERP.Application.DTOs.ExpenseVouchers.Lookups;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers.Expenses
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseTypeController : ControllerBase
    {
        private readonly IExpenseTypeService _expenseTypeService;

        public ExpenseTypeController(IExpenseTypeService expenseTypeService)
        {
            _expenseTypeService = expenseTypeService;
        }

        // GET: api/ExpenseType
        [HttpGet]
        public async Task<ActionResult<List<ExpenseTypeDto>>> GetAll([FromQuery] bool? isActive = null)
        {
            var result = await _expenseTypeService.GetAllAsync(isActive);
            return Ok(result);
        }

        // GET: api/ExpenseType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseTypeDto>> GetById(int id)
        {
            var result = await _expenseTypeService.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST: api/ExpenseType
        [HttpPost]
        public async Task<ActionResult<ExpenseTypeDto>> Create([FromBody] ExpenseTypeDto dto)
        {
            var result = await _expenseTypeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: api/ExpenseType/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExpenseTypeDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var success = await _expenseTypeService.UpdateAsync(dto);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // PATCH: api/ExpenseType/5/activate
        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var success = await _expenseTypeService.SetActiveAsync(id, true);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // PATCH: api/ExpenseType/5/deactivate
        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var success = await _expenseTypeService.SetActiveAsync(id, false);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
