using BranchERP.Application.DTOs.CommissionRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces
{
    public interface ICommissionRuleService
    {
        Task<List<CommissionRuleDto>> GetAllAsync();
        Task<CommissionRuleDto?> GetByIdAsync(int id);
        Task<CommissionRuleDto> CreateAsync(CommissionRuleCreateUpdateDto dto);
        Task<CommissionRuleDto> UpdateAsync(int id, CommissionRuleCreateUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
