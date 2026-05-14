using BranchERP.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{
    public class BranchDailyReturn : BaseEntity
    {
        public DateTime ReturnDate { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }   // ربط مباشر بالفرع

        public decimal ReturnAmount { get; set; }

        public BranchReturnType ReturnType { get; set; } // enum: Cash / Replacement

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }   // ← أضف دي
    }

}
