using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.Reports
{
    public class BranchDailyPerformanceReportFilterDto
    {
        public DateTime Date { get; set; }      // اليوم المطلوب تقريره

        public int? CityId { get; set; }            // المدينة (اختياري)
        public int? BranchId { get; set; }          // فرع محدد (اختياري)
    }
}
