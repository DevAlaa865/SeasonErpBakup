using BranchERP.Domain.Entities;
using BranchERP.Domain.Entities.BranchERP.Domain.Entities;
using BranchERP.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BranchERP.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }

        // Shortage Types
        public DbSet<ShortageType> ShortageTypes { get; set; }

        // Branch Sales Daily
        public DbSet<BranchSalesDaily> BranchSalesDailies { get; set; }
        public DbSet<BranchSalesShortageDetail> BranchSalesShortageDetails { get; set; }

        // Branch Daily Target
        public DbSet<BranchDailyTargetHeader> BranchDailyTargetHeaders { get; set; }
        public DbSet<BranchDailyTargetDetail> BranchDailyTargetDetails { get; set; }

        // Permissions / Roles / Departments
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<UserDepartment> UserDepartments { get; set; }
        public DbSet<BranchDailyPerformance> BranchDailyPerformances { get; set; }
        public DbSet<EmployeeShiftTargetHeader> EmployeeShiftTargetHeaders { get; set; }
        public DbSet<EmployeePersonalTarget> EmployeePersonalTargets { get; set; }
        public DbSet<EmployeePersonalAchievement> EmployeePersonalAchievements { get; set; }
        public DbSet<CommissionRule> CommissionRules { get; set; }
        public DbSet<BranchDailyReturn> BranchDailyReturns { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================
            // Branch ↔ Supervisor (Employee)
            // ============================
            modelBuilder.Entity<Branch>()
                .HasOne(b => b.Supervisor)
                .WithMany()
                .HasForeignKey(b => b.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ============================
            // Branch ↔ City
            // ============================
            modelBuilder.Entity<Branch>()
                .HasOne(b => b.City)
                .WithMany()
                .HasForeignKey(b => b.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            // ============================
            // Branch ↔ ActivityType
            // ============================
            modelBuilder.Entity<Branch>()
                .HasOne(b => b.ActivityType)
                .WithMany()
                .HasForeignKey(b => b.ActivityTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // ============================
            // RolePermission
            // ============================
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne<ApplicationRole>()
                .WithMany()
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany()
                .HasForeignKey(rp => rp.PermissionId);

            // ============================
            // UserDepartment
            // ============================
            modelBuilder.Entity<UserDepartment>()
                .HasKey(ud => new { ud.UserId, ud.DepartmentId });

            modelBuilder.Entity<UserDepartment>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(ud => ud.UserId);

            modelBuilder.Entity<UserDepartment>()
                .HasOne(ud => ud.Department)
                .WithMany()
                .HasForeignKey(ud => ud.DepartmentId);

            // ============================
            // BranchDailyTarget
            // ============================

            // Header → Branch
            modelBuilder.Entity<BranchDailyTargetHeader>()
                .HasOne(h => h.Branch)
                .WithMany()
                .HasForeignKey(h => h.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Detail → Header
            modelBuilder.Entity<BranchDailyTargetDetail>()
                .HasOne(d => d.Header)
                .WithMany(h => h.Details)
                .HasForeignKey(d => d.HeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Detail → Employee
            modelBuilder.Entity<BranchDailyTargetDetail>()
                .HasOne(d => d.Employee)
                .WithMany()
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // ============================
            // BranchSalesDaily
            // ============================

            // Daily → Branch
            modelBuilder.Entity<BranchSalesDaily>()
                .HasOne(d => d.Branch)
                .WithMany()
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Daily → Supervisor (Employee)
            modelBuilder.Entity<BranchSalesDaily>()
                .HasOne(d => d.Supervisor)
                .WithMany()
                .HasForeignKey(d => d.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);
          
            modelBuilder.Entity<BranchSalesDaily>()
            .HasIndex(d => new { d.BranchId, d.SalesDate })
            .IsUnique();
            // ============================
            // BranchSalesShortageDetail
            // ============================

            // ShortageDetail → BranchSalesDaily
            modelBuilder.Entity<BranchSalesShortageDetail>()
                .HasOne(s => s.BranchSalesDaily)
                .WithMany(d => d.ShortageDetails)
                .HasForeignKey(s => s.BranchSalesDailyId)
                .OnDelete(DeleteBehavior.Cascade);

            // ShortageDetail → ShortageType
            modelBuilder.Entity<BranchSalesShortageDetail>()
                .HasOne(s => s.ShortageType)
                .WithMany()
                .HasForeignKey(s => s.ShortageTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔥 ShortageDetail → Employee (اختياري)
            modelBuilder.Entity<BranchSalesShortageDetail>()
                .HasOne(s => s.Employee)
                .WithMany()
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // BranchDailyPerformance → Branch
            modelBuilder.Entity<BranchDailyPerformance>()
                .HasOne(b => b.Branch)
                .WithMany()
                .HasForeignKey(b => b.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // EmployeeShiftTargetHeader → Branch
            modelBuilder.Entity<EmployeeShiftTargetHeader>()
                .HasOne(h => h.Branch)
                .WithMany()
                .HasForeignKey(h => h.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // EmployeePersonalTarget → ShiftHeader
            modelBuilder.Entity<EmployeePersonalTarget>()
                .HasOne(t => t.ShiftHeader)
                .WithMany(h => h.PersonalTargets)
                .HasForeignKey(t => t.ShiftHeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            // EmployeePersonalTarget → Employee
            modelBuilder.Entity<EmployeePersonalTarget>()
                .HasOne(t => t.Employee)
                .WithMany()
                .HasForeignKey(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // EmployeePersonalAchievement → EmployeePersonalTarget
            modelBuilder.Entity<EmployeePersonalAchievement>()
                .HasOne(a => a.EmployeePersonalTarget)
                .WithOne(t => t.Achievement)
                .HasForeignKey<EmployeePersonalAchievement>(a => a.EmployeePersonalTargetId)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================
            // BranchDailyReturn
            // ============================
            modelBuilder.Entity<BranchDailyReturn>()
                .ToTable("BranchDailyReturns");

            modelBuilder.Entity<BranchDailyReturn>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<BranchDailyReturn>()
                .Property(r => r.ReturnAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            modelBuilder.Entity<BranchDailyReturn>()
                .Property(r => r.ReturnType)
                .IsRequired();

            modelBuilder.Entity<BranchDailyReturn>()
                .Property(r => r.Notes)
                .HasMaxLength(500);

            modelBuilder.Entity<BranchDailyReturn>()
                .HasOne(r => r.Branch)
                .WithMany()
                .HasForeignKey(r => r.BranchId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
