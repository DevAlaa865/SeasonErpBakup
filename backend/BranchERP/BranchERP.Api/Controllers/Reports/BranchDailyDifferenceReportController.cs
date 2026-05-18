using BranchERP.Application.DTOs.Reports.DailyReports;
using BranchERP.Application.Interfaces.Reports.DailyReports;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers.Reports
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchDailyDifferenceReportController : ControllerBase
    {
        private readonly IBranchDailyDifferenceReportService _reportService;

        public BranchDailyDifferenceReportController(IBranchDailyDifferenceReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("GetReport")]
        public async Task<IActionResult> GetReport([FromBody] BranchDailyDifferenceReportFilterDto filter)
        {
            var data = await _reportService.GetBranchDailyDifferenceReportAsync(filter);
            return Ok(new { success = true, data });
        }
    }

}
