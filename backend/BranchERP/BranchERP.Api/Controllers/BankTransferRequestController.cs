using BranchERP.Application.DTOs.BankTransferRequests;
using BranchERP.Application.Interfaces;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BankTransferRequestController : ControllerBase
{
    private readonly IBankTransferRequestService _service;

    public BankTransferRequestController(
        IBankTransferRequestService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateBankTransferRequestDto dto)
    {
        var result = await _service.CreateAsync(dto);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        return Ok(result);
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search(
        BankTransferRequestFilterDto filter)
    {
        var result = await _service.SearchAsync(filter);

        return Ok(result);
    }

    [HttpPut("update-status")]
    public async Task<IActionResult> UpdateStatus(
        UpdateTransferStatusDto dto)
    {
        var userName = User.Identity?.Name ?? "System";

        var result =
            await _service.UpdateStatusAsync(dto, userName);

        return Ok(result);
    }
}