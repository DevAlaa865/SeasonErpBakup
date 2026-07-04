using BranchERP.Application.DTOs.ExpenseVouchers;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CashPostingController : ControllerBase
    {
        private readonly ICashPostingService _cashPostingService;

        public CashPostingController(ICashPostingService cashPostingService)
        {
            _cashPostingService = cashPostingService;
        }

        /// <summary>
        /// ترحيل نقدية اليوميات لكل المدن المرتبطة باليوزر
        /// </summary>
        [HttpPost("post-daily-cash")]
        public async Task<IActionResult> PostDailyCash([FromBody] PostDailyCashRequestDto model)
        {
            if (model == null)
                return BadRequest("Invalid request");

            var result = await _cashPostingService.PostDailyCashForUserAsync(model.UserId, model.Date);

            return Ok(result);
        }

        // ============================================================
        // 2) الترحيل اليدوي
        // ============================================================
        [HttpPost("manual")]
        public async Task<IActionResult> ManualPost([FromBody] ManualPostingRequestDto dto)
        {
            var result = await _cashPostingService.ManualPostAsync(dto);
            return Ok(result);
        }
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory([FromQuery] DateTime date)
        {
            var result = await _cashPostingService.GetPostingHistoryAsync(date);
            return Ok(result);
        }
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var result = await _cashPostingService.GetPostingDetailsAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
