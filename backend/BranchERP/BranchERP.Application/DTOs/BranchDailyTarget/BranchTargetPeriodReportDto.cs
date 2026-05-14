using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BranchDailyTarget
{
    public class BranchTargetPeriodReportDto
    {
        public string CityName { get; set; }
        public string BranchName { get; set; }
        public string SupervisorName { get; set; }

        public List<BranchTargetPeriodRowDto> Rows { get; set; }
    }
}
