using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoinverGMAO_api.Migrations
{
    /// <inheritdoc />
    public partial class LinkPlanningToGender_OneToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdGenero",
                table: "CultivesPlanning",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CultivesPlanning_IdGenero",
                table: "CultivesPlanning",
                column: "IdGenero",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CultivesPlanning_Gender_IdGenero",
                table: "CultivesPlanning",
                column: "IdGenero",
                principalTable: "Gender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CultivesPlanning_Gender_IdGenero",
                table: "CultivesPlanning");

            migrationBuilder.DropIndex(
                name: "IX_CultivesPlanning_IdGenero",
                table: "CultivesPlanning");

            migrationBuilder.DropColumn(
                name: "IdGenero",
                table: "CultivesPlanning");
        }
    }
}
