using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RIIS.Academic.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefactorClassePedagogique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassesPedagogiques_AnneesAcademiques_AnneeAcademiqueId",
                table: "ClassesPedagogiques");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassesPedagogiques_MaquettesPedagogiques_MaquettePedagogiqueId",
                table: "ClassesPedagogiques");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassesPedagogiques_NiveauxEtude_NiveauEtudeId",
                table: "ClassesPedagogiques");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassesPedagogiques_ParcoursAcademiques_ParcoursAcademiqueId",
                table: "ClassesPedagogiques");

            migrationBuilder.DropIndex(
                name: "IX_ClassesPedagogiques_AnneeAcademiqueId_ParcoursAcademiqueId_NiveauEtudeId_Code",
                table: "ClassesPedagogiques");

            migrationBuilder.DropIndex(
                name: "IX_ClassesPedagogiques_MaquettePedagogiqueId",
                table: "ClassesPedagogiques");

            migrationBuilder.DropIndex(
                name: "IX_ClassesPedagogiques_NiveauEtudeId",
                table: "ClassesPedagogiques");

            migrationBuilder.DropIndex(
                name: "IX_ClassesPedagogiques_ParcoursAcademiqueId",
                table: "ClassesPedagogiques");

            migrationBuilder.DropColumn(
                name: "AnneeAcademiqueId",
                table: "ClassesPedagogiques");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "ClassesPedagogiques");

            migrationBuilder.DropColumn(
                name: "EstActive",
                table: "ClassesPedagogiques");

            migrationBuilder.DropColumn(
                name: "MaquettePedagogiqueId",
                table: "ClassesPedagogiques");

            migrationBuilder.DropColumn(
                name: "NiveauEtudeId",
                table: "ClassesPedagogiques");

            migrationBuilder.AddColumn<int>(
                name: "Effectif",
                table: "ClassesPedagogiques",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClassesPedagogiques_ParcoursAcademiqueId_Libelle",
                table: "ClassesPedagogiques",
                columns: new[] { "ParcoursAcademiqueId", "Libelle" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassesPedagogiques_ParcoursAcademiqueId_Libelle",
                table: "ClassesPedagogiques");

            migrationBuilder.DropColumn(
                name: "Effectif",
                table: "ClassesPedagogiques");

            migrationBuilder.AddColumn<long>(
                name: "AnneeAcademiqueId",
                table: "ClassesPedagogiques",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "ClassesPedagogiques",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EstActive",
                table: "ClassesPedagogiques",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "MaquettePedagogiqueId",
                table: "ClassesPedagogiques",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NiveauEtudeId",
                table: "ClassesPedagogiques",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ClassesPedagogiques_AnneeAcademiqueId_ParcoursAcademiqueId_NiveauEtudeId_Code",
                table: "ClassesPedagogiques",
                columns: new[] { "AnneeAcademiqueId", "ParcoursAcademiqueId", "NiveauEtudeId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassesPedagogiques_MaquettePedagogiqueId",
                table: "ClassesPedagogiques",
                column: "MaquettePedagogiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassesPedagogiques_NiveauEtudeId",
                table: "ClassesPedagogiques",
                column: "NiveauEtudeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassesPedagogiques_ParcoursAcademiqueId",
                table: "ClassesPedagogiques",
                column: "ParcoursAcademiqueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassesPedagogiques_AnneesAcademiques_AnneeAcademiqueId",
                table: "ClassesPedagogiques",
                column: "AnneeAcademiqueId",
                principalTable: "AnneesAcademiques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassesPedagogiques_MaquettesPedagogiques_MaquettePedagogiqueId",
                table: "ClassesPedagogiques",
                column: "MaquettePedagogiqueId",
                principalTable: "MaquettesPedagogiques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassesPedagogiques_NiveauxEtude_NiveauEtudeId",
                table: "ClassesPedagogiques",
                column: "NiveauEtudeId",
                principalTable: "NiveauxEtude",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassesPedagogiques_ParcoursAcademiques_ParcoursAcademiqueId",
                table: "ClassesPedagogiques",
                column: "ParcoursAcademiqueId",
                principalTable: "ParcoursAcademiques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
