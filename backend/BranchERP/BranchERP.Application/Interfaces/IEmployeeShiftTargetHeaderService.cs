using BranchERP.Application.DTOs.EmployeeShiftTarget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces
{
    public interface IEmployeeShiftTargetHeaderService
    {
        Task<EmployeeShiftTargetHeaderDto?> GetAsync(int id);

        Task<EmployeeShiftTargetHeaderDto> CreateAsync(EmployeeShiftTargetHeaderCreateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
