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

        // 1) Create PettyHolder مع مدن متعددة
        public async Task<PettyHolderDto> CreateAsync(CreatePettyHolderDto dto)
        {
            var holderRepo = _unitOfWork.Repository<PettyHolder>();
            var linkRepo = _unitOfWork.Repository<PettyHolderCity>();

            var entity = new PettyHolder
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                RegionId = dto.RegionId,
                IsActive = true
            };

            await holderRepo.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            foreach (var cityId in dto.CityIds)
            {
                await linkRepo.AddAsync(new PettyHolderCity
                {
                    PettyHolderId = entity.Id,
                    CityId = cityId
                });
            }

            await _unitOfWork.CompleteAsync();

            return await GetByIdAsync(entity.Id) ?? new PettyHolderDto();
        }

        // 2) Update PettyHolder + المدن
        public async Task<bool> UpdateAsync(UpdatePettyHolderDto dto)
        {
            var holderRepo = _unitOfWork.Repository<PettyHolder>();
            var linkRepo = _unitOfWork.Repository<PettyHolderCity>();

            var entity = await holderRepo.GetByIdAsync(dto.Id);
            if (entity == null)
                return false;

            entity.Name = dto.Name;
            entity.PhoneNumber = dto.PhoneNumber;
            entity.RegionId = dto.RegionId;
            entity.IsActive = dto.IsActive;

            holderRepo.Update(entity);

            var oldLinks = await linkRepo.GetAllAsync(x => x.PettyHolderId == dto.Id);
            foreach (var link in oldLinks)
                linkRepo.Delete(link);

            foreach (var cityId in dto.CityIds)
            {
                await linkRepo.AddAsync(new PettyHolderCity
                {
                    PettyHolderId = dto.Id,
                    CityId = cityId
                });
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }

        // 3) Get PettyHolder By Id
        public async Task<PettyHolderDto?> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<PettyHolder>();

            var entity = await repo.GetAsync(
                x => x.Id == id,
                include: q => q
                    .Include(x => x.PettyHolderCities)
                        .ThenInclude(pc => pc.City)
                    .Include(x => x.Region)
                    .Include(x => x.CashBoxes)
            );

            return _mapper.Map<PettyHolderDto>(entity);
        }

        // 4) Get All PettyHolders
        public async Task<List<PettyHolderDto>> GetAllAsync(bool? isActive = null)
        {
            var repo = _unitOfWork.Repository<PettyHolder>();

            var data = await repo.GetAllAsync(
                filter: x => !isActive.HasValue || x.IsActive == isActive.Value,
                include: q => q
                    .Include(x => x.PettyHolderCities)
                        .ThenInclude(pc => pc.City)
                    .Include(x => x.Region)
                    .Include(x => x.CashBoxes)
            );

            return _mapper.Map<List<PettyHolderDto>>(data);
        }

        // 5) Activate / Deactivate PettyHolder
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
