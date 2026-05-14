using Application.Interfaces;
using BranchERP.Application.DTOs.BranchDailyReturnDto;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchDailyReturnsController : ControllerBase
    {
        private readonly IBranchDailyReturnService _returnService;

        public BranchDailyReturnsController(IBranchDailyReturnService returnService)
        {
            _returnService = returnService;
        }

        [HttpPost("upload-excel")]
        public async Task<IActionResult> UploadExcel([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("الملف غير صالح");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            await _returnService.ImportFromExcelAsync(stream, file.FileName);

            return Ok(new { message = "تم استيراد ملف المرتجعات بنجاح" });
        }

        [HttpGet]
        public async Task<IActionResult> GetReturns(
       [FromQuery] DateTime? fromDate,
       [FromQuery] DateTime? toDate,
       [FromQuery] int? branchId,
       [FromQuery] int? branchNumber,
       [FromQuery] int? cityId,
       [FromQuery] int? returnType
   )
        {
            var result = await _returnService.GetReturnsAsync(
                fromDate,
                toDate,
                branchId,
                branchNumber,
                cityId,
                returnType
            );

            return Ok(new
            {
                Success = true,
                Data = result
            });
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _returnService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { Success = false, Message = "لم يتم العثور على المرتجع" });

            return Ok(new { Success = true, Data = result });
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BranchDailyReturnUpdateDto dto)
        {
            // لو عندك UserName من الـ JWT أو الـ Context هاته هنا
            var userName = User?.Identity?.Name ?? "system";

            var success = await _returnService.UpdateAsync(id, dto, userName);

            if (!success)
                return NotFound(new { Success = false, Message = "لم يتم العثور على المرتجع" });

            return Ok(new { Success = true, Message = "تم تعديل المرتجع بنجاح" });
        }


        [HttpGet("export")]
        public async Task<IActionResult> ExportToExcel(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int? branchId,
            [FromQuery] int? branchNumber,
            [FromQuery] int? cityId,
            [FromQuery] int? returnType
)
        {
            var fileBytes = await _returnService.ExportToExcelAsync(
                fromDate,
                toDate,
                branchId,
                branchNumber,
                cityId,
                returnType
            );

            var fileName = $"DailyReturns_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }

    }
}
