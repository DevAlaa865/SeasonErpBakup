using AutoMapper;
using BranchERP.Application.DTOs.EmployeeShiftTarget;
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
    public class EmployeeShiftTargetHeaderService : IEmployeeShiftTargetHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeShiftTargetHeaderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeShiftTargetHeaderDto?> GetAsync(int id)
        {
            var repo = _unitOfWork.Repository<EmployeeShiftTargetHeader>();

            var entity = await repo.GetAsync(
                x => x.Id == id,
                include: q => q.Include(x => x.Branch)
            );

            if (entity == null)
                return null;

            return _mapper.Map<EmployeeShiftTargetHeaderDto>(entity);
        }

        public async Task<EmployeeShiftTargetHeaderDto> CreateAsync(EmployeeShiftTargetHeaderCreateDto dto)
        {
            var repo = _unitOfWork.Repository<EmployeeShiftTargetHeader>();

            var entity = _mapper.Map<EmployeeShiftTargetHeader>(dto);

            await repo.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<EmployeeShiftTargetHeaderDto>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<EmployeeShiftTargetHeader>();

            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
                return false;

            repo.Delete(entity);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
