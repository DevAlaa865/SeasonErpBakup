using AutoMapper;
using BranchERP.Application.DTOs.ExpenseVouchers.Lookups;
using BranchERP.Application.Interfaces;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using BranchERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Infrastructure.Services.ExpenseVouchers
{
    public class ExpenseTypeService : IExpenseTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExpenseTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ============================================================
        // 1) Get All
        // ============================================================
        public async Task<List<ExpenseTypeDto>> GetAllAsync(bool? isActive = null)
        {
            var repo = _unitOfWork.Repository<ExpenseType>();

            var data = await repo.GetAllAsync(
                filter: x => !isActive.HasValue || x.IsActive == isActive.Value
            );

            return _mapper.Map<List<ExpenseTypeDto>>(data);
        }

        // ============================================================
        // 2) Get By Id
        // ============================================================
        public async Task<ExpenseTypeDto?> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<ExpenseType>();

            var entity = await repo.GetByIdAsync(id);

            return _mapper.Map<ExpenseTypeDto>(entity);
        }

        // ============================================================
        // 3) Create
        // ============================================================
        public async Task<ExpenseTypeDto> CreateAsync(ExpenseTypeDto dto)
        {
            var repo = _unitOfWork.Repository<ExpenseType>();

            var entity = _mapper.Map<ExpenseType>(dto);
            entity.IsActive = true;

            await repo.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ExpenseTypeDto>(entity);
        }

        // ============================================================
        // 4) Update
        // ============================================================
        public async Task<bool> UpdateAsync(ExpenseTypeDto dto)
        {
            var repo = _unitOfWork.Repository<ExpenseType>();

            var entity = await repo.GetByIdAsync(dto.Id);

            if (entity == null)
                return false;

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;

            repo.Update(entity);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        // ============================================================
        // 5) Activate / Deactivate
        // ============================================================
        public async Task<bool> SetActiveAsync(int id, bool isActive)
        {
            var repo = _unitOfWork.Repository<ExpenseType>();

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
