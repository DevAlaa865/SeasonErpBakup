using AutoMapper;
using BranchERP.Application.DTOs.CommissionRule;
using BranchERP.Application.Interfaces;
using BranchERP.Domain.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Infrastructure.Services
{
    public class CommissionRuleService : ICommissionRuleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommissionRuleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CommissionRuleDto>> GetAllAsync()
        {
            var repo = _unitOfWork.Repository<CommissionRule>();
            var list = await repo.Query().OrderBy(x => x.MinPercentage).ToListAsync();
            return _mapper.Map<List<CommissionRuleDto>>(list);
        }

        public async Task<CommissionRuleDto?> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<CommissionRule>();
            var entity = await repo.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<CommissionRuleDto>(entity);
        }

        public async Task<CommissionRuleDto> CreateAsync(CommissionRuleCreateUpdateDto dto)
        {
            var repo = _unitOfWork.Repository<CommissionRule>();

            var entity = _mapper.Map<CommissionRule>(dto);
            entity.CreatedAt = DateTime.UtcNow;

            await repo.AddAsync(entity);
            await _unitOfWork.CompleteAsync();   // 👈 الصح

            return _mapper.Map<CommissionRuleDto>(entity);
        }

        public async Task<CommissionRuleDto> UpdateAsync(int id, CommissionRuleCreateUpdateDto dto)
        {
            var repo = _unitOfWork.Repository<CommissionRule>();
            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
                throw new Exception("Commission rule not found");

            _mapper.Map(dto, entity);

            repo.Update(entity);
            await _unitOfWork.CompleteAsync();   // 👈 الصح

            return _mapper.Map<CommissionRuleDto>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<CommissionRule>();
            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
                return false;

            repo.Delete(entity);
            await _unitOfWork.CompleteAsync();   // 👈 الصح

            return true;
        }

    }
}
