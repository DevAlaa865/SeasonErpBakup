using BranchERP.Application.Interfaces;
using BranchERP.Application.Interfaces.Reports.SalesSummaryReports;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace BranchERP.Infrastructure.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _http;

        public UserContextService(IHttpContextAccessor http)
        {
            _http = http;
        }

        private ClaimsPrincipal User => _http.HttpContext?.User;

        public string UserId =>
            User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        public string UserName =>
            User?.FindFirst(ClaimTypes.Name)?.Value ?? "";

        public string UserType =>
            User?.FindFirst("userType")?.Value ?? "";

        public int BranchId =>
            int.TryParse(User?.FindFirst("branchId")?.Value, out var id) ? id : 0;

        public List<int> CityIds =>
            User?.FindFirst("cityIds")?.Value?
                .Split(',')
                .Select(int.Parse)
                .ToList()
            ?? new List<int>();
    }
}
