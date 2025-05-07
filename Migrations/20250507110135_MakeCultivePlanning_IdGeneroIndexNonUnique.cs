using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoinverGMAO_api.Migrations
{
    public partial class MakeCultivePlanning_IdGeneroIndexNonUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Suelta la FK que apuntaba a ese índice
            migrationBuilder.DropForeignKey(
                name: "FK_CultivesPlanning_Gender_IdGenero",
                table: "CultivesPlanning");

            // 2) Elimina el índice único
            migrationBuilder.DropIndex(
                name: "IX_CultivesPlanning_IdGenero",
                table: "CultivesPlanning");

            // 3) Recréalo sin UNIQUE
            migrationBuilder.CreateIndex(
                name: "IX_CultivesPlanning_IdGenero",
                table: "CultivesPlanning",
                column: "IdGenero");

            // 4) Vuelve a añadir la FK (ahora el índice ya no es único)
            migrationBuilder.AddForeignKey(
                name: "FK_CultivesPlanning_Gender_IdGenero",
                table: "CultivesPlanning",
                column: "IdGenero",
                principalTable: "Gender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Inversa: suelta la FK
            migrationBuilder.DropForeignKey(
                name: "FK_CultivesPlanning_Gender_IdGenero",
                table: "CultivesPlanning");

            // Suelta el índice no‐único
            migrationBuilder.DropIndex(
                name: "IX_CultivesPlanning_IdGenero",
                table: "CultivesPlanning");

            // Recrea el índice como UNIQUE
            migrationBuilder.CreateIndex(
                name: "IX_CultivesPlanning_IdGenero",
                table: "CultivesPlanning",
                column: "IdGenero",
                unique: true);

            // Vuelve a añadir la FK original
            migrationBuilder.AddForeignKey(
                name: "FK_CultivesPlanning_Gender_IdGenero",
                table: "CultivesPlanning",
                column: "IdGenero",
                principalTable: "Gender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
