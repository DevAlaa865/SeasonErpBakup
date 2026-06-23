using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces.Reports.SalesSummaryReports
{
    public interface IUserContextService
    {
        string UserType { get; }
        int BranchId { get; }
        List<int> CityIds { get; }
        string UserId { get; }
        string UserName { get; }
    }
}
