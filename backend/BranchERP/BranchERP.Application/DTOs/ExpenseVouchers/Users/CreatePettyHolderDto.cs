using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchERP.Application.DTOs.ExpenseVouchers.Users
{
    public class CreatePettyHolderDto
    {
        public string Name { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        // 🔥 صاحب العهدة مسؤول عن أكتر من مدينة
        public List<int> CityIds { get; set; } = new();

        public int? RegionId { get; set; }
    }
}
