# Graph Report - C:\Users\VULCANO\Documents\Codex\2026-07-02\bonjour-on-va-concevoir-d-velopper\outputs\riis-academic-clean-architecture  (2026-08-10)

## Corpus Check
- 210 files · ~119,559 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 2083 nodes · 3960 edges · 77 communities
- Extraction: 100% EXTRACTED · 0% INFERRED · 0% AMBIGUOUS · INFERRED: 8 edges (avg confidence: 0.81)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- Dto Enseignement Hierarchy
- Lookup Notes Saisie
- Dto Formation Filiere
- Radzen Dashboard Point
- Radzen Changed Form
- Evaluation Lookup Evaluations
- Build Dashboard Groupe
- Radzen Proces Export
- Etudiant Etudiants Service
- Radzen Maquettes Filter
- Radzen Changed Programme
- Radzen Text Programme
- Radzen Inscriptions Inscription
- Radzen Releves Service
- Radzen Etudiants Notify
- Radzen Notes Changed
- Ensure Seed Data
- Seed Data Notes
- Radzen Classes Pedagogiques
- Proces Verbal Element
- Proces Lookup Verbal
- Radzen Programme Semestres
- Classe Pedagogique Lookup
- Official Data Seed
- Radzen Referentiels Specialites
- Radzen Referentiels Annees
- Radzen Referentiels Cycles
- Radzen Referentiels Niveaux
- Cell Table Build
- Radzen Referentiels Filieres
- Configuration Entity Type
- Rii Dtos Services
- Table Build Proces
- Seeder Evaluation Entity
- Radzen Proces Verbal
- Dashboard Dto Academique
- Rii Service Services
- Model Builder Rii
- Configuration Dossier Admission
- Radzen Releves Export
- Cycle Formation Configuration
- Build Table Template
- Table Build Releve
- Build Cell Xml
- Releve Template Table
- Releve Note Word
- Service Calcul Notes
- Microsoft Rii Entity
- Definition Ensure Formation
- List Delete Repository
- Evaluation Token Seeder
- Bts Programme Ensure
- Evaluation Notes Token
- Calculer Moyenne Cancellation
- Element Constitutif Resultat
- Rii Clean Architecture
- List Proces Verbal
- Resultat Annuel Niveau
- Semestre Resultat Pedagogique
- Unite Enseignement Resultat
- Proces Service Verbal
- Radzen Dashboard Route
- Proces Verbal Export
- Radzen Layout Sidebar
- Releve Note Dto
- Radzen Module Route
- Found View Radzen
- Cycle Filiere Pedagogique
- Maquette Pedagogique Date
- Releve Note Etudiant
- Evaluation Academique Configuration
- Proces Verbal Configuration
- Rii Extensions Export
- Radzen Panel Menu
- Proces Verbal Excel
- Configuration Service Collection
- Head Outlet Radzen

## God Nodes (most connected - your core abstractions)
1. `RIIS.Academic.Domain` - 98 edges
2. `LookupDto` - 69 edges
3. `ProgrammePedagogiqueService` - 47 edges
4. `Inscription` - 43 edges
5. `IProgrammePedagogiqueService` - 33 edges
6. `DashboardAcademiqueService` - 31 edges
7. `ProcesVerbalDto` - 30 edges
8. `RiisAcademicDbContext` - 29 edges
9. `SemestrePedagogique` - 28 edges
10. `CycleFormationFiliere` - 27 edges

## Surprising Connections (you probably didn't know these)
- `RIIS Academic Clean Architecture Repository Root` --conceptually_related_to--> `RIIS Academic Prototype Clean Architecture`  [INFERRED]
  graphify-out/.graphify_root → README.md
- `ReleveNoteAnnuelDto` --references--> `ReleveNoteResumeDto`  [EXTRACTED]
  src/RIIS.Academic.Application/Releves/Dtos/ReleveNoteAnnuelDto.cs → src/RIIS.Academic.Application/Releves/Dtos/ReleveNoteResumeDto.cs
- `ReleveNoteSemestreDto` --references--> `ReleveNoteLigneDto`  [EXTRACTED]
  src/RIIS.Academic.Application/Releves/Dtos/ReleveNoteSemestreDto.cs → src/RIIS.Academic.Application/Releves/Dtos/ReleveNoteLigneDto.cs
