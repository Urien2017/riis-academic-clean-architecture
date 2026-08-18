# Graph Report - src  (2026-08-14)

## Corpus Check
- 243 files · ~74,470 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 2421 nodes · 4433 edges · 88 communities (84 shown, 4 thin omitted)
- Extraction: 100% EXTRACTED · 0% INFERRED · 0% AMBIGUOUS · INFERRED: 6 edges (avg confidence: 0.8)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- ProgrammePedagogiqueService
- LookupDto
- ReleveNoteTemplateWordExportService
- DashboardAcademique.razor
- PaiementScolarite
- RIIS.Academic.Infrastructure.Persistence.Configurations
- DossiersScolarite.razor
- FinancesScolarite.razor
- Evaluations.razor
- TarifsScolariteService
- TarifsScolarite.razor
- Inscription
- EvaluationsService
- ProcesVerbaux.razor
- Maquettes.razor
- UnitesEnseignement.razor
- ElementsConstitutifs.razor
- EtudiantDto
- Inscriptions.razor
- ParcoursAcademiques.razor
- TypesElementsScolarite.razor
- RiisAcademicDbContext
- Releves.razor
- RIIS.Academic.Domain
- Etudiants.razor
- Specialites.razor
- SaisieNotes.razor
- ClassesPedagogiquesService
- ClassesPedagogiques.razor
- ProcesVerbauxService
- Semestres.razor
- _Imports.razor
- CycleFormation
- ReferentielsService
- AnneesAcademiques.razor
- CyclesFormation.razor
- NiveauxEtude.razor
- ModesPaiementScolarite.razor
- Filieres.razor
- ProcesVerbalDetails.razor
- DashboardAcademiqueDto
- IReferentielsService
- ModePaiementScolariteDto
- TypeElementScolariteDto
- ProcesVerbalExcelExportService
- DependencyInjection.cs
- ReleveDetails.razor
- RIIS.Academic.Application.ProcesVerbaux.Dtos
- ProcesVerbalWordExportService
- RIIS.Academic.Infrastructure.csproj
- IRepository
- ElementScolariteEtudiant
- .MapRiisAcademicExportEndpoints
- ProcesVerbalTemplateWordExportService
- DossiersScolariteService
- RIIS.Academic.Application.Referentiels.Dtos
- .ExporterProcesVerbalAsync
- RIIS.Academic.Application.Abstractions.Services
- .BuildSemestreDto
- RIIS.Academic.Application.Abstractions.Persistence
- ProcesVerbalDto
- AdministrationDossierScolariteDto
- NotificationScolarite
- Dashboard.razor
- ElementConstitutif
- DashboardLayout.razor
- IDossiersScolariteService
- InitialCreate
- DossierScolarite
- ModulePlaceholder.razor
- Routes.razor
- DocumentElementScolarite
- ValidationElementScolarite
- MaquettePedagogique
- RIIS.Academic.Infrastructure.Persistence
- .InitializeRiisAcademicDatabaseAsync
- NavBar.razor
- ValidationInscription
- IInscriptionService
- Etablissement
- EtudiantsService.cs
- .AddRiisAcademicInfrastructure
- RiisAcademicDbContextModelSnapshot.cs
- .ExecuteAsync
- App.razor
- AnneeAcademiqueDto
- CycleFormationDto
- FiliereDto

## God Nodes (most connected - your core abstractions)
1. `RIIS.Academic.Domain` - 131 edges
2. `LookupDto` - 69 edges
3. `ProgrammePedagogiqueService` - 49 edges
4. `Inscription` - 44 edges
5. `RiisAcademicDbContext` - 42 edges
6. `ReferentielsService` - 37 edges
7. `RIIS.Academic.Infrastructure.Persistence.Configurations` - 36 edges
8. `IProgrammePedagogiqueService` - 33 edges
9. `DashboardAcademiqueService` - 32 edges
10. `ProcesVerbalDto` - 30 edges

## Surprising Connections (you probably didn't know these)
- `SaisieNotesGrilleDto` --references--> `TypeEvaluation`  [EXTRACTED]
  RIIS.Academic.Application/Notes/Dtos/SaisieNotesGrilleDto.cs → RIIS.Academic.Domain/Enums/TypeEvaluation.cs
- `ProcesVerbalDto` --references--> `TypeProcesVerbal`  [EXTRACTED]
  RIIS.Academic.Application/ProcesVerbaux/Dtos/ProcesVerbalDto.cs → RIIS.Academic.Domain/Enums/TypeProcesVerbal.cs
