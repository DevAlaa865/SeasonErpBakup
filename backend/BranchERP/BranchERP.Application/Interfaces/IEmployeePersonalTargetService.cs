using BranchERP.Application.DTOs.EmployeePersonalTarget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces
{
    public interface IEmployeePersonalTargetService
    {
        Task<List<EmployeePersonalTargetDto>> GetByShiftHeaderAsync(int shiftHeaderId);

        Task<EmployeePersonalTargetDto> CreateAsync(EmployeePersonalTargetCreateDto dto);

        Task<EmployeePersonalTargetDto> UpdateAsync(int id, EmployeePersonalTargetCreateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
