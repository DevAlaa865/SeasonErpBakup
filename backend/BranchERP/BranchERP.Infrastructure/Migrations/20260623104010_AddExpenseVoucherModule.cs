using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BranchERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseVoucherModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DepositCollectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositCollectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositCollectors_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepositCollectors_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PettyHolders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PettyHolders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PettyHolders_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PettyHolders_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

         

            migrationBuilder.CreateTable(
                name: "CashBoxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepositCollectorId = table.Column<int>(type: "int", nullable: true),
                    PettyHolderId = table.Column<int>(type: "int", nullable: true),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashBoxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashBoxes_DepositCollectors_DepositCollectorId",
                        column: x => x.DepositCollectorId,
                        principalTable: "DepositCollectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashBoxes_PettyHolders_PettyHolderId",
                        column: x => x.PettyHolderId,
                        principalTable: "PettyHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseVouchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoucherDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CashBoxId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ApprovedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseVouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseVouchers_CashBoxes_CashBoxId",
                        column: x => x.CashBoxId,
                        principalTable: "CashBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CashBoxTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashBoxId = table.Column<int>(type: "int", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ExpenseVoucherId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    PettyHolderId = table.Column<int>(type: "int", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashBoxTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashBoxTransactions_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashBoxTransactions_CashBoxes_CashBoxId",
                        column: x => x.CashBoxId,
                        principalTable: "CashBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CashBoxTransactions_ExpenseVouchers_ExpenseVoucherId",
                        column: x => x.ExpenseVoucherId,
                        principalTable: "ExpenseVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashBoxTransactions_PettyHolders_PettyHolderId",
                        column: x => x.PettyHolderId,
                        principalTable: "PettyHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseVoucherLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseVoucherId = table.Column<int>(type: "int", nullable: false),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    ExpenseTypeId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    PettyHolderId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseVoucherLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseVoucherLines_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseVoucherLines_ExpenseTypes_ExpenseTypeId",
                        column: x => x.ExpenseTypeId,
                        principalTable: "ExpenseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseVoucherLines_ExpenseVouchers_ExpenseVoucherId",
                        column: x => x.ExpenseVoucherId,
                        principalTable: "ExpenseVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpenseVoucherLines_PettyHolders_PettyHolderId",
                        column: x => x.PettyHolderId,
                        principalTable: "PettyHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashBoxes_DepositCollectorId",
                table: "CashBoxes",
                column: "DepositCollectorId");

            migrationBuilder.CreateIndex(
                name: "IX_CashBoxes_PettyHolderId",
                table: "CashBoxes",
                column: "PettyHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_CashBoxTransactions_BranchId",
                table: "CashBoxTransactions",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CashBoxTransactions_CashBoxId",
                table: "CashBoxTransactions",
                column: "CashBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_CashBoxTransactions_ExpenseVoucherId",
                table: "CashBoxTransactions",
                column: "ExpenseVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_CashBoxTransactions_PettyHolderId",
                table: "CashBoxTransactions",
                column: "PettyHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositCollectors_CityId",
                table: "DepositCollectors",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositCollectors_RegionId",
                table: "DepositCollectors",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseVoucherLines_BranchId",
                table: "ExpenseVoucherLines",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseVoucherLines_ExpenseTypeId",
                table: "ExpenseVoucherLines",
                column: "ExpenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseVoucherLines_ExpenseVoucherId",
                table: "ExpenseVoucherLines",
                column: "ExpenseVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseVoucherLines_PettyHolderId",
                table: "ExpenseVoucherLines",
                column: "PettyHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseVouchers_CashBoxId",
                table: "ExpenseVouchers",
                column: "CashBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_PettyHolders_CityId",
                table: "PettyHolders",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_PettyHolders_RegionId",
                table: "PettyHolders",
                column: "RegionId");

       
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashBoxTransactions");

            migrationBuilder.DropTable(
                name: "ExpenseVoucherLines");

           
            migrationBuilder.DropTable(
                name: "ExpenseTypes");

            migrationBuilder.DropTable(
                name: "ExpenseVouchers");

            migrationBuilder.DropTable(
                name: "CashBoxes");

            migrationBuilder.DropTable(
                name: "DepositCollectors");

            migrationBuilder.DropTable(
                name: "PettyHolders");
        }
    }
}
