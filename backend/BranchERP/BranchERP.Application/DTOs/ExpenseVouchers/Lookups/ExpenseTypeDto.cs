using BranchERP.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.Lookups
{
    public class ExpenseTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string? Description { get; set; }

        // 🔥 التصنيف
        public ExpenseCategory Category { get; set; }
    }
}
