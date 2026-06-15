using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BranchERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCountryRegionCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_CountryId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Cities_CityId",
                table: "Regions");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "Regions",
                newName: "CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Regions_CityId",
                table: "Regions",
                newName: "IX_Regions_CountryId");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Cities",
                newName: "RegionId");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                newName: "IX_Cities_RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Regions_RegionId",
                table: "Cities",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Countries_CountryId",
                table: "Regions",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Regions_RegionId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Countries_CountryId",
                table: "Regions");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Regions",
                newName: "CityId");

            migrationBuilder.RenameIndex(
                name: "IX_Regions_CountryId",
                table: "Regions",
                newName: "IX_Regions_CityId");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                table: "Cities",
                newName: "CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_RegionId",
                table: "Cities",
                newName: "IX_Cities_CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_CountryId",
                table: "Cities",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Cities_CityId",
                table: "Regions",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
