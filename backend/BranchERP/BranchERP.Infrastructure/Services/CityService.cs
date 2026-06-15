using AutoMapper;
using BranchERP.Application.DTOs.Common;
using BranchERP.Application.DTOs.City;
using BranchERP.Application.Interfaces;
using BranchERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BranchERP.Infrastructure.Services
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ============================
        // Get All
        // ============================
        public async Task<ApiResponse<IReadOnlyList<CityDto>>> GetAllAsync()
        {
            var repo = _unitOfWork.Repository<City>();

            var cities = await repo.GetAllAsync(
                filter: null,
                include: q => q.Include(c => c.Region)
            );

            var data = _mapper.Map<IReadOnlyList<CityDto>>(cities);
            return ApiResponse<IReadOnlyList<CityDto>>.Ok(data);
        }

        // ============================
        // Get Paged
        // ============================
        public async Task<ApiResponse<IReadOnlyList<CityDto>>> GetPagedAsync(
            int pageIndex,
            int pageSize,
            string? search = null)
        {
            var repo = _unitOfWork.Repository<City>();

            Expression<Func<City, bool>>? filter = null;

            if (!string.IsNullOrWhiteSpace(search))
                filter = c => c.CityName.Contains(search);

            var cities = await repo.GetAllAsync(
                filter,
                include: q => q.Include(c => c.Region)
            );

            var paged = cities
                .OrderBy(c => c.CityName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var data = _mapper.Map<IReadOnlyList<CityDto>>(paged);
            return ApiResponse<IReadOnlyList<CityDto>>.Ok(data);
        }

        // ============================
        // Get By Id
        // ============================
        public async Task<ApiResponse<CityDto>> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<City>();

            var cities = await repo.GetAllAsync(
                filter: c => c.Id == id,
                include: q => q.Include(c => c.Region)
            );

            var entity = cities.FirstOrDefault();
            if (entity is null)
                return ApiResponse<CityDto>.Fail("City not found");

            var dto = _mapper.Map<CityDto>(entity);
            return ApiResponse<CityDto>.Ok(dto);
        }

        // ============================
        // Create
        // ============================
        public async Task<ApiResponse<CityDto>> CreateAsync(CityCreateUpdateDto model)
        {
            var repo = _unitOfWork.Repository<City>();
            var entity = _mapper.Map<City>(model);

            await repo.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            var dto = _mapper.Map<CityDto>(entity);
            return ApiResponse<CityDto>.Ok(dto, "City created successfully");
        }

        // ============================
        // Update
        // ============================
        public async Task<ApiResponse<CityDto>> UpdateAsync(int id, CityCreateUpdateDto model)
        {
            var repo = _unitOfWork.Repository<City>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null)
                return ApiResponse<CityDto>.Fail("City not found");

            _mapper.Map(model, entity);
            repo.Update(entity);
            await _unitOfWork.CompleteAsync();

            var dto = _mapper.Map<CityDto>(entity);
            return ApiResponse<CityDto>.Ok(dto, "City updated successfully");
        }

        // ============================
        // Delete
        // ============================
        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<City>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null)
                return ApiResponse<bool>.Fail("City not found");

            repo.Delete(entity);
            await _unitOfWork.CompleteAsync();

            return ApiResponse<bool>.Ok(true, "City deleted successfully");
        }
    }
}
