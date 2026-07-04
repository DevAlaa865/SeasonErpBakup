using BranchERP.Application.Interfaces.ExpenseVouchers;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers.Expenses
{
    [ApiController]
    [Route("api/branches")]
    public class UserBranchController : ControllerBase
    {
        private readonly IUserBranchService _service;

        public UserBranchController(IUserBranchService service)
        {
            _service = service;
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<BranchDto>>> GetMyBranches()
        {
            var userId = User.FindFirst("sub")?.Value;

            var result = await _service.GetMyBranchesAsync(userId!);

            return Ok(result);
        }
    }
}
