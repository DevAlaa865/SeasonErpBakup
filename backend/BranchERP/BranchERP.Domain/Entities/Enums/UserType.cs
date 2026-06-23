using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities.Enums
{
    public enum UserType
    {
        Branch = 1,       // مستخدم فرع
        RegionManager = 2,  // مدير مدينة
        Central = 3       // مستخدم مركزي
    }
}
