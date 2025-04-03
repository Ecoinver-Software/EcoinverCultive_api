using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoinverGMAO_api.Migrations
{
    /// <inheritdoc />
    public partial class Plannings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CultivePlanningId",
                table: "Cultives",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdCultivePlanning",
                table: "Cultives",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CommercialNeedsPlanningDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdCommercialNeedsPlanning = table.Column<int>(type: "int", nullable: false),
                    Kilos = table.Column<double>(type: "double", nullable: true),
                    FechaDesde = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaHasta = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NumeroSemana = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommercialNeedsPlanningDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommercialNeedsPlanningDetails_CommercialNeedsPlanning_IdCom~",
                        column: x => x.IdCommercialNeedsPlanning,
                        principalTable: "CommercialNeedsPlanning",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CultivesPlanning",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaInicio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CultivesPlanning", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CultivesPlanningDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaInicio = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Kilos = table.Column<double>(type: "double", nullable: true),
                    Tramo = table.Column<int>(type: "int", nullable: false),
                    IdCultivePlanning = table.Column<int>(type: "int", nullable: false),
                    CultivePlanningId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CultivesPlanningDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CultivesPlanningDetails_CultivesPlanning_CultivePlanningId",
                        column: x => x.CultivePlanningId,
                        principalTable: "CultivesPlanning",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Cultives_CultivePlanningId",
                table: "Cultives",
                column: "CultivePlanningId");

            migrationBuilder.CreateIndex(
                name: "IX_CommercialNeedsPlanningDetails_IdCommercialNeedsPlanning",
                table: "CommercialNeedsPlanningDetails",
                column: "IdCommercialNeedsPlanning");

            migrationBuilder.CreateIndex(
                name: "IX_CultivesPlanningDetails_CultivePlanningId",
                table: "CultivesPlanningDetails",
                column: "CultivePlanningId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cultives_CultivesPlanning_CultivePlanningId",
                table: "Cultives",
                column: "CultivePlanningId",
                principalTable: "CultivesPlanning",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cultives_CultivesPlanning_CultivePlanningId",
                table: "Cultives");

            migrationBuilder.DropTable(
                name: "CommercialNeedsPlanningDetails");

            migrationBuilder.DropTable(
                name: "CultivesPlanningDetails");

            migrationBuilder.DropTable(
                name: "CultivesPlanning");

            migrationBuilder.DropIndex(
                name: "IX_Cultives_CultivePlanningId",
                table: "Cultives");

            migrationBuilder.DropColumn(
                name: "CultivePlanningId",
                table: "Cultives");

            migrationBuilder.DropColumn(
                name: "IdCultivePlanning",
                table: "Cultives");
        }
    }
}
