using BranchERP.Domain.Entities.Enums;

namespace BranchERP.Application.DTOs.Reports
{
    public class ReturnsDiscountsManagementFilterDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int? CityId { get; set; }   // أو CityId لو ده اللي عندك
        public int? BranchId { get; set; }

        // ✅ الحالة (الكل / معتمد / غير معتمد)
        public ReturnsDiscountsApprovalStatus Status { get; set; } = ReturnsDiscountsApprovalStatus.All;

        // ✅ نوع العجز (مرتجعات، خصم على فاتورة، خصم موظف، مكافأة…)
        public int? ShortageTypeId { get; set; }
    }
}
