using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers
{
    public class PostDailyCashRequestDto
    {
        public string UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