- `DashboardAcademiqueDto` --references--> `DashboardAcademiqueFilterDto`  [EXTRACTED]
  src/RIIS.Academic.Application/Dashboard/Dtos/DashboardAcademiqueDto.cs → src/RIIS.Academic.Application/Dashboard/Dtos/DashboardAcademiqueFilterDto.cs
- `DashboardAcademiqueService` --implements--> `IDashboardAcademiqueService`  [EXTRACTED]
  src/RIIS.Academic.Application/Dashboard/Services/DashboardAcademiqueService.cs → src/RIIS.Academic.Application/Dashboard/Services/IDashboardAcademiqueService.cs

## Import Cycles
- None detected.

## Hyperedges (group relationships)
- **Clean Architecture Project Layers** — readme_riis_academic_domain, readme_riis_academic_application, readme_riis_academic_infrastructure, readme_riis_academic_api, readme_riis_academic_web [EXTRACTED 1.00]
- **Cycle Filiere Specialite Opening Model** — readme_anneeacademique, readme_cycleformationfiliere, readme_cycleformation, readme_filiere, readme_specialite [EXTRACTED 1.00]
- **CycleFormationFiliere Consumers** — readme_inscription, readme_classepedagogique, readme_maquettepedagogique, readme_cycleformationfiliere [EXTRACTED 1.00]

## Communities (77 total, 0 thin omitted)

### Community 0 - "Dto Enseignement Hierarchy"
Cohesion: 0.06
Nodes (25): RIIS.Academic.Application.Programmes.Dtos, ElementConstitutifDto, ElementConstitutifHierarchyDto, DateOnly, MaquettePedagogiqueDto, DateOnly, List, MaquettePedagogiqueHierarchyDto (+17 more)

### Community 1 - "Lookup Notes Saisie"
Cohesion: 0.08
Nodes (25): RIIS.Academic.Application.Notes.Services, RIIS.Academic.Application.Notes.Dtos, LookupDto, DateOnly, InscriptionDto, CancellationToken, List, Task (+17 more)

### Community 2 - "Dto Formation Filiere"
Cohesion: 0.08
Nodes (16): RIIS.Academic.Application.Referentiels.Dtos, RIIS.Academic.Application.Referentiels.Services, AnneeAcademiqueDto, CycleFormationDto, FiliereDto, FiliereLookupDto, NiveauEtudeDto, SpecialiteDto (+8 more)

### Community 3 - "Radzen Dashboard Point"
Cohesion: 0.03
Nodes (63): ClasseSyntheseDashboard, DashboardFilterOption, DashboardParcoursFilterOption, EcRisqueDashboard, EvaluationProgressPoint, HeaderTemplate, RadzenAreaSeries, RadzenCategoryAxis (+55 more)

### Community 4 - "Radzen Changed Form"
Cohesion: 0.04
Nodes (53): route:/evaluations, Cancel, Create, Delete, Edit, EnsureSelectedValueIsAvailable, GetDefaultPonderation, GetShortTypeLibelle (+45 more)

### Community 5 - "Evaluation Lookup Evaluations"
Cohesion: 0.11
Nodes (16): DateTime, EvaluationAcademiqueDto, CancellationToken, List, Task, EvaluationsService, CancellationToken, List (+8 more)

### Community 6 - "Build Dashboard Groupe"
Cohesion: 0.17
Nodes (17): DashboardData, DashboardGroupeKey, GroupeDashboard, DashboardAcademiqueFilterDto, CancellationToken, IEnumerable, IReadOnlyCollection, List (+9 more)

### Community 7 - "Radzen Proces Export"
Cohesion: 0.05
Nodes (42): route:/pv, ApplySearch, Contains, EnsureSelectedValueIsAvailable, ExportExcel, ExportTemplateWord, ExportWord, FormatCredit (+34 more)

### Community 8 - "Etudiant Etudiants Service"
Cohesion: 0.08
Nodes (20): RIIS.Academic.Application.Etudiants.Services, RIIS.Academic.Application.Etudiants.Dtos, DateOnly, EtudiantDto, CancellationToken, List, Task, EtudiantsService (+12 more)

