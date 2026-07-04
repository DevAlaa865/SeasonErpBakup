using BranchERP.Application.DTOs.ExpenseVouchers.ExpenseVoucher;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers.Expenses
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseVoucherController : ControllerBase
    {
        private readonly IExpenseVoucherService _service;

        public ExpenseVoucherController(IExpenseVoucherService service)
        {
            _service = service;
        }

        // POST: api/ExpenseVoucher
        // إنشاء سند صرف (Draft أو Submitted حسب dto.Submit)
        [HttpPost]
        public async Task<ActionResult<ExpenseVoucherDto>> Create([FromBody] CreateExpenseVoucherRequest dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // GET: api/ExpenseVoucher/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseVoucherDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // GET: api/ExpenseVoucher
        [HttpGet]
        public async Task<ActionResult<List<ExpenseVoucherDto>>> GetAll(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] int? cashBoxId = null,
            [FromQuery] string? status = null)
        {
            var result = await _service.GetAllAsync(fromDate, toDate, cashBoxId, status);
            return Ok(result);
        }

        // POST: api/ExpenseVoucher/5/submit
        [HttpPost("{id}/submit")]
        public async Task<IActionResult> Submit(int id)
        {
            var success = await _service.SubmitAsync(id);
            if (!success)
                return BadRequest("Voucher cannot be submitted");

            return NoContent();
        }

        // POST: api/ExpenseVoucher/approve
        [HttpPost("approve")]
        public async Task<ActionResult<ExpenseVoucherDto>> Approve([FromBody] ApproveExpenseVoucherRequest dto)
        {
            var result = await _service.ApproveAsync(dto);
            return Ok(result);
        }

        // DELETE: api/ExpenseVoucher/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
                return BadRequest("Only draft vouchers can be deleted");

            return NoContent();
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<ExpenseVoucherDto>>> GetMyVouchers()
        {
            var userId = User.FindFirst("sub")?.Value;

            var result = await _service.GetMyVouchersAsync(userId!);

            return Ok(result);
        }

    }
}
