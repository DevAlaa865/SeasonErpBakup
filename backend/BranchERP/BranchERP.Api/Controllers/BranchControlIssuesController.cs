using BranchERP.Application.DTOs.BranchControlIssues;
using BranchERP.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class BranchControlIssuesController : ControllerBase
{
    private readonly IBranchControlIssueService _service;

    public BranchControlIssuesController(IBranchControlIssueService service)
    {
        _service = service;
    }

    // ============================
    // 1) Transfer Issues
    // ============================
    [HttpPost("transfer")]
    public async Task<IActionResult> TransferIssues([FromBody] List<CreateBranchControlIssueDto> issues)
    {
        var userName = User.Identity?.Name ?? "Unknown";

        await _service.AddIssuesAsync(issues, userName);

        return Ok(new { message = "تم تحويل العجز إلى الرقابة بنجاح" });
    }

    // ============================
    // 2) Get All Issues (POST + Filter)
    // ============================
    [HttpPost("filter")]
    public async Task<IActionResult> GetAll([FromBody] BranchControlIssueFilterDto filter)
    {
        var result = await _service.GetAllIssuesAsync(filter);
        return Ok(result);
    }

    // ============================
    // 3) Get Issue By Id
    // ============================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetIssueByIdAsync(id);

        if (result == null)
            return NotFound(new { message = "Issue not found" });

        return Ok(result);
    }

    // ============================
    // 4) Update Issue
    // ============================
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateBranchControlIssueDto dto)
    {
        var updated = await _service.UpdateIssueAsync(dto);

        if (!updated)
            return NotFound(new { message = "Issue not found" });

        return Ok(new { message = "تم تحديث حالة الرقابة بنجاح" });
    }


    [HttpGet("manager-report")]
    public async Task<IActionResult> GetManagerReport([FromQuery] ManagerBranchControlIssueFilterDto filter)
    {
        var result = await _service.GetManagerReportAsync(filter);
        return Ok(result);
    }

    [HttpPost("manager-approve")]
    public async Task<IActionResult> ManagerApprove([FromBody] ManagerApproveDto dto)
    {
        var updated = await _service.ManagerApproveAsync(dto);

        if (!updated)
            return NotFound(new { message = "Issue not found" });

        return Ok(new { message = "تم حفظ اعتماد المدير بنجاح" });
    }


    [HttpPost("accountant-report")]
    public async Task<IActionResult> GetAccountantReport([FromBody] AccountantBranchControlIssueFilterDto filter)
    {
        var result = await _service.GetAccountantReportAsync(filter);
        return Ok(result);
    }

    [HttpGet("accountant-report/{id}")]
    public async Task<IActionResult> GetAccountantDetails(int id)
    {
        var result = await _service.GetAccountantDetailsAsync(id);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

}
