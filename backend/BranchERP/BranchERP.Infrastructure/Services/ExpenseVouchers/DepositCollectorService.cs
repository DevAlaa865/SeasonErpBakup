using AutoMapper;
using BranchERP.Application.DTOs.ExpenseVouchers.Users;
using BranchERP.Application.Interfaces;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using BranchERP.Domain.Entities;
using BranchERP.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BranchERP.Infrastructure.Services.ExpenseVouchers
{
    public class DepositCollectorService : IDepositCollectorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public DepositCollectorService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        // 🔥 إنشاء مسؤول إيداع مع عدة مدن
        public async Task<DepositCollectorDto> CreateAsync(CreateDepositCollectorDto dto)
        {
            var repo = _unitOfWork.Repository<DepositCollector>();
            var linkRepo = _unitOfWork.Repository<DepositCollectorCity>();

            var entity = new DepositCollector
            {
                UserId = dto.UserId,
                RegionId = dto.RegionId,
                IsActive = true
            };

            await repo.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            // إضافة المدن
            foreach (var cityId in dto.CityIds)
            {
                await linkRepo.AddAsync(new DepositCollectorCity
                {
                    DepositCollectorId = entity.Id,
                    CityId = cityId
                });
            }

            await _unitOfWork.CompleteAsync();

            return await BuildDto(entity.Id);
        }

        // 🔥 تحديث مسؤول إيداع + المدن
        public async Task<bool> UpdateAsync(UpdateDepositCollectorDto dto)
        {
            var repo = _unitOfWork.Repository<DepositCollector>();
            var linkRepo = _unitOfWork.Repository<DepositCollectorCity>();

            var entity = await repo.GetByIdAsync(dto.Id);
            if (entity == null)
                return false;

            entity.UserId = dto.UserId;
            entity.RegionId = dto.RegionId;
            entity.IsActive = dto.IsActive;

            repo.Update(entity);

            // حذف المدن القديمة
            var oldLinks = await linkRepo.GetAllAsync(x => x.DepositCollectorId == dto.Id);
            foreach (var link in oldLinks)
                linkRepo.Delete(link);

            // إضافة المدن الجديدة
            foreach (var cityId in dto.CityIds)
            {
                await linkRepo.AddAsync(new DepositCollectorCity
                {
                    DepositCollectorId = dto.Id,
                    CityId = cityId
                });
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }

        // 🔥 GetById
        public async Task<DepositCollectorDto?> GetByIdAsync(int id)
        {
            return await BuildDto(id);
        }

        // 🔥 GetAll
        public async Task<List<DepositCollectorDto>> GetAllAsync(bool? isActive = null)
        {
            var repo = _unitOfWork.Repository<DepositCollector>();

            var data = await repo.GetAllAsync(
                filter: x => !isActive.HasValue || x.IsActive == isActive.Value,
                include: q => q
                    .Include(x => x.DepositCollectorCities)
                        .ThenInclude(dc => dc.City)
                    .Include(x => x.Region)
                    .Include(x => x.CashBoxes)
            );

            var list = new List<DepositCollectorDto>();

            foreach (var entity in data)
                list.Add(await BuildDto(entity.Id));

            return list;
        }

        // 🔥 تفعيل / إلغاء تفعيل
        public async Task<bool> SetActiveAsync(int id, bool isActive)
        {
            var repo = _unitOfWork.Repository<DepositCollector>();

            var entity = await repo.GetByIdAsync(id);
            if (entity == null)
                return false;

            entity.IsActive = isActive;

            repo.Update(entity);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        // 🔥 بناء DTO كامل
        private async Task<DepositCollectorDto> BuildDto(int id)
        {
            var repo = _unitOfWork.Repository<DepositCollector>();

            var entity = await repo.GetAsync(
                x => x.Id == id,
                include: q => q
                    .Include(x => x.DepositCollectorCities)
                        .ThenInclude(dc => dc.City)
                    .Include(x => x.Region)
                    .Include(x => x.CashBoxes)
            );

            var user = await _userManager.FindByIdAsync(entity.UserId);

            var dto = _mapper.Map<DepositCollectorDto>(entity);

            dto.UserName = user?.UserName ?? "";
            dto.PhoneNumber = user?.PhoneNumber ?? "";

            return dto;
        }
    }
}
