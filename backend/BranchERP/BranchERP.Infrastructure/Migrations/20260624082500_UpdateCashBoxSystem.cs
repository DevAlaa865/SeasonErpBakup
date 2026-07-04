using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BranchERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCashBoxSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "CashBoxTransactions",
                newName: "TransactionType");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PettyHolders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DepositCollectors",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CashBoxTransactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "CashBoxTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PettyHolders_UserId",
                table: "PettyHolders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositCollectors_UserId",
                table: "DepositCollectors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepositCollectors_AspNetUsers_UserId",
                table: "DepositCollectors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PettyHolders_AspNetUsers_UserId",
                table: "PettyHolders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepositCollectors_AspNetUsers_UserId",
                table: "DepositCollectors");

            migrationBuilder.DropForeignKey(
                name: "FK_PettyHolders_AspNetUsers_UserId",
                table: "PettyHolders");

            migrationBuilder.DropIndex(
                name: "IX_PettyHolders_UserId",
                table: "PettyHolders");

            migrationBuilder.DropIndex(
                name: "IX_DepositCollectors_UserId",
                table: "DepositCollectors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PettyHolders");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "CashBoxTransactions");

            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "CashBoxTransactions",
                newName: "Type");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DepositCollectors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CashBoxTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
