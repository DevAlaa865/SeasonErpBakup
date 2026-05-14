using BranchERP.Application.DTOs.CommissionRule;
using BranchERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommissionRuleController : ControllerBase
    {
        private readonly ICommissionRuleService _service;

        public CommissionRuleController(ICommissionRuleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(new { success = true, data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _service.GetByIdAsync(id);
            return Ok(new { success = true, data });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommissionRuleCreateUpdateDto dto)
        {
            var data = await _service.CreateAsync(dto);
            return Ok(new { success = true, data });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CommissionRuleCreateUpdateDto dto)
        {
            var data = await _service.UpdateAsync(id, dto);
            return Ok(new { success = true, data });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return Ok(new { success });
        }
    }
}
