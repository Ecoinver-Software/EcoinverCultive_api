using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoinverGMAO_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGeneroForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

         

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Gender_IdGenero",
                table: "Gender",
                column: "IdGenero");

            migrationBuilder.CreateIndex(
                name: "IX_Gender_IdGenero",
                table: "Gender",
                column: "IdGenero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommercialNeeds_GeneroId",
                table: "CommercialNeeds",
                column: "GeneroId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommercialNeeds_Gender_GeneroId",
                table: "CommercialNeeds",
                column: "GeneroId",
                principalTable: "Gender",
                principalColumn: "IdGenero",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommercialNeeds_Gender_GeneroId",
                table: "CommercialNeeds");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Gender_IdGenero",
                table: "Gender");

            migrationBuilder.DropIndex(
                name: "IX_Gender_IdGenero",
                table: "Gender");

            migrationBuilder.DropIndex(
                name: "IX_CommercialNeeds_GeneroId",
                table: "CommercialNeeds");

            migrationBuilder.DropColumn(
                name: "GeneroId",
                table: "CommercialNeeds");

            migrationBuilder.DropColumn(
                name: "GeneroNombre",
                table: "CommercialNeeds");
        }
    }
}