- `ProcesVerbalLigneDto` --references--> `DecisionAcademique`  [EXTRACTED]
  RIIS.Academic.Application/ProcesVerbaux/Dtos/ProcesVerbalLigneDto.cs → RIIS.Academic.Domain/Enums/DecisionAcademique.cs
- `ProcesVerbalExcelExportService` --implements--> `IProcesVerbalExcelExportService`  [EXTRACTED]
  RIIS.Academic.Infrastructure/Documents/ProcesVerbalExcelExportService.cs → RIIS.Academic.Application/ProcesVerbaux/Services/IProcesVerbalExcelExportService.cs
- `ProcesVerbalTemplateWordExportService` --implements--> `IProcesVerbalTemplateWordExportService`  [EXTRACTED]
  RIIS.Academic.Infrastructure/Documents/ProcesVerbalTemplateWordExportService.cs → RIIS.Academic.Application/ProcesVerbaux/Services/IProcesVerbalTemplateWordExportService.cs

## Import Cycles
- None detected.

## Communities (88 total, 4 thin omitted)

### Community 0 - "ProgrammePedagogiqueService"
Cohesion: 0.06
Nodes (25): RIIS.Academic.Application.Programmes.Dtos, ElementConstitutifDto, ElementConstitutifHierarchyDto, DateOnly, MaquettePedagogiqueDto, DateOnly, List, MaquettePedagogiqueHierarchyDto (+17 more)

### Community 1 - "LookupDto"
Cohesion: 0.09
Nodes (23): LookupDto, DateOnly, InscriptionDto, CancellationToken, List, Task, IInscriptionsService, CancellationToken (+15 more)

### Community 2 - "ReleveNoteTemplateWordExportService"
Cohesion: 0.06
Nodes (28): DateOnly, List, ReleveNoteAnnuelDto, ReleveNoteEtudiantDisponibleDto, ReleveNoteLigneDto, ReleveNoteResumeDto, List, ReleveNoteSemestreDto (+20 more)

### Community 3 - "DashboardAcademique.razor"
Cohesion: 0.03
Nodes (63): ClasseSyntheseDashboard, DashboardFilterOption, DashboardParcoursFilterOption, EcRisqueDashboard, EvaluationProgressPoint, HeaderTemplate, RadzenAreaSeries, RadzenCategoryAxis (+55 more)

### Community 4 - "PaiementScolarite"
Cohesion: 0.06
Nodes (36): DateOnly, DateTime, List, AffectationPaiementDossierDto, EcheanceFinanciereDossierDto, ElementFinancierDossierDto, FinanceDossierScolariteDto, PaiementLibreDossierDto (+28 more)

### Community 5 - "RIIS.Academic.Infrastructure.Persistence.Configurations"
Cohesion: 0.04
Nodes (31): RIIS.Academic.Infrastructure.Persistence.Configurations, IEntityTypeConfiguration, DossierAdmission, EntityTypeBuilder, EtudiantConfiguration, EntityTypeBuilder, DossierAdmissionConfiguration, EntityTypeBuilder (+23 more)

### Community 6 - "DossiersScolarite.razor"
Cohesion: 0.03
Nodes (57): CodeLibelleOption, DisplayAutorisationParcours, DisplayExistence, DisplayParcours, DisplayStatut, FiliereBelongsToCycle, FilterMatches, FormatCodeLibelle (+49 more)

### Community 7 - "FinancesScolarite.razor"
Cohesion: 0.04
Nodes (54): CodeLibelleOption, DisplayParcours, DisplayStatut, EnregistrerPaiementLibre, FiliereBelongsToCycle, FilterMatches, FormatCodeLibelle, FormatMoney (+46 more)

### Community 8 - "Evaluations.razor"
Cohesion: 0.04
Nodes (53): Cancel, Create, Delete, Edit, EnsureSelectedValueIsAvailable, GetDefaultPonderation, GetShortTypeLibelle, GetTypeCode (+45 more)

### Community 9 - "TarifsScolariteService"
Cohesion: 0.09
Nodes (17): DateOnly, TarifScolariteContexteDto, DateOnly, TarifScolariteDto, CancellationToken, List, Task, ITarifsScolariteService (+9 more)

