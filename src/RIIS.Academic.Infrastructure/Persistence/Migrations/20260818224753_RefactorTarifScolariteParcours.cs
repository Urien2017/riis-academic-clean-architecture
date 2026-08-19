using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RIIS.Academic.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefactorTarifScolariteParcours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TarifsScolarite_TypeElementScolariteId_AnneeAcademiqueCode_CycleCode_NiveauNumero_FiliereCode_SpecialiteCode",
                table: "TarifsScolarite");

            migrationBuilder.DropColumn(
                name: "AnneeAcademiqueCode",
                table: "TarifsScolarite");

            migrationBuilder.DropColumn(
                name: "CycleCode",
                table: "TarifsScolarite");

            migrationBuilder.DropColumn(
                name: "FiliereCode",
                table: "TarifsScolarite");

            migrationBuilder.DropColumn(
                name: "NiveauNumero",
                table: "TarifsScolarite");

            migrationBuilder.DropColumn(
                name: "SpecialiteCode",
                table: "TarifsScolarite");

            migrationBuilder.AddColumn<long>(
                name: "ParcoursAcademiqueId",
                table: "TarifsScolarite",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_TarifsScolarite_ParcoursAcademiqueId",
                table: "TarifsScolarite",
                column: "ParcoursAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_TarifsScolarite_TypeElementScolariteId_ParcoursAcademiqueId",
                table: "TarifsScolarite",
                columns: new[] { "TypeElementScolariteId", "ParcoursAcademiqueId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TarifsScolarite_ParcoursAcademiques_ParcoursAcademiqueId",
                table: "TarifsScolarite",
                column: "ParcoursAcademiqueId",
                principalTable: "ParcoursAcademiques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TarifsScolarite_ParcoursAcademiques_ParcoursAcademiqueId",
                table: "TarifsScolarite");

            migrationBuilder.DropIndex(
                name: "IX_TarifsScolarite_ParcoursAcademiqueId",
                table: "TarifsScolarite");

            migrationBuilder.DropIndex(
                name: "IX_TarifsScolarite_TypeElementScolariteId_ParcoursAcademiqueId",
                table: "TarifsScolarite");

            migrationBuilder.DropColumn(
                name: "ParcoursAcademiqueId",
                table: "TarifsScolarite");

            migrationBuilder.AddColumn<string>(
                name: "AnneeAcademiqueCode",
                table: "TarifsScolarite",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CycleCode",
                table: "TarifsScolarite",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FiliereCode",
                table: "TarifsScolarite",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NiveauNumero",
                table: "TarifsScolarite",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialiteCode",
                table: "TarifsScolarite",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TarifsScolarite_TypeElementScolariteId_AnneeAcademiqueCode_CycleCode_NiveauNumero_FiliereCode_SpecialiteCode",
                table: "TarifsScolarite",
                columns: new[] { "TypeElementScolariteId", "AnneeAcademiqueCode", "CycleCode", "NiveauNumero", "FiliereCode", "SpecialiteCode" });
        }
    }
}
