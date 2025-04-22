using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoinverGMAO_api.Migrations
{
    /// <inheritdoc />
    public partial class AddKilosAjustadosAndCultiveFKKK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CultiveId",
                table: "CultivesProduction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "KilosAjustados",
                table: "CultivesProduction",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CultivesProduction_CultiveId",
                table: "CultivesProduction",
                column: "CultiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_CultivesProduction_Cultives_CultiveId",
                table: "CultivesProduction",
                column: "CultiveId",
                principalTable: "Cultives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CultivesProduction_Cultives_CultiveId",
                table: "CultivesProduction");

            migrationBuilder.DropIndex(
                name: "IX_CultivesProduction_CultiveId",
                table: "CultivesProduction");

            migrationBuilder.DropColumn(
                name: "CultiveId",
                table: "CultivesProduction");

            migrationBuilder.DropColumn(
                name: "KilosAjustados",
                table: "CultivesProduction");
        }
    }
}