### Community 10 - "TarifsScolarite.razor"
Cohesion: 0.04
Nodes (49): Cancel, CodeLibelleOption, Create, Delete, DisplayValidite, Edit, FiliereBelongsToCycle, FormatCodeLibelle (+41 more)

### Community 11 - "Inscription"
Cohesion: 0.15
Nodes (22): DashboardData, DashboardGroupeKey, GroupeDashboard, DashboardAcademiqueFilterDto, CancellationToken, IEnumerable, IReadOnlyCollection, List (+14 more)

### Community 12 - "EvaluationsService"
Cohesion: 0.13
Nodes (12): DateTime, EvaluationAcademiqueDto, CancellationToken, IReadOnlyCollection, List, Task, EvaluationsService, CancellationToken (+4 more)

### Community 13 - "ProcesVerbaux.razor"
Cohesion: 0.05
Nodes (42): ApplySearch, Contains, EnsureSelectedValueIsAvailable, ExportExcel, ExportTemplateWord, ExportWord, FormatCredit, FormatNote (+34 more)

### Community 14 - "Maquettes.razor"
Cohesion: 0.05
Nodes (41): ApplyFilter, BuildVersionOptions, Cancel, Create, Delete, Edit, Load, LoadLookups (+33 more)

### Community 15 - "UnitesEnseignement.razor"
Cohesion: 0.05
Nodes (41): ApplyFilters, CancelUe, CreateUe, DeleteUe, EditUe, EnsureSelectedValueIsAvailable, Load, NotifyError (+33 more)

### Community 16 - "ElementsConstitutifs.razor"
Cohesion: 0.05
Nodes (39): Cancel, Create, Delete, Edit, Load, LoadLookups, NotifyError, NotifySuccess (+31 more)

### Community 17 - "EtudiantDto"
Cohesion: 0.09
Nodes (19): DateOnly, EtudiantDto, CancellationToken, List, Task, EtudiantsService, CancellationToken, List (+11 more)

### Community 18 - "Inscriptions.razor"
Cohesion: 0.05
Nodes (38): ApplyFilter, Cancel, Create, Delete, Edit, Load, LoadLookups, NotifyError (+30 more)

### Community 19 - "ParcoursAcademiques.razor"
Cohesion: 0.05
Nodes (38): Cancel, Create, Delete, Edit, LoadParcours, LoadReferentiels, NotifyError, NotifySuccess (+30 more)

### Community 20 - "TypesElementsScolarite.razor"
Cohesion: 0.05
Nodes (37): CategorieOption, Cancel, CategorieOption, Create, Delete, DisplayCategorie, Edit, Load (+29 more)

### Community 21 - "RiisAcademicDbContext"
Cohesion: 0.09
Nodes (32): DbContext, DbSet, DecisionAcademique, StatutValidationAcademique, ICollection, ClassePedagogique, DateTime, ResultatAnnuel (+24 more)

### Community 22 - "Releves.razor"
Cohesion: 0.05
Nodes (37): RadzenPanel, ApplySearch, Contains, ExportTemplateWord, ExportWord, FormatCredit, FormatNote, Load (+29 more)

### Community 23 - "RIIS.Academic.Domain"
Cohesion: 0.06
Nodes (8): RIIS.Academic.Application.Notes.Services, RIIS.Academic.Domain, RIIS.Academic.Application.Notes.Dtos, SaisieNoteLigneDto, DateTime, AuditableEntity, Entity, StatutPresenceEvaluation

### Community 24 - "Etudiants.razor"
Cohesion: 0.06
Nodes (35): Cancel, Create, Delete, Edit, Load, NotifyError, NotifySuccess, OnDateNaissanceChanged (+27 more)

### Community 25 - "Specialites.razor"
Cohesion: 0.06
Nodes (35): Cancel, Create, Delete, Edit, GetFiliereIdsForCycle, Load, NotifyError, NotifySuccess (+27 more)

### Community 26 - "SaisieNotes.razor"
Cohesion: 0.06
Nodes (35): EnsureSelectedValueIsAvailable, LoadGrille, NotifyError, NotifySuccess, OnAnneeAcademiqueChanged, OnEvaluationFilterChanged, OnInitializedAsync, OnPresenceChanged (+27 more)

### Community 27 - "ClassesPedagogiquesService"
Cohesion: 0.17
Nodes (10): ClassePedagogiqueDto, CancellationToken, IReadOnlyCollection, List, Task, ClassesPedagogiquesService, CancellationToken, List (+2 more)

