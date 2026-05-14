using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BranchERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTargetSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BranchDailyPerformances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    TargetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BranchTargetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BranchAchievedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BranchInvoicesCountTarget = table.Column<int>(type: "int", nullable: true),
                    BranchInvoicesCountAchieved = table.Column<int>(type: "int", nullable: true),
                    BranchItemsCountTarget = table.Column<int>(type: "int", nullable: true),
                    BranchItemsCountAchieved = table.Column<int>(type: "int", nullable: true),
                    AchievementPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchDailyPerformances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchDailyPerformances_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommissionRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FixedBonusAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentageBonus = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommissionRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeShiftTargetHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    TargetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Shift = table.Column<int>(type: "int", nullable: false),
                    TotalPersonalTargetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeShiftTargetHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeShiftTargetHeaders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePersonalTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftHeaderId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PersonalTargetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePersonalTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeePersonalTargets_EmployeeShiftTargetHeaders_ShiftHeaderId",
                        column: x => x.ShiftHeaderId,
                        principalTable: "EmployeeShiftTargetHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeePersonalTargets_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePersonalAchievements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeePersonalTargetId = table.Column<int>(type: "int", nullable: false),
                    AchievedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoicesCount = table.Column<int>(type: "int", nullable: true),
                    ItemsCount = table.Column<int>(type: "int", nullable: true),
                    AchievementPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsTargetAchieved = table.Column<bool>(type: "bit", nullable: false),
                    AttachmentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommissionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EnteredBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnteredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePersonalAchievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeePersonalAchievements_EmployeePersonalTargets_EmployeePersonalTargetId",
                        column: x => x.EmployeePersonalTargetId,
                        principalTable: "EmployeePersonalTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchDailyPerformances_BranchId",
                table: "BranchDailyPerformances",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePersonalAchievements_EmployeePersonalTargetId",
                table: "EmployeePersonalAchievements",
                column: "EmployeePersonalTargetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePersonalTargets_EmployeeId",
                table: "EmployeePersonalTargets",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePersonalTargets_ShiftHeaderId",
                table: "EmployeePersonalTargets",
                column: "ShiftHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftTargetHeaders_BranchId",
                table: "EmployeeShiftTargetHeaders",
                column: "BranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchDailyPerformances");

            migrationBuilder.DropTable(
                name: "CommissionRules");

            migrationBuilder.DropTable(
                name: "EmployeePersonalAchievements");

            migrationBuilder.DropTable(
                name: "EmployeePersonalTargets");

            migrationBuilder.DropTable(
                name: "EmployeeShiftTargetHeaders");
        }
    }
}
