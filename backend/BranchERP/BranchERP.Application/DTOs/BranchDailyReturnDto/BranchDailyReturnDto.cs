using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BranchDailyReturnDto
{
    public class BranchDailyReturnDto
    {
        public int Id { get; set; }
        public DateTime ReturnDate { get; set; }
        public int BranchId { get; set; }
        public int BranchNumber { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public decimal ReturnAmount { get; set; }
        public int ReturnType { get; set; } // 1 كاش - 2 استبدال
        public string? Notes { get; set; }
    }
}
