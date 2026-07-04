using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BranchERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_DepositCollector_MultiCities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepositCollectors_Cities_CityId",
                table: "DepositCollectors");

            migrationBuilder.DropIndex(
                name: "IX_DepositCollectors_CityId",
                table: "DepositCollectors");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "DepositCollectors");

            migrationBuilder.CreateTable(
                name: "DepositCollectorCity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepositCollectorId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositCollectorCity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositCollectorCity_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepositCollectorCity_DepositCollectors_DepositCollectorId",
                        column: x => x.DepositCollectorId,
                        principalTable: "DepositCollectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepositCollectorCity_CityId",
                table: "DepositCollectorCity",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositCollectorCity_DepositCollectorId",
                table: "DepositCollectorCity",
                column: "DepositCollectorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepositCollectorCity");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "DepositCollectors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DepositCollectors_CityId",
                table: "DepositCollectors",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepositCollectors_Cities_CityId",
                table: "DepositCollectors",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
