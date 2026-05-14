using BranchERP.Application.DTOs.CityBranchSales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.Interfaces
{
    public interface ICityBranchSalesReportService
    {
        Task<List<CityBranchSalesRowDto>> GetCityBranchSalesSummaryAsync(CityBranchSalesFilterDto filter);

        Task<List<CitySalesChartDto>> GetCitySalesChartAsync(CityBranchSalesFilterDto filter);
    }
}
