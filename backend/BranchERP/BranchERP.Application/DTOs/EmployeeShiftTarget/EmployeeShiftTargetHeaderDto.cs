using BranchERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.EmployeeShiftTarget
{
    public class EmployeeShiftTargetHeaderDto
    {
        public int Id { get; set; }

        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;

        public DateTime TargetDate { get; set; }

        public WorkShift Shift { get; set; }
        public string ShiftName => Shift.ToString();

        public decimal TotalPersonalTargetAmount { get; set; }
    }
}
