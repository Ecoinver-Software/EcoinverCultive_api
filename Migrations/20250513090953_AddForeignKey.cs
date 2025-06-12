using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoinverGMAO_api.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ControlStockDetails_IdGenero",
                table: "ControlStockDetails",
                column: "IdGenero");

            migrationBuilder.AddForeignKey(
                name: "FK_ControlStockDetails_Gender_IdGenero",
                table: "ControlStockDetails",
                column: "IdGenero",
                principalTable: "Gender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ControlStockDetails_Gender_IdGenero",
                table: "ControlStockDetails");

            migrationBuilder.DropIndex(
                name: "IX_ControlStockDetails_IdGenero",
                table: "ControlStockDetails");
        }
    }
}
