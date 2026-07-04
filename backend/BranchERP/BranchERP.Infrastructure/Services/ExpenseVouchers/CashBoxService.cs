using AutoMapper;
using BranchERP.Application.DTOs.ExpenseVouchers.CashBox;
using BranchERP.Application.Interfaces;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using BranchERP.Domain.Entities;
using BranchERP.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BranchERP.Infrastructure.Services
{
    public class CashBoxService : ICashBoxService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public CashBoxService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        private async Task<CashBoxDto> BuildDto(CashBox entity)
        {
            var dto = _mapper.Map<CashBoxDto>(entity);

            // اسم مسؤول الإيداع من AspNetUsers
            if (entity.DepositCollector?.UserId != null)
            {
                var user = await _userManager.FindByIdAsync(entity.DepositCollector.UserId);
                dto.DepositCollectorName = user?.UserName ?? "";
            }

            // اسم أمين العهدة
            dto.PettyHolderName = entity.PettyHolder?.Name ?? "";

            return dto;
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
                    .Include(x => x.Transactions)
            );

            if (entity == null)
                return null;

            return await BuildDto(entity);
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
                    .Include(x => x.Transactions)
            );

            var list = new List<CashBoxDto>();

            foreach (var entity in data)
            {
                list.Add(await BuildDto(entity));
            }

            return list;
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
                    .Include<CashBoxTransaction, object>(x => x.ExpenseVoucher)
                    .Include<CashBoxTransaction, object>(x => x.Branch)
                    .Include<CashBoxTransaction, object>(x => x.PettyHolder)
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

        // ============================================================
        // 8) Get CashBoxes For User
        // ============================================================
        public async Task<List<CashBoxDto>> GetCashBoxesForUserAsync(string userId)
        {
            var boxes = new List<CashBox>();

            var petty = await _unitOfWork.Repository<PettyHolder>()
                .GetAllAsync(x => x.UserId == userId);

            if (petty.Any())
                boxes.AddRange(petty.SelectMany(x => x.CashBoxes).ToList());

            var deposit = await _unitOfWork.Repository<DepositCollector>()
                .GetAllAsync(x => x.UserId == userId);

            if (deposit.Any())
                boxes.AddRange(deposit.SelectMany(x => x.CashBoxes).ToList());

            var userCities = await _unitOfWork.Repository<UserCity>()
                .GetAllAsync(x => x.UserId == userId);

            if (userCities.Any())
            {
                var cityIds = userCities.Select(x => x.CityId).ToList();

                var branches = await _unitOfWork.Repository<Branch>()
                    .GetAllAsync(x => cityIds.Contains(x.CityId));

                var branchIds = branches.Select(x => x.Id).ToList();

                var branchBoxes = await _unitOfWork.Repository<CashBox>()
                    .GetAllAsync(x => branchIds.Contains(x.Id));

                boxes.AddRange(branchBoxes);
            }

            if (!petty.Any() && !deposit.Any() && !userCities.Any())
            {
                var allBoxes = await _unitOfWork.Repository<CashBox>().GetAllAsync();
                boxes = allBoxes.ToList();
            }

            return _mapper.Map<List<CashBoxDto>>(boxes.Distinct().ToList());
        }
    }
}
