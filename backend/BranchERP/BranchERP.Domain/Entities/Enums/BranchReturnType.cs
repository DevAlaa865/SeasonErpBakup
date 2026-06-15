using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities.Enums
{
    public enum BranchReturnType
    {
        Cash = 1,      // كاش
        Replacement = 2, // استبدال
        Tabby = 3,         // تابى
        Tamara = 4,         // تمارا
        BankTransfere = 5,    // تحويل بنكى
        FaultEntry=6,       // ادخال خطأ 
        Other = 7           // اخري

    }
}
