using BranchERP.Application.DTOs.BranchSalesDaily;
using BranchERP.Application.DTOs.Reports;
using BranchERP.Application.Interfaces;
using BranchERP.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchSalesDailyController : ControllerBase
    {
        private readonly IBranchSalesDailyService _service;

        public BranchSalesDailyController(IBranchSalesDailyService service)
        {
            _service = service;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpGet("by-branch-date")]
        public async Task<IActionResult> GetByBranchAndDate(int branchId, DateTime date)
            => Ok(await _service.GetByBranchAndDateAsync(branchId, date));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BranchSalesDailyCreateUpdateDto model)
            => Ok(await _service.CreateAsync(model));

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BranchSalesDailyCreateUpdateDto model)
            => Ok(await _service.UpdateAsync(id, model));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.DeleteAsync(id));

        [HttpPost("summary-report")]
        public async Task<IActionResult> GetSummaryReport([FromBody] BranchDailySummaryFilterDto filter)
        {
            var result = await _service.GetSummaryReportAsync(filter);
            return Ok(result);
        }

        [HttpGet("exists")]
        public async Task<IActionResult> Exists(int branchId, DateTime date)
        {
            var exists = await _service.ExistsAsync(branchId, date);
            return Ok(exists);
        }

        [HttpPost("daily-summary-report")]
        public async Task<IActionResult> GetDailySummary([FromBody] BranchDailySummaryFilterDto filter)
        {
            var data = await _service.GetBranchDailySummaryAsync(filter);
            return Ok(new { success = true, data });
        }


        [HttpPost("returns-discounts-management")]
        public async Task<IActionResult> GetReturnsDiscountsManagement([FromBody] ReturnsDiscountsManagementFilterDto filter)
        {
            var data = await _service.GetReturnsDiscountsManagementAsync(filter);
            return Ok(new { success = true, data });
        }

        [HttpPost("update-shortages-approvals")]
        public async Task<IActionResult> UpdateShortagesApprovals(
        [FromBody] List<ShortageApprovalUpdateDto> items)
        {
            var result = await _service.UpdateShortagesApprovalsAsync(items);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("branch-network-shortages")]
        public async Task<IActionResult> GetBranchNetworkShortages([FromBody] BranchNetworkShortageFilterDto filter)
        {
            var data = await _service.GetBranchNetworkShortagesAsync(filter);
            return Ok(new { success = true, data });
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] BranchSalesDailySearchFilterDto filter)
        {
            var data = await _service.SearchAsync(filter);
            return Ok(new { success = true, data });
        }
    }
}
