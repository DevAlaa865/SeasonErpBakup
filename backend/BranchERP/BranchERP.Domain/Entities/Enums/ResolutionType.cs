using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities.Enums
{
    public enum ResolutionType
    {
        EmployeeFault = 1,       // تحميل موظف
        SystemError = 2,         // خطأ نظام
        InventoryDifference = 3, // فرق جرد
        Settled = 4,             // تم التسوية
        UnderReview = 5          // تحت المراجعة
    }
}
