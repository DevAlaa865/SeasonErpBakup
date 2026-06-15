using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Domain.Entities.Enums
{
    public enum BranchControlIssueStatus
    {
        Pending = 0,     // تم الإرسال من الحسابات – في انتظار الرقابة
        InProgress = 1,  // تحت المعالجة
        Resolved = 2     // تمت المعالجة
    }
}
