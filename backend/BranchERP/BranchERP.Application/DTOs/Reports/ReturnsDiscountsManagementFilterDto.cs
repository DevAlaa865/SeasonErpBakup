using BranchERP.Domain.Entities.Enums;

namespace BranchERP.Application.DTOs.Reports
{
    public class ReturnsDiscountsManagementFilterDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        // 🔥 بدل CityId واحدة
        public List<int>? CityIds { get; set; }

        // 🔥 Multi Branches بدل BranchId
        public List<int>? BranchIds { get; set; }

        // ✅ الحالة
        public ReturnsDiscountsApprovalStatus Status { get; set; }
            = ReturnsDiscountsApprovalStatus.All;

        // ✅ نوع العجز
        public int? ShortageTypeId { get; set; }
    }
}