using AutoMapper;
using BranchERP.Application.DTOs.ExpenseVouchers.CashBox;
using BranchERP.Application.DTOs.ExpenseVouchers.Users;
using BranchERP.Application.Interfaces;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using BranchERP.Domain.Entities;
using BranchERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Infrastructure.Services.ExpenseVouchers
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ============================================================
        // DepositCollector (DTO كامل)
        // ============================================================
        public async Task<DepositCollectorDto?> GetDepositCollectorByIdAsync(int id)
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

            if (entity == null)
                return null;

            var dto = _mapper.Map<DepositCollectorDto>(entity);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == entity.UserId);

            if (user != null)
            {
                dto.UserName = user.DisplayName;
                dto.PhoneNumber = user.PhoneNumber!;
            }

            return dto;
        }

        public async Task<List<DepositCollectorDto>> GetAllDepositCollectorsAsync(bool? isActive = null)
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

            var list = _mapper.Map<List<DepositCollectorDto>>(data);

            foreach (var item in list)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == item.UserId);

                if (user != null)
                {
                    item.UserName = user.DisplayName;
                    item.PhoneNumber = user.PhoneNumber!;
                }
            }

            return list;
        }

        // ============================================================
        // PettyHolder (DTO كامل)
        // ============================================================
        public async Task<PettyHolderDto?> GetPettyHolderByIdAsync(int id)
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

        public async Task<List<PettyHolderDto>> GetAllPettyHoldersAsync(bool? isActive = null)
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

        // ============================================================
        // CashBoxes
        // ============================================================
        public async Task<List<CashBoxDto>> GetUserCashBoxesAsync(int userId)
        {
            var repo = _unitOfWork.Repository<CashBox>();

            var data = await repo.GetAllAsync(
                filter: x => x.DepositCollectorId == userId,
                include: q => q.Include(x => x.Transactions)
            );

            return _mapper.Map<List<CashBoxDto>>(data);
        }

        public async Task<bool> AssignCashBoxToUserAsync(int userId, int cashBoxId)
        {
            var repo = _unitOfWork.Repository<CashBox>();

            var box = await repo.GetByIdAsync(cashBoxId);

            if (box == null)
                return false;

            box.DepositCollectorId = userId;

            repo.Update(box);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