### Community 9 - "Radzen Maquettes Filter"
Cohesion: 0.05
Nodes (41): route:/programme/maquettes, ApplyFilter, BuildVersionOptions, Cancel, Create, Delete, Edit, Load (+33 more)

### Community 10 - "Radzen Changed Programme"
Cohesion: 0.05
Nodes (41): route:/programme/ue, ApplyFilters, CancelUe, CreateUe, DeleteUe, EditUe, EnsureSelectedValueIsAvailable, Load (+33 more)

### Community 11 - "Radzen Text Programme"
Cohesion: 0.05
Nodes (39): route:/programme/ec, Cancel, Create, Delete, Edit, Load, LoadLookups, NotifyError (+31 more)

### Community 12 - "Radzen Inscriptions Inscription"
Cohesion: 0.05
Nodes (38): route:/inscriptions, ApplyFilter, Cancel, Create, Delete, Edit, Load, LoadLookups (+30 more)

### Community 13 - "Radzen Releves Service"
Cohesion: 0.05
Nodes (37): RadzenPanel, route:/releves, ApplySearch, Contains, ExportTemplateWord, ExportWord, FormatCredit, FormatNote (+29 more)

### Community 14 - "Radzen Etudiants Notify"
Cohesion: 0.06
Nodes (35): route:/etudiants, Cancel, Create, Delete, Edit, Load, NotifyError, NotifySuccess (+27 more)

### Community 15 - "Radzen Notes Changed"
Cohesion: 0.06
Nodes (35): route:/saisie-notes, EnsureSelectedValueIsAvailable, LoadGrille, NotifyError, NotifySuccess, OnAnneeAcademiqueChanged, OnEvaluationFilterChanged, OnInitializedAsync (+27 more)

### Community 16 - "Ensure Seed Data"
Cohesion: 0.17
Nodes (15): Nom, Prenoms, CancellationToken, DateOnly, List, RiisAcademicDbContext, string, Task (+7 more)

### Community 17 - "Seed Data Notes"
Cohesion: 0.13
Nodes (21): NotesSectionSeedData, NotesSeedData, ParsedNote, ProgrammeSeedContext, CancellationToken, List, RiisAcademicDbContext, string (+13 more)

### Community 18 - "Radzen Classes Pedagogiques"
Cohesion: 0.06
Nodes (33): route:/classes-pedagogiques, Cancel, Create, Delete, Edit, Load, LoadLookups, NotifyError (+25 more)

### Community 19 - "Proces Verbal Element"
Cohesion: 0.09
Nodes (19): byte, ElementConstitutifContext, JsonSerializerOptions, DecisionAcademique, ICollection, ClassePedagogique, ProcesVerbalLigne, EntityTypeBuilder (+11 more)

### Community 20 - "Proces Lookup Verbal"
Cohesion: 0.14
Nodes (13): JsonElement, List, ProcesVerbalLigneDto, CancellationToken, List, Task, IProcesVerbauxService, CancellationToken (+5 more)

### Community 21 - "Radzen Programme Semestres"
Cohesion: 0.06
Nodes (32): route:/programme/semestres, Cancel, Create, Delete, Edit, Load, LoadLookups, NotifyError (+24 more)

### Community 22 - "Classe Pedagogique Lookup"
Cohesion: 0.17
Nodes (10): ClassePedagogiqueDto, CancellationToken, IReadOnlyCollection, List, Task, ClassesPedagogiquesService, CancellationToken, List (+2 more)

### Community 23 - "Official Data Seed"
Cohesion: 0.16
Nodes (18): OfficialElementConstitutifData, OfficialProgrammeData, OfficialProgrammesSeedData, OfficialSemestreData, OfficialUniteEnseignementData, CancellationToken, DateOnly, IReadOnlyCollection (+10 more)

### Community 24 - "Radzen Referentiels Specialites"
Cohesion: 0.06
Nodes (31): route:/referentiels/specialites, Cancel, Create, Delete, Edit, Load, NotifyError, NotifySuccess (+23 more)

### Community 25 - "Radzen Referentiels Annees"
Cohesion: 0.06
Nodes (30): route:/referentiels/annees, Cancel, Create, Delete, Edit, Load, NotifyError, NotifySuccess (+22 more)

