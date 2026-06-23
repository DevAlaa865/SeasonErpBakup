using BranchERP.Application.Interfaces.Reports.SalesSummaryReports;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchDailyDetailsController : ControllerBase
    {
        private readonly IBranchDailyDetailsService _service;

        public BranchDailyDetailsController(IBranchDailyDetailsService service)
        {
            _service = service;
        }

        [HttpGet("GetBranchDailyDetails")]
        public async Task<IActionResult> GetBranchDailyDetails(
            int branchId,
            DateTime fromDate,
            DateTime toDate)
        {
            var result = await _service.GetBranchDailyDetailsAsync(branchId, fromDate, toDate);

            return Ok(result);
        }
    }
}
