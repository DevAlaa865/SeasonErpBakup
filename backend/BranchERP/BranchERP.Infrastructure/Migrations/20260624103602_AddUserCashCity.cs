using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BranchERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserCashCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCashCities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    RoleType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCashCities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCashCities_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCashCities_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCashCities_CityId",
                table: "UserCashCities",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCashCities_UserId",
                table: "UserCashCities",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCashCities");
        }
    }
}