### Community 26 - "Radzen Referentiels Cycles"
Cohesion: 0.06
Nodes (30): route:/referentiels/cycles, Cancel, Create, Delete, Edit, Load, NotifyError, NotifySuccess (+22 more)

### Community 27 - "Radzen Referentiels Niveaux"
Cohesion: 0.06
Nodes (30): route:/referentiels/niveaux, Cancel, Create, Delete, Edit, Load, NotifyError, NotifySuccess (+22 more)

### Community 28 - "Cell Table Build"
Cohesion: 0.11
Nodes (20): Path, build_document(), extract_prepa_rows(), merge_repeated_cells(), normalize(), Merge consecutive repeated values in Spécialité, Semestre and UE columns. The…, resolve_ue(), set_cell_margins() (+12 more)

### Community 29 - "Radzen Referentiels Filieres"
Cohesion: 0.07
Nodes (29): route:/referentiels/filieres, Cancel, Create, Delete, Edit, Load, NotifyError, NotifySuccess (+21 more)

### Community 30 - "Configuration Entity Type"
Cohesion: 0.09
Nodes (16): RIIS.Academic.Infrastructure.Persistence.Configurations, IEntityTypeConfiguration, Etablissement, ContactUrgence, DateOnly, ValidationInscription, EntityTypeBuilder, EtablissementConfiguration (+8 more)

### Community 31 - "Rii Dtos Services"
Cohesion: 0.07
Nodes (28): Microsoft.AspNetCore.Components, Microsoft.AspNetCore.Components.Routing, Microsoft.AspNetCore.Components.Web, Radzen.Blazor, RIIS.Academic.Application.ClassesPedagogiques.Dtos, RIIS.Academic.Application.ClassesPedagogiques.Services, RIIS.Academic.Application.Common.Dtos, RIIS.Academic.Application.Dashboard.Dtos (+20 more)

### Community 32 - "Table Build Proces"
Cohesion: 0.15
Nodes (8): CancellationToken, IReadOnlyCollection, Label, string, Task, Value, ZipArchive, ProcesVerbalWordExportService

### Community 33 - "Seeder Evaluation Entity"
Cohesion: 0.10
Nodes (11): RIIS.Academic.Domain, RIIS.Academic.Infrastructure.Persistence.Seeders, DateTime, AuditableEntity, Entity, StatutInscription, StatutPresenceEvaluation, DateTime (+3 more)

### Community 34 - "Radzen Proces Verbal"
Cohesion: 0.07
Nodes (27): route:/pv/{ProcesVerbalId:long}, BackToList, BuildEcHeaders, ExportExcel, ExportTemplateWord, ExportWord, FormatCredit, FormatNote (+19 more)

### Community 35 - "Dashboard Dto Academique"
Cohesion: 0.10
Nodes (15): RIIS.Academic.Application.Dashboard.Dtos, DateTime, List, DashboardAcademiqueDto, DashboardClasseSyntheseDto, DashboardEcRisqueDto, DashboardEvaluationCompletionDto, DashboardEvaluationCompletionParSemestreDto (+7 more)

### Community 36 - "Rii Service Services"
Cohesion: 0.13
Nodes (11): RIIS.Academic.Application.ClassesPedagogiques.Services, RIIS.Academic.Application.Programmes.Services, RIIS.Academic.Application.Abstractions.Persistence, RIIS.Academic.Application.Evaluations.Dtos, RIIS.Academic.Application.ClassesPedagogiques.Dtos, RIIS.Academic.Application.Inscriptions.Dtos, RIIS.Academic.Infrastructure.Persistence.Repositories, RIIS.Academic.Application.Dashboard.Services (+3 more)

### Community 37 - "Model Builder Rii"
Cohesion: 0.10
Nodes (12): RIIS.Academic.Infrastructure.Persistence.Migrations, RIIS.Academic.Infrastructure.Persistence, Migration, ModelSnapshot, MigrationBuilder, ModelBuilder, RelaxNiveauEtudeNumeroConstraint, MigrationBuilder (+4 more)

### Community 38 - "Configuration Dossier Admission"
Cohesion: 0.09
Nodes (15): DbContext, DbSet, DossierAdmission, ICollection, Filiere, ICollection, Specialite, EntityTypeBuilder (+7 more)

