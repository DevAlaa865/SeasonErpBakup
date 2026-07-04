using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities.Enums
{
    public enum ExpenseCategory
    {
        GeneralExpense = 1,        // مصروفات عامة
        OperationalExpense = 2,    // تشغيل
        Salary = 3,                // رواتب
        Refund = 4,                // مرتجعات
        PettyAssignment = 5,       // عهدة لموظف
        PettyHolderExpense = 6,    // مصروفات صاحب عهدة
        CollectorTransfer = 7,     // تحويل بين مسؤولي الإيداع
        Adjustment = 8             // تسوية
    }
}
