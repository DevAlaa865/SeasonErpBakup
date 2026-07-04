using BranchERP.Application.DTOs.ExpenseVouchers.Users;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers.Expenses
{
    [ApiController]
    [Route("api/deposit-cash")]
    public class DepositCashController : ControllerBase
    {
        private readonly IDepositCashService _service;

        public DepositCashController(IDepositCashService service)
        {
            _service = service;
        }

        [HttpGet("my")]
        public async Task<ActionResult<DepositCashSummaryDto>> GetMyCash()
        {
            var userId = User.FindFirst("sub")?.Value;

            var result = await _service.GetMyCashAsync(userId!);

            return Ok(result);
        }
    }

}
