using BranchERP.Application.DTOs.BranchDailyPerformance;
using BranchERP.Application.DTOs.Reports;
using BranchERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchDailyPerformanceController : ControllerBase
    {
        private readonly IBranchDailyPerformanceService _service;

        public BranchDailyPerformanceController(IBranchDailyPerformanceService service)
        {
            _service = service;
        }

        // GET: api/BranchDailyPerformance?branchId=1&date=2025-01-01
        [HttpGet]
        public async Task<IActionResult> Get(int branchId, DateTime date)
        {
            var result = await _service.GetAsync(branchId, date);
            return Ok(new { success = true, data = result });
        }

        // POST: api/BranchDailyPerformance/target
        [HttpPost("target")]
        public async Task<IActionResult> CreateOrUpdateTarget([FromBody] BranchDailyPerformanceCreateUpdateDto dto)
        {
            var result = await _service.CreateOrUpdateTargetAsync(dto);
            return Ok(new { success = true, data = result });
        }

        // POST: api/BranchDailyPerformance/achievement
        [HttpPost("achievement")]
        public async Task<IActionResult> CreateOrUpdateAchievement([FromBody] BranchDailyPerformanceCreateUpdateDto dto)
        {
            var result = await _service.CreateOrUpdateAchievementAsync(dto);
            return Ok(new { success = true, data = result });
        }

        [HttpPost("upload-excel")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadExcel([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { success = false, message = "من فضلك اختر ملف إكسل" });

            using var stream = file.OpenReadStream();
            await _service.ImportDailyTargetsFromExcelAsync(stream);

            return Ok(new { success = true, message = "تم رفع الملف وتحديث تارجت الفروع بنجاح" });
        }

        // POST: api/BranchDailyPerformance/report
        [HttpPost("report")]
        public async Task<IActionResult> GetBranchDailyPerformanceReport([FromBody] BranchDailyPerformanceReportFilterDto filter)
        {
            var result = await _service.GetBranchDailyPerformanceReportAsync(filter);
            return Ok(new { success = true, data = result });
        }




        [HttpPost("export-excel")]
        public async Task<IActionResult> ExportExcel([FromBody] BranchDailyPerformanceReportFilterDto filter)
        {
            var fileBytes = await _service.ExportBranchDailyPerformanceReportToExcelAsync(filter);

            var fileName = $"BranchDailyReport_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );

        }

        [HttpPost("chart-data")]
        public async Task<IActionResult> GetChartData([FromBody] BranchDailyPerformanceReportFilterDto filter)
        {
            var data = await _service.GetBranchPerformanceChartAsync(filter);
            return Ok(new { success = true, data });
        }
    }
 
}
