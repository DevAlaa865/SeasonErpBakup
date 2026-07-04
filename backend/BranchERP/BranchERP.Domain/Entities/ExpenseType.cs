using BranchERP.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{
    public class ExpenseType : BaseEntity
    {
        public string? Name { get; set; }
        public bool IsActive { get; set; }

        public string? Description { get; set; }

        // 🔥 تصنيف البند
        public ExpenseCategory Category { get; set; }

        //public ICollection<ExpenseVoucherLine> Lines { get; set; }
        //    = new List<ExpenseVoucherLine>();
    }
}
