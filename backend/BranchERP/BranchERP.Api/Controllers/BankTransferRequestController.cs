using System.IdentityModel.Tokens.Jwt;
using BranchERP.Application.DTOs.BankTransferRequests;
using BranchERP.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class BankTransferRequestController : ControllerBase
    {
        private readonly IBankTransferRequestService _service;
        private readonly IWebHostEnvironment _env;
        public BankTransferRequestController(IWebHostEnvironment env,
            IBankTransferRequestService service)
        {
            _service = service;
            _env = env;
        }

        private string CurrentUserName =>
            User.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value
            ?? User.Identity?.Name
            ?? "Unknown";

        /// <summary>
        /// إنشاء طلب تحويل جديد
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateBankTransferRequestDto dto)
        {
            var result = await _service.CreateAsync(
                dto,
                CurrentUserName);

            return Ok(result);
        }

        /// <summary>
        /// عرض طلب برقم الـ ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// الطلبات المعلقة فقط
        /// </summary>
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var result = await _service.GetPendingAsync();
            return Ok(result);
        }

        /// <summary>
        /// البحث فى الطلبات
        /// </summary>
        [HttpPost("search")]
        public async Task<IActionResult> Search(
            [FromBody] BankTransferRequestFilterDto filter)
        {
            var result = await _service.SearchAsync(filter);
            return Ok(result);
        }

        /// <summary>
        /// تغيير حالة الطلب (تم التحويل / ملغى)
        /// </summary>
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateStatus(
            [FromBody] UpdateTransferStatusDto dto)
        {
            var result = await _service.UpdateStatusAsync(
                dto,
                CurrentUserName);

            return Ok(result);
        }

        [HttpPost("upload-attachment/{requestId}")]
        public async Task<IActionResult> UploadAttachment(int requestId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("لم يتم اختيار ملف.");

            // فولدر خاص بكل طلب
            var folderPath = Path.Combine(
                _env.WebRootPath,
                "uploads",
                "bank-transfer-requests",
                requestId.ToString()
            );

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = $"uploads/bank-transfer-requests/{requestId}/{fileName}";

            return Ok(new { path = relativePath });
        }


        [HttpPut("update-attachment")]
        public async Task<IActionResult> UpdateAttachment([FromBody] UpdateAttachmentDto dto)
        {
            await _service.UpdateAttachmentAsync(dto);
            return Ok();
        }


    }
}