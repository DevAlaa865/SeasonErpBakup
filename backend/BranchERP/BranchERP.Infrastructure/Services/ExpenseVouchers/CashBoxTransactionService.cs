using BranchERP.Application.DTOs.ExpenseVouchers.CashBox;
using BranchERP.Application.DTOs.ExpenseVouchers.CashTransaction;
using BranchERP.Application.Interfaces;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.Enums;
using BranchERP.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Infrastructure.Services.ExpenseVouchers
{
    public class CashBoxTransactionService : ICashBoxTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CashBoxTransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ---------------------------------------------------------
        // 1) Branch Deposit (استلام نقدية من فرع)
        // ---------------------------------------------------------
        public async Task<bool> AddBranchDepositAsync(BranchDepositRequest dto)
        {
            var cashBoxRepo = _unitOfWork.Repository<CashBox>();
            var trxRepo = _unitOfWork.Repository<CashBoxTransaction>();

            var cashBox = await cashBoxRepo.GetByIdAsync(dto.CashBoxId);
            if (cashBox == null) return false;

            cashBox.CurrentBalance += dto.Amount;
            cashBoxRepo.Update(cashBox);

            var trx = new CashBoxTransaction
            {
                CashBoxId = dto.CashBoxId,
                Amount = dto.Amount,
                Direction = TransactionDirection.IN,
                TransactionType = TransactionType.BranchDeposit,
                TransactionDate = dto.TransactionDate,
                Description = dto.Description,
                ReferenceNumber = dto.ReferenceNumber,
                BranchId = dto.BranchId
            };

            await trxRepo.AddAsync(trx);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        // ---------------------------------------------------------
        // 2) Petty Cash Assignment (صرف عهدة)
        // ---------------------------------------------------------
        public async Task<bool> AssignPettyCashAsync(PettyCashAssignmentRequest dto)
        {
            var cashBoxRepo = _unitOfWork.Repository<CashBox>();
            var trxRepo = _unitOfWork.Repository<CashBoxTransaction>();

            var fromBox = await cashBoxRepo.GetByIdAsync(dto.FromCashBoxId);
            var toBox = await cashBoxRepo.GetByIdAsync(dto.ToCashBoxId);

            if (fromBox == null || toBox == null) return false;
            if (fromBox.CurrentBalance < dto.Amount) return false;

            fromBox.CurrentBalance -= dto.Amount;
            toBox.CurrentBalance += dto.Amount;

            cashBoxRepo.Update(fromBox);
            cashBoxRepo.Update(toBox);

            // OUT
            var outTrx = new CashBoxTransaction
            {
                CashBoxId = dto.FromCashBoxId,
                Amount = dto.Amount,
                Direction = TransactionDirection.OUT,
                TransactionType = TransactionType.PettyCashAssignment,
                TransactionDate = dto.TransactionDate,
                Description = dto.Description,
                PettyHolderId = dto.PettyHolderId
            };

            // IN
            var inTrx = new CashBoxTransaction
            {
                CashBoxId = dto.ToCashBoxId,
                Amount = dto.Amount,
                Direction = TransactionDirection.IN,
                TransactionType = TransactionType.PettyCashAssignment,
                TransactionDate = dto.TransactionDate,
                Description = dto.Description,
                PettyHolderId = dto.PettyHolderId
            };

            await trxRepo.AddAsync(outTrx);
            await trxRepo.AddAsync(inTrx);

            await _unitOfWork.CompleteAsync();
            return true;
        }

        // ---------------------------------------------------------
        // 3) Transfer (تحويل بين صناديق)
        // ---------------------------------------------------------
        public async Task<bool> TransferAsync(CashBoxTransferRequest dto)
        {
            var cashBoxRepo = _unitOfWork.Repository<CashBox>();
            var trxRepo = _unitOfWork.Repository<CashBoxTransaction>();

            var fromBox = await cashBoxRepo.GetByIdAsync(dto.FromCashBoxId);
            var toBox = await cashBoxRepo.GetByIdAsync(dto.ToCashBoxId);

            if (fromBox == null || toBox == null) return false;
            if (fromBox.CurrentBalance < dto.Amount) return false;

            fromBox.CurrentBalance -= dto.Amount;
            toBox.CurrentBalance += dto.Amount;

            cashBoxRepo.Update(fromBox);
            cashBoxRepo.Update(toBox);

            // OUT
            var outTrx = new CashBoxTransaction
            {
                CashBoxId = dto.FromCashBoxId,
                Amount = dto.Amount,
                Direction = TransactionDirection.OUT,
                TransactionType = TransactionType.Transfer,
                TransactionDate = dto.TransactionDate,
                Description = dto.Description
            };

            // IN
            var inTrx = new CashBoxTransaction
            {
                CashBoxId = dto.ToCashBoxId,
                Amount = dto.Amount,
                Direction = TransactionDirection.IN,
                TransactionType = TransactionType.Transfer,
                TransactionDate = dto.TransactionDate,
                Description = dto.Description
            };

            await trxRepo.AddAsync(outTrx);
            await trxRepo.AddAsync(inTrx);

            await _unitOfWork.CompleteAsync();
            return true;
        }

        // ---------------------------------------------------------
        // 4) Adjustment (تسوية)
        // ---------------------------------------------------------
        public async Task<bool> AdjustAsync(CashBoxAdjustmentRequest dto)
        {
            var cashBoxRepo = _unitOfWork.Repository<CashBox>();
            var trxRepo = _unitOfWork.Repository<CashBoxTransaction>();

            var box = await cashBoxRepo.GetByIdAsync(dto.CashBoxId);
            if (box == null) return false;

            var directionEnum = dto.Direction == "IN"
                ? TransactionDirection.IN
                : TransactionDirection.OUT;

            if (directionEnum == TransactionDirection.IN)
                box.CurrentBalance += dto.Amount;
            else
            {
                if (box.CurrentBalance < dto.Amount) return false;
                box.CurrentBalance -= dto.Amount;
            }

            cashBoxRepo.Update(box);

            var trx = new CashBoxTransaction
            {
                CashBoxId = dto.CashBoxId,
                Amount = dto.Amount,
                Direction = directionEnum,
                TransactionType = TransactionType.Adjustment,
                TransactionDate = dto.TransactionDate,
                Description = dto.Reason
            };

            await trxRepo.AddAsync(trx);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        // ---------------------------------------------------------
        // 5) Admin Funding (تمويل من الإدارة)
        // ---------------------------------------------------------
        public async Task<bool> AddAdminFundingAsync(AdminFundingRequest dto)
        {
            var cashBoxRepo = _unitOfWork.Repository<CashBox>();
            var trxRepo = _unitOfWork.Repository<CashBoxTransaction>();

            var box = await cashBoxRepo.GetByIdAsync(dto.CashBoxId);
            if (box == null) return false;

            box.CurrentBalance += dto.Amount;
            cashBoxRepo.Update(box);

            var trx = new CashBoxTransaction
            {
                CashBoxId = dto.CashBoxId,
                Amount = dto.Amount,
                Direction = TransactionDirection.IN,
                TransactionType = TransactionType.AdminFunding,
                TransactionDate = dto.TransactionDate,
                Description = dto.Description,
                ReferenceNumber = dto.ReferenceNumber
            };

            await trxRepo.AddAsync(trx);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        // ---------------------------------------------------------
        // 6) Admin Deduction (خصم من الإدارة)
        // ---------------------------------------------------------
        public async Task<bool> AddAdminDeductionAsync(AdminDeductionRequest dto)
        {
            var cashBoxRepo = _unitOfWork.Repository<CashBox>();
            var trxRepo = _unitOfWork.Repository<CashBoxTransaction>();

            var box = await cashBoxRepo.GetByIdAsync(dto.CashBoxId);
            if (box == null) return false;
            if (box.CurrentBalance < dto.Amount) return false;

            box.CurrentBalance -= dto.Amount;
            cashBoxRepo.Update(box);

            var trx = new CashBoxTransaction
            {
                CashBoxId = dto.CashBoxId,
                Amount = dto.Amount,
                Direction = TransactionDirection.OUT,
                TransactionType = TransactionType.AdminDeduction,
                TransactionDate = dto.TransactionDate,
                Description = dto.Reason
            };

            await trxRepo.AddAsync(trx);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        // ---------------------------------------------------------
        // 7) Get Transactions (استعلام)
        // ---------------------------------------------------------
        public async Task<List<CashBoxTransactionDto>> GetTransactionsAsync(CashBoxTransactionFilter filter)
        {
            var trxRepo = _unitOfWork.Repository<CashBoxTransaction>();

            var query = trxRepo.Query()
                .Include(x => x.Branch)
                .Include(x => x.PettyHolder)
                .Include(x => x.ExpenseVoucher)
                .Where(x => x.CashBoxId == filter.CashBoxId);

            if (filter.FromDate.HasValue)
                query = query.Where(x => x.TransactionDate >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(x => x.TransactionDate <= filter.ToDate.Value);

            if (!string.IsNullOrEmpty(filter.TransactionType))
                query = query.Where(x => x.TransactionType.ToString() == filter.TransactionType);

            if (filter.BranchId.HasValue)
                query = query.Where(x => x.BranchId == filter.BranchId.Value);

            if (filter.PettyHolderId.HasValue)
                query = query.Where(x => x.PettyHolderId == filter.PettyHolderId.Value);

            var list = await query.OrderByDescending(x => x.TransactionDate).ToListAsync();

            return list.Select(x => new CashBoxTransactionDto
            {
                Id = x.Id,
                CashBoxId = x.CashBoxId,
                Amount = x.Amount,
                Direction = x.Direction.ToString(),
                Type = x.TransactionType.ToString(),
                TransactionDate = x.TransactionDate,
                Description = x.Description,
                ReferenceNumber = x.ReferenceNumber,
                BranchId = x.BranchId,
                BranchName = x.Branch?.BranchName,
                PettyHolderId = x.PettyHolderId,
                PettyHolderName = x.PettyHolder?.Name,
                ExpenseVoucherId = x.ExpenseVoucherId,
                ExpenseVoucherNumber = x.ExpenseVoucher?.VoucherNumber
            }).ToList();
        }
    }
}
