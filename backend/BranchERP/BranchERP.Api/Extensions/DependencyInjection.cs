using Application.Interfaces;
using BranchERP.Application.Interfaces;
using BranchERP.Application.Interfaces.Reports.DailyReports;
using BranchERP.Infrastructure.Data;
using BranchERP.Infrastructure.Services;
using BranchERP.Infrastructure.Services.Reports.DailyReports;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BranchERP.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            // تسجيل UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IActivityTypeService, ActivityTypeService>();
            services.AddScoped<IShortageTypeService, ShortageTypeService>();
            services.AddScoped<IBranchSalesDailyService, BranchSalesDailyService>();
            services.AddScoped<PermissionService>();
            services.AddScoped<IAuthorizationAdminService, AuthorizationAdminService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ICityBranchSalesReportService, CityBranchSalesReportService>();
            services.AddScoped<IBranchDailyTargetService, BranchDailyTargetService>();
            services.AddScoped<IBranchDailyPerformanceService, BranchDailyPerformanceService>();
            services.AddScoped<IEmployeeShiftTargetHeaderService, EmployeeShiftTargetHeaderService>();
            services.AddScoped<IEmployeePersonalAchievementService, EmployeePersonalAchievementService>();
            services.AddScoped<ICommissionRuleService, CommissionRuleService>();
            services.AddScoped<IBranchDailyReturnService, BranchDailyReturnService>();
            services.AddScoped<IBranchDailyDifferenceReportService, BranchDailyDifferenceReportService>();
            services.AddScoped<TokenService>();
            // هنا بعدين هنضيف:
            // - Services
            // - AutoMapper
            // - Validators
            // - Custom Repositories
            // - Authentication
            // - Authorization
            // - CORS
            // - Logging
            // - Swagger Config
            // - أي حاجة تخص المشروع

            return services;
        }
    }
}
