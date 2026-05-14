using BranchERP.Application.DTOs.CityBranchSales;
using BranchERP.Application.DTOs.Reports;
using BranchERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.API.Controllers.Reports
{
    [ApiController]
    [Route("api/reports/city-branch-sales")]
    public class CityBranchSalesReportController : ControllerBase
    {
        private readonly ICityBranchSalesReportService _service;

        public CityBranchSalesReportController(ICityBranchSalesReportService service)
        {
            _service = service;
        }

        [HttpPost("summary")]
        public async Task<IActionResult> GetSummary([FromBody] CityBranchSalesFilterDto filter)
        {
            if (filter == null)
                return BadRequest(new { success = false, message = "Invalid filter" });

            var data = await _service.GetCityBranchSalesSummaryAsync(filter);

            return Ok(new { success = true, data });
        }

        [HttpPost("chart")]
        public async Task<IActionResult> GetChart([FromBody] CityBranchSalesFilterDto filter)
        {
            if (filter == null)
                return BadRequest(new { success = false, message = "Invalid filter" });

            var data = await _service.GetCitySalesChartAsync(filter);

            return Ok(new { success = true, data });
        }
    }
}
