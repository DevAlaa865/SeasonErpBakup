using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TargetAttachmentsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public TargetAttachmentsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // POST: api/TargetAttachments/upload
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { success = false, message = "No file uploaded" });

            // 1) تحديد المسار
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "targets");

            // 2) إنشاء الفولدر لو مش موجود
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // 3) اسم الملف
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            // 4) المسار الكامل
            var filePath = Path.Combine(uploadsFolder, fileName);

            // 5) حفظ الملف
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 6) المسار اللي يرجع للفرونت
            var relativePath = $"/uploads/targets/{fileName}";

            return Ok(new
            {
                success = true,
                path = relativePath
            });
        }
    }
}
