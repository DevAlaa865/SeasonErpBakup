using AutoMapper;
using BranchERP.Application.DTOs.Common;
using BranchERP.Application.DTOs.Region;
using BranchERP.Application.Interfaces;
using BranchERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BranchERP.Infrastructure.Services
{
    public class RegionService : IRegionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ============================
        // Get All
        // ============================
        public async Task<ApiResponse<IReadOnlyList<RegionDto>>> GetAllAsync()
        {
            var repo = _unitOfWork.Repository<Region>();

            var regions = await repo.GetAllAsync(
                filter: null,
                include: q => q.Include(r => r.Country)
            );

            var data = _mapper.Map<IReadOnlyList<RegionDto>>(regions);
            return ApiResponse<IReadOnlyList<RegionDto>>.Ok(data);
        }

        // ============================
        // Get Paged
        // ============================
        public async Task<ApiResponse<IReadOnlyList<RegionDto>>> GetPagedAsync(
            int pageIndex,
            int pageSize,
            string? search = null)
        {
            var repo = _unitOfWork.Repository<Region>();

            Expression<Func<Region, bool>>? filter = null;

            if (!string.IsNullOrWhiteSpace(search))
                filter = r => r.RegionName.Contains(search);

            var regions = await repo.GetAllAsync(
                filter,
                include: q => q.Include(r => r.Country)
            );

            var paged = regions
                .OrderBy(r => r.RegionName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var data = _mapper.Map<IReadOnlyList<RegionDto>>(paged);
            return ApiResponse<IReadOnlyList<RegionDto>>.Ok(data);
        }

        // ============================
        // Get By Id
        // ============================
        public async Task<ApiResponse<RegionDto>> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<Region>();

            var regions = await repo.GetAllAsync(
                filter: r => r.Id == id,
                include: q => q.Include(r => r.Country)
            );

            var entity = regions.FirstOrDefault();
            if (entity is null)
                return ApiResponse<RegionDto>.Fail("Region not found");

            var dto = _mapper.Map<RegionDto>(entity);
            return ApiResponse<RegionDto>.Ok(dto);
        }

        // ============================
        // Create
        // ============================
        public async Task<ApiResponse<RegionDto>> CreateAsync(RegionCreateUpdateDto model)
        {
            var repo = _unitOfWork.Repository<Region>();
            var entity = _mapper.Map<Region>(model);

            await repo.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            var dto = _mapper.Map<RegionDto>(entity);
            return ApiResponse<RegionDto>.Ok(dto, "Region created successfully");
        }

        // ============================
        // Update
        // ============================
        public async Task<ApiResponse<RegionDto>> UpdateAsync(int id, RegionCreateUpdateDto model)
        {
            var repo = _unitOfWork.Repository<Region>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null)
                return ApiResponse<RegionDto>.Fail("Region not found");

            _mapper.Map(model, entity);
            repo.Update(entity);
            await _unitOfWork.CompleteAsync();

            var dto = _mapper.Map<RegionDto>(entity);
            return ApiResponse<RegionDto>.Ok(dto, "Region updated successfully");
        }

        // ============================
        // Delete
        // ============================
        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<Region>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null)
                return ApiResponse<bool>.Fail("Region not found");

            repo.Delete(entity);
            await _unitOfWork.CompleteAsync();

            return ApiResponse<bool>.Ok(true, "Region deleted successfully");
        }
    }
}
