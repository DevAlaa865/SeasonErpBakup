using BranchERP.Application.DTOs.BranchDailyTarget;
using BranchERP.Application.Interfaces;
using BranchERP.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchDailyTargetController : ControllerBase
    {
        private readonly IBranchDailyTargetService _service;

        public BranchDailyTargetController(IBranchDailyTargetService service)
        {
            _service = service;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpGet("by-branch-date")]
        public async Task<IActionResult> GetByBranchAndDate(int branchId, DateTime date)
        {
            var result = await _service.GetByBranchAndDateAsync(branchId, date);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BranchDailyTargetHeaderCreateUpdateDto model)
            => Ok(await _service.CreateAsync(model));

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BranchDailyTargetHeaderCreateUpdateDto model)
            => Ok(await _service.UpdateAsync(id, model));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.DeleteAsync(id));

        [HttpPost("import-excel")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportExcel([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("الملف غير موجود");

            using var stream = file.OpenReadStream();

            var result = await _service.ImportFromExcelAsync(stream);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpGet("today-target/{branchId}")]
        public async Task<IActionResult> GetTodayTarget(int branchId)
        {
            var today = DateTime.Today;

            var result = await _service.GetByBranchAndDateAsync(branchId, today);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("employee-report")]
        public async Task<IActionResult> GetEmployeeReport(int? cityId, int? branchId, DateTime date)
        {
            var result = await _service.GetEmployeeReportAsync(cityId, branchId, date);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("branch-period-report")]
        public async Task<IActionResult> GetBranchPeriodReport(
        int branchId,
        DateTime fromDate,
        DateTime toDate)
        {
            if (branchId <= 0)
                return BadRequest("يجب اختيار فرع");

            var result = await _service
                .GetBranchTargetPeriodReportAsync(branchId, fromDate, toDate);

            return Ok(result);
        }


    }
}
