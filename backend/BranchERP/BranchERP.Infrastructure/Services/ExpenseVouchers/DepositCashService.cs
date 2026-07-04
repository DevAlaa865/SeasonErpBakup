using BranchERP.Application.DTOs.ExpenseVouchers.Users;
using BranchERP.Application.Interfaces;
using BranchERP.Application.Interfaces.ExpenseVouchers;
using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace BranchERP.Infrastructure.Services.ExpenseVouchers
{
    public class DepositCashService : IDepositCashService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepositCashService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DepositCashSummaryDto> GetMyCashAsync(string userId)
        {
            var collectorRepo = _unitOfWork.Repository<DepositCollector>();

            // 🔥 هات مسؤول الإيداع + المدن المرتبطة بيه
            var collectors = await collectorRepo.GetAllAsync(
                x => x.UserId == userId,
                include: q => q
                    .Include(dc => dc.DepositCollectorCities)
                        .ThenInclude(dcc => dcc.City)
            );

            if (!collectors.Any())
                return new DepositCashSummaryDto();

            // 🔥 كل المدن اللي مسؤول الإيداع مرتبط بيها
            var cityIds = collectors
                .SelectMany(c => c.DepositCollectorCities)
                .Select(c => c.CityId)
                .Distinct()
                .ToList();

            if (!cityIds.Any())
                return new DepositCashSummaryDto();

            var branchRepo = _unitOfWork.Repository<Branch>();
            var transactionRepo = _unitOfWork.Repository<CashBoxTransaction>();

            // 🔥 كل الفروع التابعة لكل المدن
            var branches = await branchRepo.GetAllAsync(b => cityIds.Contains(b.CityId));
            var branchIds = branches.Select(b => b.Id).ToList();

            if (!branchIds.Any())
                return new DepositCashSummaryDto();

            // 🔥 كل معاملات الفروع
            var transactions = await transactionRepo.GetAllAsync(
                x => branchIds.Contains(x.BranchId ?? 0)
            );

            var summary = new DepositCashSummaryDto
            {
                TotalBranchCash = transactions
                    .Where(x => x.TransactionType == TransactionType.BranchDeposit)
                    .Sum(x => x.Amount),

                TotalReturns = transactions
                    .Where(x => x.TransactionType == TransactionType.Adjustment)
                    .Sum(x => x.Amount),

                TotalDeposited = transactions
                    .Where(x => x.TransactionType == TransactionType.Transfer)
                    .Sum(x => x.Amount),
            };

            summary.RemainingCash =
                summary.TotalBranchCash
                - summary.TotalReturns
                - summary.TotalDeposited;

            return summary;
        }
    }
}