### Community 39 - "Radzen Releves Export"
Cohesion: 0.08
Nodes (24): route:/releves/{InscriptionId:long}, BackToList, ExportTemplateWord, ExportWord, FormatCredit, FormatNote, GetDecisionBadgeStyle, OnParametersSetAsync (+16 more)

### Community 40 - "Cycle Formation Configuration"
Cohesion: 0.10
Nodes (14): IReadOnlyCollection, ICollection, AnneeAcademique, ICollection, CycleFormation, DateOnly, ICollection, CycleFormationFiliere (+6 more)

### Community 41 - "Build Table Template"
Cohesion: 0.16
Nodes (6): CancellationToken, IReadOnlyCollection, Task, XElement, XNamespace, ProcesVerbalTemplateWordExportService

### Community 42 - "Table Build Releve"
Cohesion: 0.17
Nodes (8): CancellationToken, IReadOnlyCollection, Label, string, Task, Value, ZipArchive, ReleveNoteWordExportService

### Community 43 - "Build Cell Xml"
Cohesion: 0.19
Nodes (7): CancellationToken, IReadOnlyCollection, string, Task, ZipArchive, ProcesVerbalExcelExportService, StringBuilder

### Community 44 - "Releve Template Table"
Cohesion: 0.19
Nodes (6): IReadOnlyCollection, List, XDocument, XElement, XNamespace, ReleveNoteTemplateWordExportService

### Community 45 - "Releve Note Word"
Cohesion: 0.12
Nodes (11): RIIS.Academic.Application.Releves.Services, RIIS.Academic.Application.Releves.Dtos, ReleveNoteLigneDto, ReleveNoteResumeDto, ReleveNoteWordExportDto, CancellationToken, Task, IReleveNoteTemplateWordExportService (+3 more)

### Community 46 - "Service Calcul Notes"
Cohesion: 0.10
Nodes (9): RIIS.Academic.Application.Abstractions.Services, ICalculNotesService, CancellationToken, Task, IInscriptionService, CancellationToken, Task, IProcesVerbalService (+1 more)

### Community 47 - "Microsoft Rii Entity"
Cohesion: 0.12
Nodes (16): Microsoft.EntityFrameworkCore (10.0.10), Microsoft.EntityFrameworkCore.SqlServer (10.0.10), Microsoft.EntityFrameworkCore.Tools (10.0.10), Radzen.Blazor (11.1.5), net10.0, Microsoft.NET.Sdk.Web, net10.0, Microsoft.NET.Sdk (+8 more)

### Community 48 - "Definition Ensure Formation"
Cohesion: 0.23
Nodes (13): FiliereDefinition, OuvertureFormationDefinition, short, SpecialiteDefinition, CancellationToken, RiisAcademicDbContext, string, Task (+5 more)

### Community 49 - "List Delete Repository"
Cohesion: 0.18
Nodes (8): CancellationToken, List, Task, IRepository, CancellationToken, List, Task, EfRepository

### Community 50 - "Evaluation Token Seeder"
Cohesion: 0.14
Nodes (13): CancellationToken, EvaluationDefinition, IReadOnlyList, RiisAcademicDbContext, string, Task, EvaluationDefinition, RiisAcademicEvaluationSeeder (+5 more)

### Community 51 - "Bts Programme Ensure"
Cohesion: 0.31
Nodes (10): BtsProgrammeLine, IGrouping, CancellationToken, IEnumerable, IReadOnlyList, RiisAcademicDbContext, string, Task (+2 more)

### Community 52 - "Evaluation Notes Token"
Cohesion: 0.26
Nodes (8): CancellationToken, EvaluationDefinition, IReadOnlyCollection, RiisAcademicDbContext, string, Task, EvaluationDefinition, RiisAcademicPrepaTestNotesSeeder

### Community 53 - "Calculer Moyenne Cancellation"
Cohesion: 0.26
Nodes (5): CancellationToken, IReadOnlyCollection, List, Task, RelevesNotesService

