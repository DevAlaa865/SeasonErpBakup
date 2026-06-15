using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.Reports.DailyReports
{
    public class BranchDailyDifferenceReportFilterDto
    {
        public List<int>? CityIds { get; set; }
        public List<int>? BranchIds { get; set; }

        public int? BranchNumber { get; set; }

        // 🔥 الفلاتر الجديدة
        public bool? IsAllowedShortage { get; set; }   // من -35 إلى -1
        public bool? IsBigShortage { get; set; }       // أقل من -35

        // 🔥 فلاتر الزيادة الجديدة
        public bool? IsSmallIncrease { get; set; }     // 1 إلى 35
        public bool? IsBigIncrease { get; set; }       // أكبر من 35

        public bool? IsNetworkReport { get; set; }     // شبكة فقط

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
