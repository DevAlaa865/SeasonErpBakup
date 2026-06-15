using BranchERP.Application.DTOs.BranchControlIssues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces
{
    public interface IBranchControlIssueService
    {
        Task AddIssuesAsync(List<CreateBranchControlIssueDto> issues, string userName);
        Task<List<BranchControlIssueListDto>> GetAllIssuesAsync(BranchControlIssueFilterDto filter);
        Task<BranchControlIssueListDto?> GetIssueByIdAsync(int id);
        Task<bool> UpdateIssueAsync(UpdateBranchControlIssueDto dto);

        Task<List<ManagerBranchControlIssueListDto>> GetManagerReportAsync(ManagerBranchControlIssueFilterDto filter);
        Task<bool> ManagerApproveAsync(ManagerApproveDto dto);

        Task<List<AccountantBranchControlIssueListDto>> GetAccountantReportAsync(AccountantBranchControlIssueFilterDto filter);
        Task<AccountantBranchControlIssueDetailsDto?> GetAccountantDetailsAsync(int id);

    }
}
