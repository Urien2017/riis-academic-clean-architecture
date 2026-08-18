using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RIIS.Academic.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRefonteQoder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationsAcademiques_ElementsConstitutifs_ElementConstitutifId",
                table: "EvaluationsAcademiques");

            migrationBuilder.DropForeignKey(
                name: "FK_MaquettesPedagogiques_CyclesFormation_CycleFormationId",
                table: "MaquettesPedagogiques");

            migrationBuilder.DropForeignKey(
                name: "FK_MaquettesPedagogiques_Filieres_FiliereId",
                table: "MaquettesPedagogiques");

            migrationBuilder.DropForeignKey(
                name: "FK_MaquettesPedagogiques_NiveauxEtude_NiveauEtudeId",
                table: "MaquettesPedagogiques");

            migrationBuilder.DropForeignKey(
                name: "FK_MaquettesPedagogiques_Specialites_SpecialiteId",
                table: "MaquettesPedagogiques");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultatsElementsConstitutifs_ElementsConstitutifs_ElementConstitutifId",
                table: "ResultatsElementsConstitutifs");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultatsUnitesEnseignement_UnitesEnseignement_UniteEnseignementId",
                table: "ResultatsUnitesEnseignement");

            migrationBuilder.DropForeignKey(
                name: "FK_SemestresPedagogiques_NiveauxEtude_NiveauEtudeId",
                table: "SemestresPedagogiques");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitesEnseignement_SemestresPedagogiques_SemestrePedagogiqueId",
                table: "UnitesEnseignement");

            migrationBuilder.DropIndex(
                name: "IX_UnitesEnseignement_SemestrePedagogiqueId_Code",
                table: "UnitesEnseignement");

            migrationBuilder.DropCheckConstraint(
                name: "CK_UnitesEnseignement_CreditsVolume",
                table: "UnitesEnseignement");

            migrationBuilder.DropIndex(
                name: "IX_SemestresPedagogiques_MaquettePedagogiqueId_Numero",
                table: "SemestresPedagogiques");

            migrationBuilder.DropCheckConstraint(
                name: "CK_SemestresPedagogiques_Numero",
                table: "SemestresPedagogiques");

            migrationBuilder.DropIndex(
                name: "IX_MaquettesPedagogiques_CycleFormationId_NiveauEtudeId_FiliereId_SpecialiteId_Code_Version",
                table: "MaquettesPedagogiques");

            migrationBuilder.DropIndex(
                name: "IX_MaquettesPedagogiques_FiliereId",
                table: "MaquettesPedagogiques");

            migrationBuilder.DropIndex(
                name: "IX_MaquettesPedagogiques_NiveauEtudeId",
                table: "MaquettesPedagogiques");

            migrationBuilder.DropIndex(
                name: "IX_MaquettesPedagogiques_SpecialiteId",
                table: "MaquettesPedagogiques");

            migrationBuilder.DropIndex(
                name: "IX_ElementsConstitutifs_UniteEnseignementId_OrdreAffichage",
                table: "ElementsConstitutifs");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ElementsConstitutifs_CreditsCoefVolume",
                table: "ElementsConstitutifs");

            migrationBuilder.DropColumn(
                name: "Credits",
                table: "UnitesEnseignement");

            migrationBuilder.DropColumn(
                name: "EstObligatoire",
                table: "UnitesEnseignement");

            migrationBuilder.DropColumn(
                name: "OrdreAffichage",
                table: "UnitesEnseignement");

            migrationBuilder.DropColumn(
                name: "SemestrePedagogiqueId",
                table: "UnitesEnseignement");

            migrationBuilder.DropColumn(
                name: "VolumeHoraire",
                table: "UnitesEnseignement");

            migrationBuilder.DropColumn(
                name: "CreditsAttendus",
                table: "SemestresPedagogiques");

            migrationBuilder.DropColumn(
                name: "VolumeHoraireAttendu",
                table: "SemestresPedagogiques");

            migrationBuilder.DropColumn(
                name: "CycleFormationId",
                table: "MaquettesPedagogiques");

            migrationBuilder.DropColumn(
                name: "FiliereId",
                table: "MaquettesPedagogiques");

            migrationBuilder.DropColumn(
                name: "NiveauEtudeId",
                table: "MaquettesPedagogiques");

            migrationBuilder.DropColumn(
                name: "Coefficient",
                table: "ElementsConstitutifs");

            migrationBuilder.DropColumn(
                name: "Credits",
                table: "ElementsConstitutifs");

            migrationBuilder.DropColumn(
                name: "EstObligatoire",
                table: "ElementsConstitutifs");

            migrationBuilder.DropColumn(
                name: "Observation",
                table: "ElementsConstitutifs");

            migrationBuilder.DropColumn(
                name: "OrdreAffichage",
                table: "ElementsConstitutifs");

            migrationBuilder.DropColumn(
                name: "VolumeHoraire",
                table: "ElementsConstitutifs");

            migrationBuilder.RenameColumn(
                name: "Numero",
                table: "SemestresPedagogiques",
                newName: "NumeroSemestre");

            migrationBuilder.RenameColumn(
                name: "UniteEnseignementId",
                table: "ResultatsUnitesEnseignement",
                newName: "SemestrePedagogiqueId");

            migrationBuilder.RenameIndex(
                name: "IX_ResultatsUnitesEnseignement_UniteEnseignementId",
                table: "ResultatsUnitesEnseignement",
                newName: "IX_ResultatsUnitesEnseignement_SemestrePedagogiqueId");

            migrationBuilder.RenameIndex(
                name: "IX_ResultatsUnitesEnseignement_InscriptionId_UniteEnseignementId",
                table: "ResultatsUnitesEnseignement",
                newName: "IX_ResultatsUnitesEnseignement_InscriptionId_SemestrePedagogiqueId");

            migrationBuilder.RenameColumn(
                name: "ElementConstitutifId",
                table: "ResultatsElementsConstitutifs",
                newName: "MaquetteElementConstitutifId");

            migrationBuilder.RenameIndex(
                name: "IX_ResultatsElementsConstitutifs_InscriptionId_ElementConstitutifId",
                table: "ResultatsElementsConstitutifs",
                newName: "IX_ResultatsElementsConstitutifs_InscriptionId_MaquetteElementConstitutifId");

            migrationBuilder.RenameIndex(
                name: "IX_ResultatsElementsConstitutifs_ElementConstitutifId",
                table: "ResultatsElementsConstitutifs",
                newName: "IX_ResultatsElementsConstitutifs_MaquetteElementConstitutifId");

            migrationBuilder.RenameColumn(
                name: "SpecialiteId",
                table: "MaquettesPedagogiques",
                newName: "ParcoursAcademiqueId");

            migrationBuilder.RenameColumn(
                name: "ElementConstitutifId",
                table: "EvaluationsAcademiques",
                newName: "MaquetteElementConstitutifId");

            migrationBuilder.RenameIndex(
                name: "IX_EvaluationsAcademiques_ElementConstitutifId",
                table: "EvaluationsAcademiques",
                newName: "IX_EvaluationsAcademiques_MaquetteElementConstitutifId");

            migrationBuilder.RenameIndex(
                name: "IX_EvaluationsAcademiques_AnneeAcademiqueId_ElementConstitutifId_Type_Numero",
                table: "EvaluationsAcademiques",
                newName: "IX_EvaluationsAcademiques_AnneeAcademiqueId_MaquetteElementConstitutifId_Type_Numero");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "UnitesEnseignement",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "UniteEnseignementId",
                table: "SemestresPedagogiques",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "MaquetteElementsConstitutifs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemestrePedagogiqueId = table.Column<long>(type: "bigint", nullable: false),
                    ElementConstitutifId = table.Column<long>(type: "bigint", nullable: false),
                    Credits = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Coefficient = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 1m),
                    VolumeHoraire = table.Column<short>(type: "smallint", nullable: false),
                    OrdreAffichage = table.Column<short>(type: "smallint", nullable: false),
                    EstObligatoire = table.Column<bool>(type: "bit", nullable: false),
                    Observation = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaquetteElementsConstitutifs", x => x.Id);
                    table.CheckConstraint("CK_MaquetteElementsConstitutifs_CreditsCoefVolume", "[Credits] >= 0 AND [Coefficient] >= 0 AND [VolumeHoraire] >= 0");
                    table.ForeignKey(
                        name: "FK_MaquetteElementsConstitutifs_ElementsConstitutifs_ElementConstitutifId",
                        column: x => x.ElementConstitutifId,
                        principalTable: "ElementsConstitutifs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaquetteElementsConstitutifs_SemestresPedagogiques_SemestrePedagogiqueId",
                        column: x => x.SemestrePedagogiqueId,
                        principalTable: "SemestresPedagogiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitesEnseignement_Code",
                table: "UnitesEnseignement",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SemestresPedagogiques_MaquettePedagogiqueId_NumeroSemestre_UniteEnseignementId",
                table: "SemestresPedagogiques",
                columns: new[] { "MaquettePedagogiqueId", "NumeroSemestre", "UniteEnseignementId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SemestresPedagogiques_UniteEnseignementId",
                table: "SemestresPedagogiques",
                column: "UniteEnseignementId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SemestresPedagogiques_NumeroSemestre",
                table: "SemestresPedagogiques",
                sql: "[NumeroSemestre] BETWEEN 1 AND 10");

            migrationBuilder.CreateIndex(
                name: "IX_MaquettesPedagogiques_ParcoursAcademiqueId_Code_Version",
                table: "MaquettesPedagogiques",
                columns: new[] { "ParcoursAcademiqueId", "Code", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaquetteElementsConstitutifs_ElementConstitutifId",
                table: "MaquetteElementsConstitutifs",
                column: "ElementConstitutifId");

            migrationBuilder.CreateIndex(
                name: "IX_MaquetteElementsConstitutifs_SemestrePedagogiqueId_ElementConstitutifId",
                table: "MaquetteElementsConstitutifs",
                columns: new[] { "SemestrePedagogiqueId", "ElementConstitutifId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationsAcademiques_MaquetteElementsConstitutifs_MaquetteElementConstitutifId",
                table: "EvaluationsAcademiques",
                column: "MaquetteElementConstitutifId",
                principalTable: "MaquetteElementsConstitutifs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaquettesPedagogiques_ParcoursAcademiques_ParcoursAcademiqueId",
                table: "MaquettesPedagogiques",
                column: "ParcoursAcademiqueId",
                principalTable: "ParcoursAcademiques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultatsElementsConstitutifs_MaquetteElementsConstitutifs_MaquetteElementConstitutifId",
                table: "ResultatsElementsConstitutifs",
                column: "MaquetteElementConstitutifId",
                principalTable: "MaquetteElementsConstitutifs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultatsUnitesEnseignement_SemestresPedagogiques_SemestrePedagogiqueId",
                table: "ResultatsUnitesEnseignement",
                column: "SemestrePedagogiqueId",
                principalTable: "SemestresPedagogiques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SemestresPedagogiques_NiveauxEtude_NiveauEtudeId",
                table: "SemestresPedagogiques",
                column: "NiveauEtudeId",
                principalTable: "NiveauxEtude",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SemestresPedagogiques_UnitesEnseignement_UniteEnseignementId",
                table: "SemestresPedagogiques",
                column: "UniteEnseignementId",
                principalTable: "UnitesEnseignement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationsAcademiques_MaquetteElementsConstitutifs_MaquetteElementConstitutifId",
                table: "EvaluationsAcademiques");

            migrationBuilder.DropForeignKey(
                name: "FK_MaquettesPedagogiques_ParcoursAcademiques_ParcoursAcademiqueId",
                table: "MaquettesPedagogiques");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultatsElementsConstitutifs_MaquetteElementsConstitutifs_MaquetteElementConstitutifId",
                table: "ResultatsElementsConstitutifs");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultatsUnitesEnseignement_SemestresPedagogiques_SemestrePedagogiqueId",
                table: "ResultatsUnitesEnseignement");

            migrationBuilder.DropForeignKey(
                name: "FK_SemestresPedagogiques_NiveauxEtude_NiveauEtudeId",
                table: "SemestresPedagogiques");

            migrationBuilder.DropForeignKey(
                name: "FK_SemestresPedagogiques_UnitesEnseignement_UniteEnseignementId",
                table: "SemestresPedagogiques");

            migrationBuilder.DropTable(
                name: "MaquetteElementsConstitutifs");

            migrationBuilder.DropIndex(
                name: "IX_UnitesEnseignement_Code",
                table: "UnitesEnseignement");

            migrationBuilder.DropIndex(
                name: "IX_SemestresPedagogiques_MaquettePedagogiqueId_NumeroSemestre_UniteEnseignementId",
                table: "SemestresPedagogiques");

            migrationBuilder.DropIndex(
                name: "IX_SemestresPedagogiques_UniteEnseignementId",
                table: "SemestresPedagogiques");

            migrationBuilder.DropCheckConstraint(
                name: "CK_SemestresPedagogiques_NumeroSemestre",
                table: "SemestresPedagogiques");

            migrationBuilder.DropIndex(
                name: "IX_MaquettesPedagogiques_ParcoursAcademiqueId_Code_Version",
                table: "MaquettesPedagogiques");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "UnitesEnseignement");

            migrationBuilder.DropColumn(
                name: "UniteEnseignementId",
                table: "SemestresPedagogiques");

            migrationBuilder.RenameColumn(
                name: "NumeroSemestre",
                table: "SemestresPedagogiques",
                newName: "Numero");

            migrationBuilder.RenameColumn(
                name: "SemestrePedagogiqueId",
                table: "ResultatsUnitesEnseignement",
                newName: "UniteEnseignementId");

            migrationBuilder.RenameIndex(
                name: "IX_ResultatsUnitesEnseignement_SemestrePedagogiqueId",
                table: "ResultatsUnitesEnseignement",
                newName: "IX_ResultatsUnitesEnseignement_UniteEnseignementId");

            migrationBuilder.RenameIndex(
                name: "IX_ResultatsUnitesEnseignement_InscriptionId_SemestrePedagogiqueId",
                table: "ResultatsUnitesEnseignement",
                newName: "IX_ResultatsUnitesEnseignement_InscriptionId_UniteEnseignementId");

            migrationBuilder.RenameColumn(
                name: "MaquetteElementConstitutifId",
                table: "ResultatsElementsConstitutifs",
                newName: "ElementConstitutifId");

            migrationBuilder.RenameIndex(
                name: "IX_ResultatsElementsConstitutifs_MaquetteElementConstitutifId",
                table: "ResultatsElementsConstitutifs",
                newName: "IX_ResultatsElementsConstitutifs_ElementConstitutifId");

            migrationBuilder.RenameIndex(
                name: "IX_ResultatsElementsConstitutifs_InscriptionId_MaquetteElementConstitutifId",
                table: "ResultatsElementsConstitutifs",
                newName: "IX_ResultatsElementsConstitutifs_InscriptionId_ElementConstitutifId");

            migrationBuilder.RenameColumn(
                name: "ParcoursAcademiqueId",
                table: "MaquettesPedagogiques",
                newName: "SpecialiteId");

            migrationBuilder.RenameColumn(
                name: "MaquetteElementConstitutifId",
                table: "EvaluationsAcademiques",
                newName: "ElementConstitutifId");

            migrationBuilder.RenameIndex(
                name: "IX_EvaluationsAcademiques_MaquetteElementConstitutifId",
                table: "EvaluationsAcademiques",
                newName: "IX_EvaluationsAcademiques_ElementConstitutifId");

            migrationBuilder.RenameIndex(
                name: "IX_EvaluationsAcademiques_AnneeAcademiqueId_MaquetteElementConstitutifId_Type_Numero",
                table: "EvaluationsAcademiques",
                newName: "IX_EvaluationsAcademiques_AnneeAcademiqueId_ElementConstitutifId_Type_Numero");

            migrationBuilder.AddColumn<decimal>(
                name: "Credits",
                table: "UnitesEnseignement",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "EstObligatoire",
                table: "UnitesEnseignement",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<short>(
                name: "OrdreAffichage",
                table: "UnitesEnseignement",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<long>(
                name: "SemestrePedagogiqueId",
                table: "UnitesEnseignement",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<short>(
                name: "VolumeHoraire",
                table: "UnitesEnseignement",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<decimal>(
                name: "CreditsAttendus",
                table: "SemestresPedagogiques",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<short>(
                name: "VolumeHoraireAttendu",
                table: "SemestresPedagogiques",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<long>(
                name: "CycleFormationId",
                table: "MaquettesPedagogiques",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "FiliereId",
                table: "MaquettesPedagogiques",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "NiveauEtudeId",
                table: "MaquettesPedagogiques",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "Coefficient",
                table: "ElementsConstitutifs",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 1m);

            migrationBuilder.AddColumn<decimal>(
                name: "Credits",
                table: "ElementsConstitutifs",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "EstObligatoire",
                table: "ElementsConstitutifs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Observation",
                table: "ElementsConstitutifs",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "OrdreAffichage",
                table: "ElementsConstitutifs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "VolumeHoraire",
                table: "ElementsConstitutifs",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_UnitesEnseignement_SemestrePedagogiqueId_Code",
                table: "UnitesEnseignement",
                columns: new[] { "SemestrePedagogiqueId", "Code" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_UnitesEnseignement_CreditsVolume",
                table: "UnitesEnseignement",
                sql: "[Credits] >= 0 AND [VolumeHoraire] >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_SemestresPedagogiques_MaquettePedagogiqueId_Numero",
                table: "SemestresPedagogiques",
                columns: new[] { "MaquettePedagogiqueId", "Numero" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_SemestresPedagogiques_Numero",
                table: "SemestresPedagogiques",
                sql: "[Numero] BETWEEN 1 AND 10");

            migrationBuilder.CreateIndex(
                name: "IX_MaquettesPedagogiques_CycleFormationId_NiveauEtudeId_FiliereId_SpecialiteId_Code_Version",
                table: "MaquettesPedagogiques",
                columns: new[] { "CycleFormationId", "NiveauEtudeId", "FiliereId", "SpecialiteId", "Code", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaquettesPedagogiques_FiliereId",
                table: "MaquettesPedagogiques",
                column: "FiliereId");

            migrationBuilder.CreateIndex(
                name: "IX_MaquettesPedagogiques_NiveauEtudeId",
                table: "MaquettesPedagogiques",
                column: "NiveauEtudeId");

            migrationBuilder.CreateIndex(
                name: "IX_MaquettesPedagogiques_SpecialiteId",
                table: "MaquettesPedagogiques",
                column: "SpecialiteId");

            migrationBuilder.CreateIndex(
                name: "IX_ElementsConstitutifs_UniteEnseignementId_OrdreAffichage",
                table: "ElementsConstitutifs",
                columns: new[] { "UniteEnseignementId", "OrdreAffichage" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_ElementsConstitutifs_CreditsCoefVolume",
                table: "ElementsConstitutifs",
                sql: "[Credits] >= 0 AND [Coefficient] >= 0 AND [VolumeHoraire] >= 0");

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationsAcademiques_ElementsConstitutifs_ElementConstitutifId",
                table: "EvaluationsAcademiques",
                column: "ElementConstitutifId",
                principalTable: "ElementsConstitutifs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaquettesPedagogiques_CyclesFormation_CycleFormationId",
                table: "MaquettesPedagogiques",
                column: "CycleFormationId",
                principalTable: "CyclesFormation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaquettesPedagogiques_Filieres_FiliereId",
                table: "MaquettesPedagogiques",
                column: "FiliereId",
                principalTable: "Filieres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaquettesPedagogiques_NiveauxEtude_NiveauEtudeId",
                table: "MaquettesPedagogiques",
                column: "NiveauEtudeId",
                principalTable: "NiveauxEtude",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaquettesPedagogiques_Specialites_SpecialiteId",
                table: "MaquettesPedagogiques",
                column: "SpecialiteId",
                principalTable: "Specialites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultatsElementsConstitutifs_ElementsConstitutifs_ElementConstitutifId",
                table: "ResultatsElementsConstitutifs",
                column: "ElementConstitutifId",
                principalTable: "ElementsConstitutifs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultatsUnitesEnseignement_UnitesEnseignement_UniteEnseignementId",
                table: "ResultatsUnitesEnseignement",
                column: "UniteEnseignementId",
                principalTable: "UnitesEnseignement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SemestresPedagogiques_NiveauxEtude_NiveauEtudeId",
                table: "SemestresPedagogiques",
                column: "NiveauEtudeId",
                principalTable: "NiveauxEtude",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitesEnseignement_SemestresPedagogiques_SemestrePedagogiqueId",
                table: "UnitesEnseignement",
                column: "SemestrePedagogiqueId",
                principalTable: "SemestresPedagogiques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
