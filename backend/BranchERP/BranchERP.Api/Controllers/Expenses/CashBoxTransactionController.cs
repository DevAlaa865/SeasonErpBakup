using BranchERP.Application.DTOs.ExpenseVouchers.CashTransaction;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using Microsoft.AspNetCore.Mvc;

namespace BranchERP.Api.Controllers.Expenses
{
    [ApiController]
    [Route("api/[controller]")]
    public class CashBoxTransactionController : ControllerBase
    {
        private readonly ICashBoxTransactionService _transactionService;

        public CashBoxTransactionController(ICashBoxTransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // ---------------------------------------------------------
        // 1) Branch Deposit
        // ---------------------------------------------------------
        [HttpPost("branch-deposit")]
        public async Task<IActionResult> AddBranchDeposit([FromBody] BranchDepositRequest dto)
        {
            var result = await _transactionService.AddBranchDepositAsync(dto);
            return result ? Ok() : BadRequest("Branch deposit failed");
        }

        // ---------------------------------------------------------
        // 2) Petty Cash Assignment
        // ---------------------------------------------------------
        [HttpPost("assign-petty-cash")]
        public async Task<IActionResult> AssignPettyCash([FromBody] PettyCashAssignmentRequest dto)
        {
            var result = await _transactionService.AssignPettyCashAsync(dto);
            return result ? Ok() : BadRequest("Petty cash assignment failed");
        }

        // ---------------------------------------------------------
        // 3) Transfer
        // ---------------------------------------------------------
        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] CashBoxTransferRequest dto)
        {
            var result = await _transactionService.TransferAsync(dto);
            return result ? Ok() : BadRequest("Transfer failed");
        }

        // ---------------------------------------------------------
        // 4) Adjustment
        // ---------------------------------------------------------
        [HttpPost("adjust")]
        public async Task<IActionResult> Adjust([FromBody] CashBoxAdjustmentRequest dto)
        {
            var result = await _transactionService.AdjustAsync(dto);
            return result ? Ok() : BadRequest("Adjustment failed");
        }

        // ---------------------------------------------------------
        // 5) Admin Funding
        // ---------------------------------------------------------
        [HttpPost("admin-funding")]
        public async Task<IActionResult> AdminFunding([FromBody] AdminFundingRequest dto)
        {
            var result = await _transactionService.AddAdminFundingAsync(dto);
            return result ? Ok() : BadRequest("Admin funding failed");
        }

        // ---------------------------------------------------------
        // 6) Admin Deduction
        // ---------------------------------------------------------
        [HttpPost("admin-deduction")]
        public async Task<IActionResult> AdminDeduction([FromBody] AdminDeductionRequest dto)
        {
            var result = await _transactionService.AddAdminDeductionAsync(dto);
            return result ? Ok() : BadRequest("Admin deduction failed");
        }

        // ---------------------------------------------------------
        // 7) Get Transactions
        // ---------------------------------------------------------
        [HttpPost("filter")]
        public async Task<IActionResult> GetTransactions([FromBody] CashBoxTransactionFilter filter)
        {
            var result = await _transactionService.GetTransactionsAsync(filter);
            return Ok(result);
        }
    }
}
