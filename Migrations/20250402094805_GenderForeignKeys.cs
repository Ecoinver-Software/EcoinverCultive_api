using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoinverGMAO_api.Migrations
{
    /// <inheritdoc />
    public partial class GenderForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdGenero",
                table: "CommercialNeeds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CommercialNeeds_IdGenero",
                table: "CommercialNeeds",
                column: "IdGenero");

            migrationBuilder.AddForeignKey(
                name: "FK_CommercialNeeds_Gender_IdGenero",
                table: "CommercialNeeds",
                column: "IdGenero",
                principalTable: "Gender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommercialNeeds_Gender_IdGenero",
                table: "CommercialNeeds");

            migrationBuilder.DropIndex(
                name: "IX_CommercialNeeds_IdGenero",
                table: "CommercialNeeds");

            migrationBuilder.DropColumn(
                name: "IdGenero",
                table: "CommercialNeeds");
        }
    }
}
