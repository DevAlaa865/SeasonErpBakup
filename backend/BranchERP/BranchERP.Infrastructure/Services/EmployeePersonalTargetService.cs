using AutoMapper;
using BranchERP.Application.DTOs.EmployeePersonalTarget;
using BranchERP.Application.Interfaces;
using BranchERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Infrastructure.Services
{
    public class EmployeePersonalTargetService : IEmployeePersonalTargetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeePersonalTargetService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EmployeePersonalTargetDto>> GetByShiftHeaderAsync(int shiftHeaderId)
        {
            var repo = _unitOfWork.Repository<EmployeePersonalTarget>();

            var list = await repo.GetAllAsync(
                filter: x => x.ShiftHeaderId == shiftHeaderId,
                include: q => q.Include(x => x.Employee)
            );

            return list
                .Select(x => _mapper.Map<EmployeePersonalTargetDto>(x))
                .ToList();
        }

        public async Task<EmployeePersonalTargetDto> CreateAsync(EmployeePersonalTargetCreateDto dto)
        {
            var repo = _unitOfWork.Repository<EmployeePersonalTarget>();

            var entity = _mapper.Map<EmployeePersonalTarget>(dto);

            await repo.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<EmployeePersonalTargetDto>(entity);
        }

        public async Task<EmployeePersonalTargetDto> UpdateAsync(int id, EmployeePersonalTargetCreateDto dto)
        {
            var repo = _unitOfWork.Repository<EmployeePersonalTarget>();

            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
                throw new Exception("Target not found");

            entity.PersonalTargetAmount = dto.PersonalTargetAmount;
            entity.Status = dto.Status;
            entity.EmployeeId = dto.EmployeeId;
            entity.ShiftHeaderId = dto.ShiftHeaderId;
            entity.UpdatedAt = DateTime.UtcNow;

            repo.Update(entity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<EmployeePersonalTargetDto>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<EmployeePersonalTarget>();

            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
                return false;

            repo.Delete(entity);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
