using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.EmployeePersonalTarget
{
    public class EmployeePersonalTargetDto
    {
        public int Id { get; set; }

        public int ShiftHeaderId { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;

        public decimal PersonalTargetAmount { get; set; }

        public string? Status { get; set; }
    }
}
