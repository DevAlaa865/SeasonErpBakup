using AutoMapper;
using BranchERP.Application.DTOs.ExpenseVouchers.Users;
using BranchERP.Application.Interfaces;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using BranchERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BranchERP.Infrastructure.Services
{
    public class PettyHolderService : IPettyHolderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PettyHolderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ============================================================
        // 1) Create PettyHolder
        // ============================================================
        public async Task<PettyHolderDto> CreateAsync(CreatePettyHolderDto dto)
        {
            var repo = _unitOfWork.Repository<PettyHolder>();

            var entity = _mapper.Map<PettyHolder>(dto);
            entity.IsActive = true;

            await repo.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<PettyHolderDto>(entity);
        }

        // ============================================================
        // 2) Update PettyHolder
        // ============================================================
        public async Task<bool> UpdateAsync(UpdatePettyHolderDto dto)
        {
            var repo = _unitOfWork.Repository<PettyHolder>();

            var entity = await repo.GetByIdAsync(dto.Id);

            if (entity == null)
                return false;

            entity.Name = dto.Name;
            entity.PhoneNumber = dto.PhoneNumber;
            entity.CityId  = dto.CityId ;
            entity.RegionId = dto.RegionId;
            entity.IsActive = dto.IsActive;

            repo.Update(entity);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        // ============================================================
        // 3) Get PettyHolder By Id
        // ============================================================
        public async Task<PettyHolderDto?> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<PettyHolder>();

            var entity = await repo.GetAsync(
                x => x.Id == id,
                include: q => q
                    .Include(x => x.City)
                    .Include(x => x.Region)
                    .Include(x => x.CashBoxes)
            );

            return _mapper.Map<PettyHolderDto>(entity);
        }

        // ============================================================
        // 4) Get All PettyHolders
        // ============================================================
        public async Task<List<PettyHolderDto>> GetAllAsync(bool? isActive = null)
        {
            var repo = _unitOfWork.Repository<PettyHolder>();

            var data = await repo.GetAllAsync(
                filter: x => !isActive.HasValue || x.IsActive == isActive.Value,
                include: q => q
                    .Include(x => x.City)
                    .Include(x => x.Region)
                    .Include(x => x.CashBoxes)
            );

            return _mapper.Map<List<PettyHolderDto>>(data);
        }

        // ============================================================
        // 5) Activate / Deactivate PettyHolder
        // ============================================================
        public async Task<bool> SetActiveAsync(int id, bool isActive)
        {
            var repo = _unitOfWork.Repository<PettyHolder>();

            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
                return false;

            entity.IsActive = isActive;

            repo.Update(entity);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
