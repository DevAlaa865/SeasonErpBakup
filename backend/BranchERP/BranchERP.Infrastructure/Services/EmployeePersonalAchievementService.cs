using AutoMapper;
using BranchERP.Application.DTOs.EmployeePersonalAchievement;
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
    public class EmployeePersonalAchievementService : IEmployeePersonalAchievementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeePersonalAchievementService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeePersonalAchievementDto?> GetByPersonalTargetIdAsync(int employeePersonalTargetId)
        {
            var repo = _unitOfWork.Repository<EmployeePersonalAchievement>();

            var entity = await repo.GetAsync(
                x => x.EmployeePersonalTargetId == employeePersonalTargetId
            );

            if (entity == null)
                return null;

            return _mapper.Map<EmployeePersonalAchievementDto>(entity);
        }

        public async Task<EmployeePersonalAchievementDto> CreateOrUpdateAsync(EmployeePersonalAchievementCreateDto dto)
        {
            var repo = _unitOfWork.Repository<EmployeePersonalAchievement>();
            var targetRepo = _unitOfWork.Repository<EmployeePersonalTarget>();

            // 1) نجيب التارجت الشخصي
            var personalTarget = await targetRepo.GetAsync(
                x => x.Id == dto.EmployeePersonalTargetId,
                include: q => q.Include(x => x.ShiftHeader)
                               .Include(x => x.Employee)
            );

            if (personalTarget == null)
                throw new Exception("Personal target not found");

            // 2) نجيب الإنجاز لو موجود
            var entity = await repo.GetAsync(
                x => x.EmployeePersonalTargetId == dto.EmployeePersonalTargetId
            );

            bool isNew = false;

            if (entity == null)
            {
                isNew = true;
                entity = _mapper.Map<EmployeePersonalAchievement>(dto);
                entity.EmployeePersonalTargetId = dto.EmployeePersonalTargetId;
                entity.EnteredAt = DateTime.UtcNow;
            }
            else
            {
                entity.AchievedAmount = dto.AchievedAmount;
                entity.InvoicesCount = dto.InvoicesCount;
                entity.ItemsCount = dto.ItemsCount;
                entity.AttachmentPath = dto.AttachmentPath;
            }

            // 3) حساب نسبة الإنجاز
            if (personalTarget.PersonalTargetAmount > 0)
            {
                entity.AchievementPercentage =
                    Math.Round((entity.AchievedAmount / personalTarget.PersonalTargetAmount) * 100, 2);
            }

            // 4) هل حقق التارجت؟
            entity.IsTargetAchieved = entity.AchievedAmount >= personalTarget.PersonalTargetAmount;

            // 5) حساب العمولة (مثال بسيط – نعدله بعدين حسب قواعدك)
            entity.CommissionAmount = entity.IsTargetAchieved
                ? Math.Round(entity.AchievedAmount * 0.02m, 2) // 2% عمولة
                : 0;

            // 6) حفظ
            if (isNew)
                await repo.AddAsync(entity);
            else
                repo.Update(entity);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<EmployeePersonalAchievementDto>(entity);
        }
    }
}
