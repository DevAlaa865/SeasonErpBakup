using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers
{
    public class ManualPostingRequestDto
    {
        public int BranchId { get; set; }
        public DateTime Date { get; set; }
        public int? DepositCollectorId { get; set; } // اختياري

        public decimal Amount { get; set; }   // 🔥 المبلغ اليدوي الجديد
    }
}