### Community 28 - "ClassesPedagogiques.razor"
Cohesion: 0.06
Nodes (33): Cancel, Create, Delete, Edit, Load, LoadLookups, NotifyError, NotifySuccess (+25 more)

### Community 29 - "ProcesVerbauxService"
Cohesion: 0.14
Nodes (13): JsonElement, List, ProcesVerbalLigneDto, CancellationToken, List, Task, IProcesVerbauxService, CancellationToken (+5 more)

### Community 30 - "Semestres.razor"
Cohesion: 0.06
Nodes (32): Cancel, Create, Delete, Edit, Load, LoadLookups, NotifyError, NotifySuccess (+24 more)

### Community 31 - "_Imports.razor"
Cohesion: 0.06
Nodes (30): Microsoft.AspNetCore.Components, Microsoft.AspNetCore.Components.Routing, Microsoft.AspNetCore.Components.Web, Radzen.Blazor, RIIS.Academic.Application.ClassesPedagogiques.Dtos, RIIS.Academic.Application.ClassesPedagogiques.Services, RIIS.Academic.Application.Common.Dtos, RIIS.Academic.Application.Dashboard.Dtos (+22 more)

### Community 32 - "CycleFormation"
Cohesion: 0.10
Nodes (19): ParcoursPlanItem, IReadOnlyCollection, ICollection, CycleFormation, ICollection, Filiere, ICollection, Specialite (+11 more)

### Community 33 - "ReferentielsService"
Cohesion: 0.20
Nodes (4): CancellationToken, List, Task, ReferentielsService

### Community 34 - "AnneesAcademiques.razor"
Cohesion: 0.06
Nodes (30): Cancel, Create, Delete, Edit, Load, NotifyError, NotifySuccess, OnInitializedAsync (+22 more)

### Community 35 - "CyclesFormation.razor"
Cohesion: 0.06
Nodes (30): Cancel, Create, Delete, Edit, Load, NotifyError, NotifySuccess, OnInitializedAsync (+22 more)

### Community 36 - "NiveauxEtude.razor"
Cohesion: 0.06
Nodes (30): Cancel, Create, Delete, Edit, Load, NotifyError, NotifySuccess, OnInitializedAsync (+22 more)

### Community 37 - "ModesPaiementScolarite.razor"
Cohesion: 0.06
Nodes (30): Cancel, Create, Delete, Edit, Load, NotifyError, NotifySuccess, OnInitializedAsync (+22 more)

### Community 38 - "Filieres.razor"
Cohesion: 0.07
Nodes (29): Cancel, Create, Delete, Edit, Load, NotifyError, NotifySuccess, OnInitializedAsync (+21 more)

### Community 39 - "ProcesVerbalDetails.razor"
Cohesion: 0.07
Nodes (27): BackToList, BuildEcHeaders, ExportExcel, ExportTemplateWord, ExportWord, FormatCredit, FormatNote, GetNoteCssClass (+19 more)

### Community 40 - "DashboardAcademiqueDto"
Cohesion: 0.10
Nodes (15): RIIS.Academic.Application.Dashboard.Dtos, DateTime, List, DashboardAcademiqueDto, DashboardClasseSyntheseDto, DashboardEcRisqueDto, DashboardEvaluationCompletionDto, DashboardEvaluationCompletionParSemestreDto (+7 more)

### Community 41 - "IReferentielsService"
Cohesion: 0.22
Nodes (5): NiveauEtudeDto, CancellationToken, List, Task, IReferentielsService

### Community 42 - "ModePaiementScolariteDto"
Cohesion: 0.12
Nodes (13): ModePaiementScolariteDto, CancellationToken, List, Task, IModesPaiementScolariteService, CancellationToken, List, Task (+5 more)

### Community 43 - "TypeElementScolariteDto"
Cohesion: 0.15
Nodes (10): TypeElementScolariteDto, CancellationToken, List, Task, ITypesElementsScolariteService, CancellationToken, List, Task (+2 more)

### Community 44 - "ProcesVerbalExcelExportService"
Cohesion: 0.18
Nodes (8): CancellationToken, IReadOnlyCollection, List, string, Task, ZipArchive, ProcesVerbalExcelExportService, StringBuilder