### Community 54 - "Element Constitutif Resultat"
Cohesion: 0.14
Nodes (9): StatutValidationAcademique, DateTime, ResultatElementConstitutif, ICollection, ElementConstitutif, EntityTypeBuilder, ResultatElementConstitutifConfiguration, EntityTypeBuilder (+1 more)

### Community 55 - "Rii Clean Architecture"
Cohesion: 0.14
Nodes (14): RIIS Academic Clean Architecture Repository Root, Graphify Scan Root, ASP.NET Core, Blazor Web App InteractiveServer, Clean Architecture, Entity Framework Core, RIIS.Academic.Api, RIIS.Academic.Application (+6 more)

### Community 56 - "List Proces Verbal"
Cohesion: 0.18
Nodes (8): DateTime, List, ProcesVerbalDto, ProcesVerbalElementConstitutifLigneDto, List, List, XDocument, List

### Community 57 - "Resultat Annuel Niveau"
Cohesion: 0.16
Nodes (8): DateTime, ResultatAnnuel, ICollection, NiveauEtude, EntityTypeBuilder, ResultatAnnuelConfiguration, EntityTypeBuilder, NiveauEtudeConfiguration

### Community 58 - "Semestre Resultat Pedagogique"
Cohesion: 0.16
Nodes (8): DateTime, ResultatSemestre, ICollection, SemestrePedagogique, EntityTypeBuilder, ResultatSemestreConfiguration, EntityTypeBuilder, SemestrePedagogiqueConfiguration

### Community 59 - "Unite Enseignement Resultat"
Cohesion: 0.16
Nodes (8): DateTime, ResultatUniteEnseignement, ICollection, UniteEnseignement, EntityTypeBuilder, ResultatUniteEnseignementConfiguration, EntityTypeBuilder, UniteEnseignementConfiguration

### Community 60 - "Proces Service Verbal"
Cohesion: 0.29
Nodes (4): RIIS.Academic.Application.ProcesVerbaux.Services, RIIS.Academic.Application.ProcesVerbaux.Dtos, RIIS.Academic.Infrastructure.Documents, System.Globalization

### Community 61 - "Radzen Dashboard Route"
Cohesion: 0.15
Nodes (12): route:/, DashboardSection, DashboardShortcut, Open, NavigationManager, RadzenButton, RadzenCard, RadzenColumn (+4 more)

### Community 62 - "Proces Verbal Export"
Cohesion: 0.18
Nodes (8): IEndpointRouteBuilder, ProcesVerbalWordExportDto, CancellationToken, Task, IProcesVerbalTemplateWordExportService, CancellationToken, Task, IProcesVerbalWordExportService

### Community 63 - "Radzen Layout Sidebar"
Cohesion: 0.18
Nodes (10): LayoutComponentBase, NavBar, RadzenBody, RadzenHeader, RadzenLayout, RadzenSidebar, RadzenSidebarToggle, RadzenStack (+2 more)

### Community 64 - "Releve Note Dto"
Cohesion: 0.18
Nodes (7): DateOnly, List, ReleveNoteAnnuelDto, List, ReleveNoteSemestreDto, CancellationToken, Task

### Community 65 - "Radzen Module Route"
Cohesion: 0.20
Nodes (9): route:/{*ModulePath}, FormatWord, HumanizeSegment, RadzenAlert, RadzenButton, RadzenCard, RadzenIcon, RadzenStack (+1 more)

### Community 66 - "Found View Radzen"
Cohesion: 0.22
Nodes (8): FocusOnNavigate, Found, LayoutView, NotFound, Router, RouteView, RadzenCard, RadzenText

### Community 67 - "Cycle Filiere Pedagogique"
Cohesion: 0.22
Nodes (9): AnneeAcademique, ClassePedagogique, CycleFormation, CycleFormationFiliere, Filiere, Inscription, MaquettePedagogique, Relation Cycle Filiere Specialite Ouverte (+1 more)

### Community 68 - "Maquette Pedagogique Date"
Cohesion: 0.25
Nodes (6): DateOnly, DateTime, ICollection, MaquettePedagogique, EntityTypeBuilder, MaquettePedagogiqueConfiguration

### Community 69 - "Releve Note Etudiant"
Cohesion: 0.32
Nodes (5): ReleveNoteEtudiantDisponibleDto, CancellationToken, List, Task, IRelevesNotesService

