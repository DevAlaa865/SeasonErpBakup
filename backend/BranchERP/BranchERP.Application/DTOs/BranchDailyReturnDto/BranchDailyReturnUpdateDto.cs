using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.BranchDailyReturnDto
{
    public class BranchDailyReturnUpdateDto
    {
        public decimal ReturnAmount { get; set; }
        public int ReturnType { get; set; } // 1 أو 2
        public int BranchNumber { get; set; }
        public DateTime ReturnDate { get; set; }
        public string? Notes { get; set; }
    }
}
