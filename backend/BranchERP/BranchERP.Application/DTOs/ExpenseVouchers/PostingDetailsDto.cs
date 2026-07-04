using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers
{
    public class PostingDetailsDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }        // SalesDate
        public DateTime PostedAt { get; set; }    // CreatedAt (تاريخ الترحيل الفعلي)
        public decimal Amount { get; set; }
        public string CashBoxName { get; set; }
        public string CollectorName { get; set; }
        public string BranchName { get; set; }
        public string CityName { get; set; }
        public string Description { get; set; }
        public string Direction { get; set; }
        public string Type { get; set; }
    }
}
