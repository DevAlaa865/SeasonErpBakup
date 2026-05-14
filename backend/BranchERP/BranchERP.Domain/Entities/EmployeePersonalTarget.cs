using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{
    public class EmployeePersonalTarget : BaseEntity
    {
        public int ShiftHeaderId { get; set; }
        public EmployeeShiftTargetHeader ShiftHeader { get; set; } = null!;

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;   // مش لازم مربوط بالفرع

        public decimal PersonalTargetAmount { get; set; }

        public string? Status { get; set; }   // Active / Off / Absent (ممكن نعملها enum بعدين)

        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public EmployeePersonalAchievement? Achievement { get; set; }
    }
}
