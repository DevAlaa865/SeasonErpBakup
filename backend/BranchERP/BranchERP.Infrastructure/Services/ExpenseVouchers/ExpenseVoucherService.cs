using AutoMapper;
using BranchERP.Application.DTOs.ExpenseVouchers;
using BranchERP.Application.DTOs.ExpenseVouchers.ExpenseVoucher;
using BranchERP.Application.Interfaces;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace BranchERP.Infrastructure.Services
{
    public class ExpenseVoucherService : IExpenseVoucherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExpenseVoucherService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ============================================================
        // 1) Create Voucher
        // ============================================================
        public async Task<ExpenseVoucherDto> CreateAsync(CreateExpenseVoucherRequest dto)
        {
            var voucherRepo = _unitOfWork.Repository<ExpenseVoucher>();
            var lineRepo = _unitOfWork.Repository<ExpenseVoucherLine>();

            var voucher = _mapper.Map<ExpenseVoucher>(dto);

            voucher.Status = VoucherStatus.Draft;
            voucher.VoucherDate = DateTime.Now;

            await voucherRepo.AddAsync(voucher);
            await _unitOfWork.CompleteAsync();

            int lineNumber = 1;

            foreach (var line in dto.Lines)
            {
                var entity = _mapper.Map<ExpenseVoucherLine>(line);
                entity.ExpenseVoucherId = voucher.Id;
                entity.LineNumber = lineNumber++;

                await lineRepo.AddAsync(entity);
            }

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ExpenseVoucherDto>(voucher);
        }

        // ============================================================
        // 2) Submit
        // ============================================================
        public async Task<bool> SubmitAsync(int id)
        {
            var repo = _unitOfWork.Repository<ExpenseVoucher>();

            var voucher = await repo.GetByIdAsync(id);

            if (voucher == null || voucher.Status != VoucherStatus.Draft)
                return false;

            voucher.Status = VoucherStatus.Submitted;

            repo.Update(voucher);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        // ============================================================
        // 3) Approve (Generate CashBoxTransaction)
        // ============================================================
        public async Task<ExpenseVoucherDto> ApproveAsync(ApproveExpenseVoucherRequest dto)
        {
            var voucherRepo = _unitOfWork.Repository<ExpenseVoucher>();
            var cashBoxRepo = _unitOfWork.Repository<CashBox>();
            var transactionRepo = _unitOfWork.Repository<CashBoxTransaction>();

            var voucher = await voucherRepo.GetAsync(
                x => x.Id == dto.VoucherId,
                include: q => q.Include(v => v.Lines)
            );

            if (voucher == null)
                throw new Exception("Voucher not found");

            if (voucher.Status != VoucherStatus.Submitted)
                throw new Exception("Voucher must be submitted first");

            voucher.Status = VoucherStatus.Approved;
            voucher.ApprovedDate = DateTime.Now;
            voucher.ApprovedByUserId = dto.ApprovedByUserId;

            voucherRepo.Update(voucher);

            var cashBox = await cashBoxRepo.GetByIdAsync(voucher.CashBoxId);

            if (cashBox == null)
                throw new Exception("CashBox not found");

            var totalAmount = voucher.Lines.Sum(x => x.Amount);

            var transaction = new CashBoxTransaction
            {
                CashBoxId = voucher.CashBoxId,
                Amount = totalAmount,
                Direction = TransactionDirection.OUT,
                TransactionType = TransactionType.Expense,
                TransactionDate = DateTime.Now,
                ExpenseVoucherId = voucher.Id,
                Description = $"Expense Voucher #{voucher.Id}"
            };

            await transactionRepo.AddAsync(transaction);

            cashBox.CurrentBalance -= totalAmount;
            cashBoxRepo.Update(cashBox);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ExpenseVoucherDto>(voucher);
        }

        // ============================================================
        // 4) Get By Id
        // ============================================================
        public async Task<ExpenseVoucherDto?> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<ExpenseVoucher>();

            var voucher = await repo.GetAsync(
                x => x.Id == id,
                include: q => q.Include(v => v.Lines)
            );

            return _mapper.Map<ExpenseVoucherDto>(voucher);
        }

        // ============================================================
        // 5) Get All
        // ============================================================
        public async Task<List<ExpenseVoucherDto>> GetAllAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int? cashBoxId = null,
            string? status = null)
        {
            var repo = _unitOfWork.Repository<ExpenseVoucher>();

            var data = await repo.GetAllAsync(
                filter: x =>
                    (!fromDate.HasValue || x.VoucherDate >= fromDate.Value) &&
                    (!toDate.HasValue || x.VoucherDate <= toDate.Value) &&
                    (!cashBoxId.HasValue || x.CashBoxId == cashBoxId.Value) &&
                    (string.IsNullOrEmpty(status) || x.Status.ToString() == status),

                include: q => q.Include(x => x.Lines)
            );

            return _mapper.Map<List<ExpenseVoucherDto>>(data);
        }

        // ============================================================
        // 6) Delete (Only Draft)
        // ============================================================
        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<ExpenseVoucher>();

            var voucher = await repo.GetByIdAsync(id);

            if (voucher == null || voucher.Status != VoucherStatus.Draft)
                return false;

            repo.Delete(voucher);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<List<ExpenseVoucherDto>> GetMyVouchersAsync(string userId)
        {
            var petty = await _unitOfWork.Repository<PettyHolder>()
                .GetAllAsync(x => x.UserId == userId);

            if (!petty.Any())
                return new List<ExpenseVoucherDto>();

            var cashBoxIds = petty
                .SelectMany(x => x.CashBoxes)
                .Select(x => x.Id)
                .ToList();

            var vouchers = await _unitOfWork.Repository<ExpenseVoucher>()
                .GetAllAsync(x => cashBoxIds.Contains(x.CashBoxId));

            return _mapper.Map<List<ExpenseVoucherDto>>(vouchers);
        }

    }
}
