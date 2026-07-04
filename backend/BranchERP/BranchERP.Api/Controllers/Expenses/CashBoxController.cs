using BranchERP.Application.DTOs.ExpenseVouchers.CashBox;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers.Expenses
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class CashBoxController : ControllerBase
    {
        private readonly ICashBoxService _service;

        public CashBoxController(ICashBoxService service)
        {
            _service = service;
        }

        // ============================================================
        // GET ALL
        // ============================================================
        [HttpGet]
        public async Task<ActionResult<List<CashBoxDto>>> GetAll([FromQuery] bool? isActive = null)
        {
            var result = await _service.GetAllAsync(isActive);
            return Ok(result);
        }

        // ============================================================
        // GET BY ID
        // ============================================================
        [HttpGet("{id}")]
        public async Task<ActionResult<CashBoxDto>> GetById(int id)
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
        public async Task<ActionResult<CashBoxDto>> Create([FromBody] CreateCashBoxDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // ============================================================
        // UPDATE
        // ============================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCashBoxDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var success = await _service.UpdateAsync(dto);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // ============================================================
        // GET BALANCE
        // ============================================================
        [HttpGet("{id}/balance")]
        public async Task<ActionResult<decimal>> GetBalance(int id)
        {
            var result = await _service.GetBalanceAsync(id);
            return Ok(result);
        }

        // ============================================================
        // GET TRANSACTIONS
        // ============================================================
        [HttpGet("{id}/transactions")]
        public async Task<ActionResult<List<CashBoxTransactionDto>>> GetTransactions(int id)
        {
            var result = await _service.GetTransactionsAsync(id);
            return Ok(result);
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