### Community 70 - "Evaluation Academique Configuration"
Cohesion: 0.29
Nodes (5): DateOnly, ICollection, EvaluationAcademique, EntityTypeBuilder, EvaluationAcademiqueConfiguration

### Community 71 - "Proces Verbal Configuration"
Cohesion: 0.29
Nodes (5): DateTime, ICollection, ProcesVerbal, EntityTypeBuilder, ProcesVerbalConfiguration

### Community 72 - "Rii Extensions Export"
Cohesion: 0.29
Nodes (5): RIIS.Academic.Infrastructure, RIIS.Academic.Web.Extensions, Radzen, RIIS.Academic.Web.Components, RiisAcademicExportEndpointExtensions

### Community 73 - "Radzen Panel Menu"
Cohesion: 0.29
Nodes (6): RadzenPanelMenu, RadzenPanelMenuItem, RadzenCard, RadzenIcon, RadzenStack, RadzenText

### Community 74 - "Proces Verbal Excel"
Cohesion: 0.33
Nodes (4): ProcesVerbalExcelExportDto, CancellationToken, Task, IProcesVerbalExcelExportService

### Community 75 - "Configuration Service Collection"
Cohesion: 0.60
Nodes (3): IConfiguration, IServiceCollection, DependencyInjection

### Community 76 - "Head Outlet Radzen"
Cohesion: 0.50
Nodes (3): HeadOutlet, RadzenTheme, Routes

## Knowledge Gaps
- **817 isolated node(s):** `net10.0`, `Microsoft.NET.Sdk.Web`, `GroupeDashboard`, `DashboardGroupeKey`, `DashboardData` (+812 more)
  These have ≤1 connection - possible missing edges or undocumented components.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `RIIS.Academic.Domain` connect `Seeder Evaluation Entity` to `Dto Enseignement Hierarchy`, `Lookup Notes Saisie`, `Dto Formation Filiere`, `Evaluation Lookup Evaluations`, `Etudiant Etudiants Service`, `Proces Verbal Element`, `Proces Lookup Verbal`, `Configuration Entity Type`, `Rii Service Services`, `Model Builder Rii`, `Configuration Dossier Admission`, `Cycle Formation Configuration`, `Releve Note Word`, `Service Calcul Notes`, `Element Constitutif Resultat`, `Resultat Annuel Niveau`, `Semestre Resultat Pedagogique`, `Unite Enseignement Resultat`, `Proces Service Verbal`, `Maquette Pedagogique Date`, `Evaluation Academique Configuration`, `Proces Verbal Configuration`?**
  _High betweenness centrality (0.151) - this node is a cross-community bridge._
- **Why does `ProcesVerbalDto` connect `List Proces Verbal` to `Table Build Proces`, `Cycle Formation Configuration`, `Build Table Template`, `Build Cell Xml`, `Proces Lookup Verbal`, `Proces Service Verbal`?**
  _High betweenness centrality (0.030) - this node is a cross-community bridge._
- **Why does `AnneeAcademique` connect `Cycle Formation Configuration` to `Dto Enseignement Hierarchy`, `Lookup Notes Saisie`, `Build Dashboard Groupe`, `Evaluation Academique Configuration`, `Proces Verbal Configuration`, `Configuration Dossier Admission`, `Definition Ensure Formation`, `Ensure Seed Data`, `Proces Verbal Element`, `Calculer Moyenne Cancellation`, `Classe Pedagogique Lookup`, `Resultat Annuel Niveau`?**
  _High betweenness centrality (0.029) - this node is a cross-community bridge._
- **What connects `net10.0`, `Microsoft.NET.Sdk.Web`, `GroupeDashboard` to the rest of the system?**
  _817 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Dto Enseignement Hierarchy` be split into smaller, more focused modules?**
  _Cohesion score 0.05765765765765766 - nodes in this community are weakly interconnected._
- **Should `Lookup Notes Saisie` be split into smaller, more focused modules?**
  _Cohesion score 0.08 - nodes in this community are weakly interconnected._
- **Should `Dto Formation Filiere` be split into smaller, more focused modules?**
  _Cohesion score 0.08077260755048288 - nodes in this community are weakly interconnected._