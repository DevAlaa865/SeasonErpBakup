using BranchERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.EmployeeShiftTarget
{
    public class EmployeeShiftTargetHeaderCreateDto
    {
        public int BranchId { get; set; }
        public DateTime TargetDate { get; set; }
        public WorkShift Shift { get; set; }

        public decimal TotalPersonalTargetAmount { get; set; }
    }
}
