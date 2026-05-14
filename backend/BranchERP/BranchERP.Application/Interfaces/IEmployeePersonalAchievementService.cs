using BranchERP.Application.DTOs.EmployeePersonalAchievement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces
{
    public interface IEmployeePersonalAchievementService
    {
        Task<EmployeePersonalAchievementDto?> GetByPersonalTargetIdAsync(int employeePersonalTargetId);

        Task<EmployeePersonalAchievementDto> CreateOrUpdateAsync(EmployeePersonalAchievementCreateDto dto);
    }
}
