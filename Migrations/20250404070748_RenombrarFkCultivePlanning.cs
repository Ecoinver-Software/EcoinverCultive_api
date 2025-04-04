using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoinverGMAO_api.Migrations
{
    /// <inheritdoc />
    public partial class RenombrarFkCultivePlanning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cultives_CultivesPlanning_CultivePlanningId",
                table: "Cultives");

            migrationBuilder.DropIndex(
                name: "IX_Cultives_CultivePlanningId",
                table: "Cultives");

            migrationBuilder.DropColumn(
                name: "CultivePlanningId",
                table: "Cultives");

            migrationBuilder.CreateIndex(
                name: "IX_Cultives_IdCultivePlanning",
                table: "Cultives",
                column: "IdCultivePlanning");

            migrationBuilder.AddForeignKey(
                name: "FK_Cultives_CultivesPlanning_IdCultivePlanning",
                table: "Cultives",
                column: "IdCultivePlanning",
                principalTable: "CultivesPlanning",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cultives_CultivesPlanning_IdCultivePlanning",
                table: "Cultives");

            migrationBuilder.DropIndex(
                name: "IX_Cultives_IdCultivePlanning",
                table: "Cultives");

            migrationBuilder.AddColumn<int>(
                name: "CultivePlanningId",
                table: "Cultives",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cultives_CultivePlanningId",
                table: "Cultives",
                column: "CultivePlanningId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cultives_CultivesPlanning_CultivePlanningId",
                table: "Cultives",
                column: "CultivePlanningId",
                principalTable: "CultivesPlanning",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
