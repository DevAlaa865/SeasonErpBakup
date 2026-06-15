using BranchERP.Application.DTOs.Reports.SalesSummaryReport;
using BranchERP.Application.Interfaces.Reports;
using BranchERP.Application.Interfaces.Reports.SalesSummaryReports;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers.Reports
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesSummaryReportController : ControllerBase
    {
        private readonly ISalesSummaryReportService _service;

        public SalesSummaryReportController(ISalesSummaryReportService service)
        {
            _service = service;
        }

        [HttpPost("Get")]
        public async Task<IActionResult> Get([FromBody] ReportFilterDto filter)
        {
            var result = await _service.GetReportAsync(filter);
            return Ok(new { success = true, data = result });
        }
    }
}
