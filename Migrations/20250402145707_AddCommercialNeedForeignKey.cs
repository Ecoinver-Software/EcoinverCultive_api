using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoinverGMAO_api.Migrations
{
    /// <inheritdoc />
    public partial class AddCommercialNeedForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CommercialNeedsPlanning_IdCommercialNeed",
                table: "CommercialNeedsPlanning",
                column: "IdCommercialNeed");

            migrationBuilder.AddForeignKey(
                name: "FK_CommercialNeedsPlanning_CommercialNeeds_IdCommercialNeed",
                table: "CommercialNeedsPlanning",
                column: "IdCommercialNeed",
                principalTable: "CommercialNeeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommercialNeedsPlanning_CommercialNeeds_IdCommercialNeed",
                table: "CommercialNeedsPlanning");

            migrationBuilder.DropIndex(
                name: "IX_CommercialNeedsPlanning_IdCommercialNeed",
                table: "CommercialNeedsPlanning");
        }
    }
}
