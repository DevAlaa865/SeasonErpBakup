using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BranchERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddManagerApprovalFieldsToBranchControlIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsManagerApproved",
                table: "BranchControlIssues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ManagerNotes",
                table: "BranchControlIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerSignature",
                table: "BranchControlIssues",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsManagerApproved",
                table: "BranchControlIssues");

            migrationBuilder.DropColumn(
                name: "ManagerNotes",
                table: "BranchControlIssues");

            migrationBuilder.DropColumn(
                name: "ManagerSignature",
                table: "BranchControlIssues");
        }
    }
}
