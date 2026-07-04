using AutoMapper;
using BranchERP.Application.Interfaces;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using BranchERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Infrastructure.Services.ExpenseVouchers
{
    public class UserBranchService : IUserBranchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserBranchService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<BranchDto>> GetMyBranchesAsync(string userId)
        {
            var userCities = await _unitOfWork.Repository<UserCity>()
                .GetAllAsync(x => x.UserId == userId);

            if (!userCities.Any())
                return new List<BranchDto>();

            var cityIds = userCities.Select(x => x.CityId).ToList();

            var branches = await _unitOfWork.Repository<Branch>()
                .GetAllAsync(x => cityIds.Contains(x.CityId));

            return _mapper.Map<List<BranchDto>>(branches);
        }
    }
}
