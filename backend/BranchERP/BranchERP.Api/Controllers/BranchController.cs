using BranchERP.Application.DTOs.Branch;
using BranchERP.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _branchService.GetAllAsync());

        [HttpGet("paged")]
         public async Task<IActionResult> GetPaged(
            int pageIndex = 1,
            int pageSize = 10,
            string? search = null)
            => Ok(await _branchService.GetPagedAsync(pageIndex, pageSize, search));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _branchService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BranchCreateUpdateDto model)
            => Ok(await _branchService.CreateAsync(model));

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BranchCreateUpdateDto model)
            => Ok(await _branchService.UpdateAsync(id, model));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _branchService.DeleteAsync(id));

        [HttpGet("by-city/{cityId}")]
        public async Task<IActionResult> GetByCity(int cityId)
        {
            var result = await _branchService.GetByCityIdAsync(cityId);
            return Ok(result);
        }
        [HttpPost("by-cities")]
        public async Task<IActionResult> GetByCities([FromBody] BranchByCitiesDto model)
        {
            var result = await _branchService.GetByCityIdsAsync(model.CityIds);

            return Ok(result);
        }
    }
}