### Community 45 - "DependencyInjection.cs"
Cohesion: 0.11
Nodes (10): RIIS.Academic.Application.ClassesPedagogiques.Services, RIIS.Academic.Application.Programmes.Services, RIIS.Academic.Application.Evaluations.Dtos, RIIS.Academic.Application.ClassesPedagogiques.Dtos, RIIS.Academic.Application.Inscriptions.Dtos, RIIS.Academic.Infrastructure.Persistence.Repositories, RIIS.Academic.Application.Dashboard.Services, RIIS.Academic.Application.Common.Dtos (+2 more)

### Community 46 - "ReleveDetails.razor"
Cohesion: 0.08
Nodes (24): BackToList, ExportTemplateWord, ExportWord, FormatCredit, FormatNote, GetDecisionBadgeStyle, OnParametersSetAsync, Columns (+16 more)

### Community 47 - "RIIS.Academic.Application.ProcesVerbaux.Dtos"
Cohesion: 0.15
Nodes (7): RIIS.Academic.Application.ProcesVerbaux.Services, RIIS.Academic.Application.Releves.Services, RIIS.Academic.Application.ProcesVerbaux.Dtos, RIIS.Academic.Infrastructure.Documents, RIIS.Academic.Application.Releves.Dtos, IProcesVerbalExcelExportService, System.Globalization

### Community 48 - "ProcesVerbalWordExportService"
Cohesion: 0.20
Nodes (6): IReadOnlyCollection, Label, List, string, Value, ProcesVerbalWordExportService

### Community 49 - "RIIS.Academic.Infrastructure.csproj"
Cohesion: 0.11
Nodes (16): Microsoft.EntityFrameworkCore (10.0.10), Microsoft.EntityFrameworkCore.SqlServer (10.0.10), Microsoft.EntityFrameworkCore.Tools (10.0.10), Radzen.Blazor (11.1.5), net10.0, Microsoft.NET.Sdk.Web, net10.0, Microsoft.NET.Sdk (+8 more)

### Community 50 - "IRepository"
Cohesion: 0.18
Nodes (8): CancellationToken, List, Task, IRepository, CancellationToken, List, Task, EfRepository

### Community 51 - "ElementScolariteEtudiant"
Cohesion: 0.12
Nodes (11): IEnumerable, StatutElementScolarite, DateTime, ICollection, ElementScolariteEtudiant, ICollection, TypeElementScolarite, EntityTypeBuilder (+3 more)

### Community 52 - ".MapRiisAcademicExportEndpoints"
Cohesion: 0.11
Nodes (13): RIIS.Academic.Web.Extensions, IEndpointRouteBuilder, ProcesVerbalExcelExportDto, CancellationToken, Task, ReleveNoteWordExportDto, CancellationToken, Task (+5 more)

### Community 53 - "ProcesVerbalTemplateWordExportService"
Cohesion: 0.21
Nodes (5): ProcesVerbalElementConstitutifLigneDto, IReadOnlyCollection, List, XNamespace, ProcesVerbalTemplateWordExportService

### Community 54 - "DossiersScolariteService"
Cohesion: 0.20
Nodes (9): DossierScolariteContext, DossierScolariteSnapshot, DateOnly, DateTime, DossierScolariteDto, List, DossierScolariteContext, DossierScolariteSnapshot (+1 more)

### Community 55 - "RIIS.Academic.Application.Referentiels.Dtos"
Cohesion: 0.12
Nodes (6): RIIS.Academic.Application.Referentiels.Dtos, RIIS.Academic.Application.Referentiels.Services, FiliereLookupDto, ParcoursAcademiqueDto, ParcoursFormationDto, SpecialiteDto

### Community 56 - ".ExporterProcesVerbalAsync"
Cohesion: 0.12
Nodes (10): ProcesVerbalWordExportDto, CancellationToken, Task, IProcesVerbalTemplateWordExportService, CancellationToken, Task, IProcesVerbalWordExportService, CancellationToken (+2 more)

### Community 57 - "RIIS.Academic.Application.Abstractions.Services"
Cohesion: 0.13
Nodes (6): RIIS.Academic.Application.Abstractions.Services, ICalculNotesService, CancellationToken, Task, IProcesVerbalService, CalculNotesService

### Community 58 - ".BuildSemestreDto"
Cohesion: 0.26
Nodes (5): CancellationToken, IReadOnlyCollection, List, Task, RelevesNotesService

