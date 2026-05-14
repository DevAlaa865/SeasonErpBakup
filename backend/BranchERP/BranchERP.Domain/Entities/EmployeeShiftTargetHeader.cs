using BranchERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{
    public class EmployeeShiftTargetHeader : BaseEntity
    {
        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;

        public DateTime TargetDate { get; set; }

        public WorkShift Shift { get; set; }   // Morning / Evening إلخ

        public decimal TotalPersonalTargetAmount { get; set; }   // إجمالي التارجت الشخصي للشيفت

        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<EmployeePersonalTarget> PersonalTargets { get; set; }
            = new List<EmployeePersonalTarget>();
    }
}
