using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities.Enums
{
    public enum TransactionType
    {
        Expense = 1,              // مصروفات
        PettyCashAssignment = 2,  // صرف عهدة
        BranchDeposit = 3,        // استلام نقدية فرع
        Transfer = 4,             // تحويل بين صناديق
        Adjustment = 5            // تسوية
    }
}
