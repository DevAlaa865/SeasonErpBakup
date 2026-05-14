using AutoMapper;
using BranchERP.Application.DTOs.Common;
using BranchERP.Application.Interfaces;
using BranchERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BranchERP.Infrastructure.Services
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BranchService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ============================
        // Get All
        // ============================
        public async Task<ApiResponse<IReadOnlyList<BranchDto>>> GetAllAsync()
        {
            var repo = _unitOfWork.Repository<Branch>();

            var branches  = await repo.GetAllAsync(
                filter : null,
                include: q => q
                    .Include(b => b.City)
                    .Include(b => b.ActivityType)
                    .Include(b => b.Supervisor !)
            );

            var data = _mapper.Map<IReadOnlyList<BranchDto>>(branches);
            return ApiResponse<IReadOnlyList<BranchDto>>.Ok(data);
        }

        // ============================
        // Get Paged
        // ============================
        public async Task<ApiResponse<IReadOnlyList<BranchDto>>> GetPagedAsync(
            int pageIndex,
            int pageSize,
            string? search = null)
        {
            var repo = _unitOfWork.Repository<Branch>();

            Expression<Func<Branch, bool>>? filter = null;

            if (!string.IsNullOrWhiteSpace(search))
                filter = b => b.BranchName.Contains(search);

            var branches = await repo.GetAllAsync(
                filter,
                include: q => q
                    .Include(b => b.City)
                    .Include(b => b.ActivityType)
                    .Include(b => b.Supervisor!)
            );

            var paged = branches
                .OrderBy(b => b.BranchName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var data = _mapper.Map<IReadOnlyList<BranchDto>>(paged);
            return ApiResponse<IReadOnlyList<BranchDto>>.Ok(data);
        }

        // ============================
        // Get By Id
        // ============================
        public async Task<ApiResponse<BranchDto>> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<Branch>();

            var branches = await repo.GetAllAsync(
                filter: b => b.Id == id,
                include: q => q
                    .Include(b => b.City)
                    .Include(b => b.ActivityType)
                    .Include(b => b.Supervisor!)
            );

            var entity = branches.FirstOrDefault();
            if (entity is null)
                return ApiResponse<BranchDto>.Fail("Branch not found");

            var dto = _mapper.Map<BranchDto>(entity);
            return ApiResponse<BranchDto>.Ok(dto);
        }

        // ============================
        // Create
        // ============================
        public async Task<ApiResponse<BranchDto>> CreateAsync(BranchCreateUpdateDto model)
        {
            var repo = _unitOfWork.Repository<Branch>();
            var entity = _mapper.Map<Branch>(model);

            await repo.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            var dto = _mapper.Map<BranchDto>(entity);
            return ApiResponse<BranchDto>.Ok(dto, "Branch created successfully");
        }

        // ============================
        // Update
        // ============================
        public async Task<ApiResponse<BranchDto>> UpdateAsync(int id, BranchCreateUpdateDto model)
        {
            var repo = _unitOfWork.Repository<Branch>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null)
                return ApiResponse<BranchDto>.Fail("Branch not found");

            _mapper.Map(model, entity);
            repo.Update(entity);
            await _unitOfWork.CompleteAsync();

            var dto = _mapper.Map<BranchDto>(entity);
            return ApiResponse<BranchDto>.Ok(dto, "Branch updated successfully");
        }

        // ============================
        // Delete
        // ============================
        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<Branch>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null)
                return ApiResponse<bool>.Fail("Branch not found");

            repo.Delete(entity);
            await _unitOfWork.CompleteAsync();

            return ApiResponse<bool>.Ok(true, "Branch deleted successfully");
        }

        public async Task<ApiResponse<IReadOnlyList<BranchDto>>> GetByCityIdAsync(int cityId)
        {
            var repo = _unitOfWork.Repository<Branch>();

            var branches = await repo.GetAllAsync(
                filter: b => b.CityId == cityId,
                include: q => q
                    .Include(b => b.City)
                    .Include(b => b.ActivityType)
                    .Include(b => b.Supervisor!)
            );

            var data = _mapper.Map<IReadOnlyList<BranchDto>>(branches);
            return ApiResponse<IReadOnlyList<BranchDto>>.Ok(data);
        }

    }
}
