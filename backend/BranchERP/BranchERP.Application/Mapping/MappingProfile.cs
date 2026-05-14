using AutoMapper;
using BranchERP.Application.DTOs.ActivityType;
using BranchERP.Application.DTOs.BranchDailyPerformance;
using BranchERP.Application.DTOs.BranchDailyReturnDto;
using BranchERP.Application.DTOs.BranchDailyTarget;
using BranchERP.Application.DTOs.BranchSalesDaily;
using BranchERP.Application.DTOs.City;
using BranchERP.Application.DTOs.CommissionRule;
using BranchERP.Application.DTOs.Country;
using BranchERP.Application.DTOs.Employee;
using BranchERP.Application.DTOs.EmployeePersonalAchievement;
using BranchERP.Application.DTOs.EmployeePersonalTarget;
using BranchERP.Application.DTOs.EmployeeShiftTarget;
using BranchERP.Application.DTOs.Region;
using BranchERP.Application.DTOs.ShortageType;
using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.Enums;

namespace BranchERP.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ============================
            // Country / City / Region
            // ============================
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, CountryCreateUpdateDto>().ReverseMap();

            CreateMap<City, CityDto>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.CountryName));
            CreateMap<City, CityCreateUpdateDto>().ReverseMap();

            CreateMap<Region, RegionDto>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.CityName));
            CreateMap<RegionCreateUpdateDto, Region>().ReverseMap();
            // ============================
            // Branch
            // ============================
            CreateMap<Branch, BranchDto>()
                .ForMember(d => d.CityName, opt => opt.MapFrom(s => s.City.CityName))
                .ForMember(d => d.ActivityTypeName, opt => opt.MapFrom(s => s.ActivityType.ActivityName))
                .ForMember(d => d.SupervisorName, opt => opt.MapFrom(s => s.Supervisor != null ? s.Supervisor.FullName : null));

            CreateMap<BranchCreateUpdateDto, Branch>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Supervisor, opt => opt.Ignore());

            // ============================
            // Employee
            // ============================
            CreateMap<Employee, EmployeeDto>();

            CreateMap<EmployeeCreateUpdateDto, Employee>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // ============================
            // ActivityType
            // ============================
            CreateMap<ActivityType, ActivityTypeDto>().ReverseMap();
            CreateMap<ActivityType, ActivityTypeCreateUpdateDto>().ReverseMap();

            // ============================
            // ShortageType
            // ============================
            CreateMap<ShortageType, ShortageTypeDto>().ReverseMap();
            CreateMap<ShortageType, ShortageTypeCreateUpdateDto>().ReverseMap();

            // ============================
            // BranchSalesDaily - ShortageDetail
            // ============================
            CreateMap<BranchSalesShortageDetail, BranchSalesShortageDetailDto>()
                .ForMember(d => d.ShortageTypeName,
                    opt => opt.MapFrom(s => s.ShortageType.ShortageName))
                .ForMember(d => d.EmployeeName,
                    opt => opt.MapFrom(s => s.Employee != null ? s.Employee.FullName : null));

            CreateMap<BranchSalesShortageDetailCreateUpdateDto, BranchSalesShortageDetail>();

            // ============================
            // BranchSalesDaily - Header
            // ============================
            CreateMap<BranchSalesDaily, BranchSalesDailyDto>()
                .ForMember(d => d.BranchName,
                    opt => opt.MapFrom(s => s.Branch.BranchName))
                .ForMember(d => d.SupervisorName,
                    opt => opt.MapFrom(s => s.Supervisor != null ? s.Supervisor.FullName : null))
            .ForMember(d => d.GrandTotal, opt => opt.MapFrom(s => s.GrandTotal));
            CreateMap<BranchSalesDailyCreateUpdateDto, BranchSalesDaily>();

            // ============================
            // BranchDailyTarget - Detail
            // ============================
            CreateMap<BranchDailyTargetDetail, BranchDailyTargetDetailDto>()
                .ForMember(d => d.EmployeeName,
                    opt => opt.MapFrom(s => s.Employee.FullName));

            CreateMap<BranchDailyTargetDetailCreateUpdateDto, BranchDailyTargetDetail>();

            // ============================
            // BranchDailyTarget - Header
            // ============================
            CreateMap<BranchDailyTargetHeader, BranchDailyTargetHeaderDto>()
                .ForMember(d => d.BranchName,
                    opt => opt.MapFrom(s => s.Branch.BranchName));

            CreateMap<BranchDailyTargetHeaderCreateUpdateDto, BranchDailyTargetHeader>();

            // ============================
            // BranchDailyPerformance
            // ============================
            CreateMap<BranchDailyPerformance, BranchDailyPerformanceDto>()
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.BranchName));

            CreateMap<BranchDailyPerformanceCreateUpdateDto, BranchDailyPerformance>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
            // ============================
            // EmployeeShiftTargetHeader
            // ============================
            CreateMap<EmployeeShiftTargetHeader, EmployeeShiftTargetHeaderDto>()
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.BranchName));

            CreateMap<EmployeeShiftTargetHeaderCreateDto, EmployeeShiftTargetHeader>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());
            // ============================
            // EmployeePersonalTarget
            // ============================
            CreateMap<EmployeePersonalTarget, EmployeePersonalTargetDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FullName));

            CreateMap<EmployeePersonalTargetCreateDto, EmployeePersonalTarget>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Achievement, opt => opt.Ignore());
            // ============================
            // EmployeePersonalAchievement
            // ============================
            CreateMap<EmployeePersonalAchievement, EmployeePersonalAchievementDto>();

            CreateMap<EmployeePersonalAchievementCreateDto, EmployeePersonalAchievement>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.AchievementPercentage, opt => opt.Ignore())
                .ForMember(dest => dest.IsTargetAchieved, opt => opt.Ignore())
                .ForMember(dest => dest.CommissionAmount, opt => opt.Ignore())
                .ForMember(dest => dest.EnteredAt, opt => opt.Ignore())
                .ForMember(dest => dest.EnteredBy, opt => opt.Ignore());

            CreateMap<CommissionRule, CommissionRuleDto>();

            CreateMap<CommissionRuleCreateUpdateDto, CommissionRule>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));

            // ============================
            // BranchDailyReturn
            // ============================
            CreateMap<BranchDailyReturn, BranchDailyReturnDto>()
                .ForMember(dest => dest.BranchNumber, opt => opt.MapFrom(src => src.Branch.BranchNumber))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.BranchName))
                .ForMember(dest => dest.ReturnType, opt => opt.MapFrom(src => (int)src.ReturnType));

            CreateMap<BranchDailyReturnDto, BranchDailyReturn>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.BranchId, opt => opt.Ignore())
                .ForMember(dest => dest.ReturnDate, opt => opt.Ignore())
                .ForMember(dest => dest.ReturnType, opt => opt.MapFrom(src => (BranchReturnType)src.ReturnType));


        }
    }
}