using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BranchERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_PettyHolder_MultiCities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PettyHolders_Cities_CityId",
                table: "PettyHolders");

            migrationBuilder.DropIndex(
                name: "IX_PettyHolders_CityId",
                table: "PettyHolders");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "PettyHolders");

            migrationBuilder.CreateTable(
                name: "PettyHolderCity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PettyHolderId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PettyHolderCity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PettyHolderCity_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PettyHolderCity_PettyHolders_PettyHolderId",
                        column: x => x.PettyHolderId,
                        principalTable: "PettyHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PettyHolderCity_CityId",
                table: "PettyHolderCity",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_PettyHolderCity_PettyHolderId",
                table: "PettyHolderCity",
                column: "PettyHolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PettyHolderCity");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "PettyHolders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PettyHolders_CityId",
                table: "PettyHolders",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PettyHolders_Cities_CityId",
                table: "PettyHolders",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
