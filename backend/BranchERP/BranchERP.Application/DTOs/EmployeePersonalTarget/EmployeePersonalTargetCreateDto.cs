using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.EmployeePersonalTarget
{
    public class EmployeePersonalTargetCreateDto
    {
        public int ShiftHeaderId { get; set; }

        public int EmployeeId { get; set; }
        public decimal PersonalTargetAmount { get; set; }

        public string? Status { get; set; }   // Active / Off / Absent
    }
}
