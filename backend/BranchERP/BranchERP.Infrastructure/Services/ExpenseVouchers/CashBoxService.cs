using AutoMapper;
using BranchERP.Application.DTOs.ExpenseVouchers.CashBox;
using BranchERP.Application.Interfaces;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using BranchERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BranchERP.Infrastructure.Services
{
    public class CashBoxService : ICashBoxService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CashBoxService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ============================================================
        // 1) Create CashBox
        // ============================================================
        public async Task<CashBoxDto> CreateAsync(CreateCashBoxDto dto)
        {
            var repo = _unitOfWork.Repository<CashBox>();

            var entity = _mapper.Map<CashBox>(dto);

            entity.CurrentBalance = entity.OpeningBalance;
            entity.IsActive = true;

            await repo.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CashBoxDto>(entity);
        }

        // ============================================================
        // 2) Update CashBox
        // ============================================================
        public async Task<bool> UpdateAsync(UpdateCashBoxDto dto)
        {
            var repo = _unitOfWork.Repository<CashBox>();

            var entity = await repo.GetByIdAsync(dto.Id);

            if (entity == null)
                return false;

            entity.Name = dto.Name;
            entity.DepositCollectorId = dto.DepositCollectorId;
            entity.PettyHolderId = dto.PettyHolderId;
            entity.IsActive = dto.IsActive;

            repo.Update(entity);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        // ============================================================
        // 3) Get CashBox By Id
        // ============================================================
        public async Task<CashBoxDto?> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<CashBox>();

            var entity = await repo.GetAsync(
                x => x.Id == id,
                include: q => q
                    .Include(x => x.DepositCollector)
                    .Include(x => x.PettyHolder)
            );

            return _mapper.Map<CashBoxDto>(entity);
        }

        // ============================================================
        // 4) Get All CashBoxes
        // ============================================================
        public async Task<List<CashBoxDto>> GetAllAsync(bool? isActive = null)
        {
            var repo = _unitOfWork.Repository<CashBox>();

            var data = await repo.GetAllAsync(
                filter: x => !isActive.HasValue || x.IsActive == isActive.Value,
                include: q => q
                    .Include(x => x.DepositCollector)
                    .Include(x => x.PettyHolder)
            );

            return _mapper.Map<List<CashBoxDto>>(data);
        }

        // ============================================================
        // 5) Get CashBox Balance
        // ============================================================
        public async Task<decimal> GetBalanceAsync(int cashBoxId)
        {
            var repo = _unitOfWork.Repository<CashBox>();

            var entity = await repo.GetByIdAsync(cashBoxId);

            if (entity == null)
                throw new Exception("CashBox not found");

            return entity.CurrentBalance;
        }

        // ============================================================
        // 6) Get CashBox Transactions
        // ============================================================
        public async Task<List<CashBoxTransactionDto>> GetTransactionsAsync(int cashBoxId)
        {
            var repo = _unitOfWork.Repository<CashBoxTransaction>();

            var data = await repo.GetAllAsync(
                filter: x => x.CashBoxId == cashBoxId,
                include: q => q
                    .Include(x => x.ExpenseVoucher)
                    .Include(x => x.Branch)
                    .Include(x => x.PettyHolder)
            );

            return _mapper.Map<List<CashBoxTransactionDto>>(data);
        }

        // ============================================================
        // 7) Activate / Deactivate CashBox
        // ============================================================
        public async Task<bool> SetActiveAsync(int id, bool isActive)
        {
            var repo = _unitOfWork.Repository<CashBox>();

            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
                return false;

            entity.IsActive = isActive;

            repo.Update(entity);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public Task<List<CashBoxTransactionDto>> GetTransactionsAsync(int cashBoxId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetCurrentBalanceAsync(int cashBoxId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ActivateAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeactivateAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddManualTransactionAsync(int cashBoxId, decimal amount, string direction, string type, string? description = null)
        {
            throw new NotImplementedException();
        }
    }
}