### Community 59 - "RIIS.Academic.Application.Abstractions.Persistence"
Cohesion: 0.29
Nodes (3): RIIS.Academic.Application.Scolarite.Dtos, RIIS.Academic.Application.Abstractions.Persistence, RIIS.Academic.Application.Scolarite.Services

### Community 60 - "ProcesVerbalDto"
Cohesion: 0.19
Nodes (7): DateTime, List, ProcesVerbalDto, CancellationToken, Task, XDocument, XElement

### Community 61 - "AdministrationDossierScolariteDto"
Cohesion: 0.32
Nodes (6): DateTime, List, AdministrationDossierScolariteDto, DocumentAdministratifDossierDto, CancellationToken, Task

### Community 62 - "NotificationScolarite"
Cohesion: 0.17
Nodes (7): CanalNotificationScolarite, StatutNotificationScolarite, TypeNotificationScolarite, DateTime, NotificationScolarite, EntityTypeBuilder, NotificationScolariteConfiguration

### Community 63 - "Dashboard.razor"
Cohesion: 0.15
Nodes (12): DashboardSection, DashboardShortcut, Open, NavigationManager, RadzenButton, RadzenCard, RadzenColumn, RadzenIcon (+4 more)

### Community 64 - "ElementConstitutif"
Cohesion: 0.20
Nodes (8): DateTime, ResultatElementConstitutif, ICollection, ElementConstitutif, EntityTypeBuilder, ResultatElementConstitutifConfiguration, EntityTypeBuilder, ElementConstitutifConfiguration

### Community 65 - "DashboardLayout.razor"
Cohesion: 0.18
Nodes (10): LayoutComponentBase, NavBar, RadzenBody, RadzenHeader, RadzenLayout, RadzenSidebar, RadzenSidebarToggle, RadzenStack (+2 more)

### Community 66 - "IDossiersScolariteService"
Cohesion: 0.40
Nodes (4): CancellationToken, List, Task, IDossiersScolariteService

### Community 67 - "InitialCreate"
Cohesion: 0.24
Nodes (5): RIIS.Academic.Infrastructure.Persistence.Migrations, Migration, MigrationBuilder, ModelBuilder, InitialCreate

### Community 68 - "DossierScolarite"
Cohesion: 0.22
Nodes (6): StatutDossierScolarite, DateTime, ICollection, DossierScolarite, EntityTypeBuilder, DossierScolariteConfiguration

### Community 69 - "ModulePlaceholder.razor"
Cohesion: 0.20
Nodes (9): FormatWord, HumanizeSegment, RadzenAlert, RadzenButton, RadzenCard, RadzenIcon, RadzenStack, RadzenText (+1 more)

### Community 70 - "Routes.razor"
Cohesion: 0.22
Nodes (8): FocusOnNavigate, Found, LayoutView, NotFound, RadzenCard, RadzenText, Router, RouteView

### Community 71 - "DocumentElementScolarite"
Cohesion: 0.25
Nodes (5): StatutDocumentScolarite, DateTime, DocumentElementScolarite, EntityTypeBuilder, DocumentElementScolariteConfiguration

### Community 72 - "ValidationElementScolarite"
Cohesion: 0.25
Nodes (5): StatutValidationScolarite, DateTime, ValidationElementScolarite, EntityTypeBuilder, ValidationElementScolariteConfiguration

### Community 73 - "MaquettePedagogique"
Cohesion: 0.25
Nodes (6): DateOnly, DateTime, ICollection, MaquettePedagogique, EntityTypeBuilder, MaquettePedagogiqueConfiguration

### Community 74 - "RIIS.Academic.Infrastructure.Persistence"
Cohesion: 0.25
Nodes (4): RIIS.Academic.Infrastructure, RIIS.Academic.Infrastructure.Persistence, Radzen, RIIS.Academic.Web.Components

### Community 75 - ".InitializeRiisAcademicDatabaseAsync"
Cohesion: 0.25
Nodes (5): RIIS.Academic.Infrastructure.Persistence.Seeders, IServiceProvider, CancellationToken, Task, DatabaseInitializer

### Community 76 - "NavBar.razor"
Cohesion: 0.29
Nodes (6): RadzenPanelMenu, RadzenPanelMenuItem, RadzenCard, RadzenIcon, RadzenStack, RadzenText

### Community 77 - "ValidationInscription"
Cohesion: 0.33
Nodes (4): DateOnly, ValidationInscription, EntityTypeBuilder, ValidationInscriptionConfiguration

