using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities
{
    public class ExpenseType
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public bool IsActive { get; set; }

        public string Description { get; set; }   // اختياري

        // Navigation
        public ICollection<ExpenseVoucherLine> Lines { get; set; } = new List<ExpenseVoucherLine>();
    }
}
