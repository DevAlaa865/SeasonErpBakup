using BranchERP.Domain.Entities;
using BranchERP.Domain.Enums;

public class BranchDailyTargetDetail : BaseEntity
{
    public int HeaderId { get; set; }
    public BranchDailyTargetHeader Header { get; set; } = null!;

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public WorkShift Shift { get; set; }  // أو Enum لو عندك

    public decimal? EmployeeTarget { get; set; } = 0;
    public decimal? EmployeeAchieved { get; set; } = 0;
    public decimal? EmployeeAchievementPercentage { get; set; } = 0;
    public decimal? EmployeeCommission { get; set; } = 0;
}
