using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.ExpenseVoucher
{
    public class ApproveExpenseVoucherRequest
    {
        public int VoucherId { get; set; }

        public string ApprovedByUserId { get; set; } = string.Empty;

        public string? ManagerNotes { get; set; }
    }
}