### Community 78 - "IInscriptionService"
Cohesion: 0.47
Nodes (3): CancellationToken, Task, IInscriptionService

### Community 79 - "Etablissement"
Cohesion: 0.40
Nodes (3): Etablissement, EntityTypeBuilder, EtablissementConfiguration

### Community 81 - ".AddRiisAcademicInfrastructure"
Cohesion: 0.60
Nodes (3): IConfiguration, IServiceCollection, DependencyInjection

### Community 82 - "RiisAcademicDbContextModelSnapshot.cs"
Cohesion: 0.40
Nodes (3): ModelSnapshot, ModelBuilder, RiisAcademicDbContextModelSnapshot

### Community 83 - ".ExecuteAsync"
Cohesion: 0.70
Nodes (3): CancellationToken, Task, RiisAcademicDatabaseResetter

### Community 84 - "App.razor"
Cohesion: 0.50
Nodes (3): HeadOutlet, RadzenTheme, Routes

## Knowledge Gaps
- **1059 isolated node(s):** `net10.0`, `Microsoft.NET.Sdk.Web`, `GroupeDashboard`, `DashboardGroupeKey`, `DashboardData` (+1054 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **4 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `RIIS.Academic.Domain` connect `RIIS.Academic.Domain` to `ProgrammePedagogiqueService`, `LookupDto`, `PaiementScolarite`, `RIIS.Academic.Infrastructure.Persistence.Configurations`, `TarifsScolariteService`, `EvaluationsService`, `EtudiantDto`, `RiisAcademicDbContext`, `ProcesVerbauxService`, `CycleFormation`, `ModePaiementScolariteDto`, `TypeElementScolariteDto`, `DependencyInjection.cs`, `RIIS.Academic.Application.ProcesVerbaux.Dtos`, `ElementScolariteEtudiant`, `RIIS.Academic.Application.Referentiels.Dtos`, `RIIS.Academic.Application.Abstractions.Services`, `RIIS.Academic.Application.Abstractions.Persistence`, `AdministrationDossierScolariteDto`, `NotificationScolarite`, `ElementConstitutif`, `DossierScolarite`, `DocumentElementScolarite`, `ValidationElementScolarite`, `MaquettePedagogique`, `RIIS.Academic.Infrastructure.Persistence`, `.InitializeRiisAcademicDatabaseAsync`, `ValidationInscription`, `Etablissement`, `EtudiantsService.cs`?**
  _High betweenness centrality (0.126) - this node is a cross-community bridge._
- **Why does `RiisAcademicDbContext` connect `RiisAcademicDbContext` to `ElementConstitutif`, `CycleFormation`, `PaiementScolarite`, `RIIS.Academic.Infrastructure.Persistence.Configurations`, `DossierScolarite`, `DocumentElementScolarite`, `ValidationElementScolarite`, `MaquettePedagogique`, `RIIS.Academic.Infrastructure.Persistence`, `Inscription`, `ModePaiementScolariteDto`, `ValidationInscription`, `TarifsScolariteService`, `Etablissement`, `EtudiantDto`, `.ExecuteAsync`, `ElementScolariteEtudiant`, `NotificationScolarite`?**
  _High betweenness centrality (0.043) - this node is a cross-community bridge._
- **Why does `Inscription` connect `Inscription` to `ElementConstitutif`, `LookupDto`, `DossierScolarite`, `RIIS.Academic.Infrastructure.Persistence.Configurations`, `MaquettePedagogique`, `ValidationInscription`, `EtudiantDto`, `RiisAcademicDbContext`, `DossiersScolariteService`, `RIIS.Academic.Domain`, `.BuildSemestreDto`, `ClassesPedagogiquesService`?**
  _High betweenness centrality (0.038) - this node is a cross-community bridge._
- **What connects `net10.0`, `Microsoft.NET.Sdk.Web`, `GroupeDashboard` to the rest of the system?**
  _1059 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `ProgrammePedagogiqueService` be split into smaller, more focused modules?**
  _Cohesion score 0.056731984829329965 - nodes in this community are weakly interconnected._
- **Should `LookupDto` be split into smaller, more focused modules?**
  _Cohesion score 0.08651911468812877 - nodes in this community are weakly interconnected._
- **Should `ReleveNoteTemplateWordExportService` be split into smaller, more focused modules?**
  _Cohesion score 0.055900621118012424 - nodes in this community are weakly interconnected._