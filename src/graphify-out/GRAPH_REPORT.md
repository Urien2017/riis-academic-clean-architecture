# Graph Report - src  (2026-08-19)

## Corpus Check
- 370 files · ~291,845 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 4671 nodes · 6506 edges · 203 communities
- Extraction: 100% EXTRACTED · 0% INFERRED · 0% AMBIGUOUS · INFERRED: 6 edges (avg confidence: 0.8)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `d73aae5a`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- LookupDto
- InscriptionsService
- ReleveNoteTemplateWordExportService
- DashboardAcademique.razor
- FinancesScolariteService
- RIIS.Academic.Infrastructure.Persistence.Configurations
- DossiersScolarite.razor
- FinancesScolarite.razor
- Evaluations.razor
- TarifScolariteDto
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
- SemestrePedagogique
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
- RiisAcademicDbContext
- ReferentielsService
- AnneesAcademiques.razor
- CyclesFormation.razor
- NiveauxEtude.razor
- ModesPaiementScolarite.razor
- Filieres.razor
- ProcesVerbalDetails.razor
- DashboardAcademiqueDto
- Core Components
- ModePaiementScolariteDto
- TypeElementScolariteDto
- ProcesVerbalExcelExportService
- RIIS.Academic.Application.Abstractions.Persistence
- ReleveDetails.razor
- RIIS.Academic.Application.ProcesVerbaux.Dtos
- ProcesVerbalWordExportService
- RIIS.Academic.Infrastructure.csproj
- IRepository
- Detailed Component Analysis
- ReleveNoteAnnuelDto
- ProcesVerbalDto
- DossiersScolariteService
- Student Management APIs
- Academic Program Services
- SaisieNotesService
- EvaluationAcademique
- Database Migrations
- Testing Strategy
- ReleveNoteWordExportService
- Detailed Component Analysis
- Dashboard.razor
- MaquetteElementConstitutif
- DashboardLayout.razor
- Database Persistence
- InitialCreate
- DossierScolarite
- ModulePlaceholder.razor
- Routes.razor
- Academic Programs
- Template Management
- MaquettePedagogique
- RIIS.Academic.Web/Program.cs
- .InitializeRiisAcademicDatabaseAsync
- NavBar.razor
- ValidationInscription
- API Layer
- Detailed Component Analysis
- Repository Pattern Implementation
- .AddRiisAcademicInfrastructure
- RIIS.Academic.Infrastructure.Persistence
- .ExecuteAsync
- App.razor
- Dependency Injection & Service Registration
- Developer Guide
- Student Management
- Infrastructure Layer
- Troubleshooting
- Dashboard & Analytics
- Detailed Component Analysis
- Detailed Component Analysis
- Evaluation Management
- Reference Data Services
- Enrollment & Registration Services
- Reference Data Services
- Registration and Validation
- Document Generation
- Enrollment System
- Grade Management
- Student Management
- Data Seeding and Initialization
- Repository Pattern Implementation
- Document Generation APIs
- Transcript Generation
- Financial Overview Dashboard
- Enrollment System Domain
- Entity Configurations
- Entity Framework Context
- External Integrations & Abstractions
- Configuration & Deployment
- Payment Methods and Handling
- Financial Administration
- Entity Framework Context
- Excel Export Generation
- Course Units Management
- Semester Management
- Student Management UI
- Web Application
- Enrollment Services
- Student Management Services
- Student Management Services
- Domain Enums & Constants
- Grade & Evaluation Domain
- Reference Data Domain
- Student Management Domain
- Document Generation Services
- Domain Layer
- Fee Structures and Calculations
- Database Persistence
- Class Management UI
- Document Generation UI
- Enrollment System UI
- Administrative Document Management
- Etudiant
- Academic Program Services
- Application Layer
- Service Architecture & Patterns
- Infrastructure Layer
- Web & API Layer
- Admission Process
- Enrollment System
- Grade and Evaluation System
- Reference Data
- Feature Modules
- Dependency Injection & Configuration
- Constituent Elements Management
- Meeting Minutes Management
- Payment Methods Configuration
- Financial Status Tracking
- Student Dossier Management
- Grade Entry Interface
- Export and Document Generation APIs
- Program Management APIs
- Document Generation Services
- Dashboard & Analytics Services
- Document Generation Services
- Academic Programs Domain
- Domain Layer
- Academic Programs
- Program Hierarchy
- Semester and Course Structure
- Student Financial Dossiers
- Result Aggregation
- Document Generation
- Project Overview
- Financial Elements Types
- Tuition Fee Management
- Evaluations Management
- Financial Administration APIs
- Financial Administration Services
- Grade Calculation Services
- Application Layer
- Financial Administration Services
- Grade & Evaluation Services
- Architecture Guide
- Core Entities & Base Classes
- Supporting Entities
- Core Entities
- Word Document Generation
- Academic Programs UI
- Program Maquettes Management
- Financial Administration UI
- Dossier Creation Workflow
- Grade Management UI
- Enrollment APIs
- Grade Management APIs
- Dashboard & Analytics Services
- Getting Started
- EcheanceScolarite
- FinanceDossierScolariteDto.cs
- PaiementScolarite
- IProcesVerbauxService
- .ToAffectationDto
- ITarifsScolariteService
- RefactorTarifScolariteParcours
- RemoveTarifValiditeDates
- ResultatSemestre
- ResultatUniteEnseignement
- AddRefonteQoder
- RefactorClassePedagogique
- ProcesVerbalElementConstitutifLigneDto

## God Nodes (most connected - your core abstractions)
1. `RIIS.Academic.Domain` - 133 edges
2. `LookupDto` - 63 edges
3. `ProgrammePedagogiqueService` - 47 edges
4. `Inscription` - 43 edges
5. `RiisAcademicDbContext` - 43 edges
6. `ReferentielsService` - 37 edges
7. `RIIS.Academic.Infrastructure.Persistence.Configurations` - 37 edges
8. `IProgrammePedagogiqueService` - 33 edges
9. `DashboardAcademiqueService` - 32 edges
10. `ProcesVerbalDto` - 30 edges

## Surprising Connections (you probably didn't know these)
- `EtudiantDto` --references--> `AptitudeMedicale`  [EXTRACTED]
  RIIS.Academic.Application/Etudiants/Dtos/EtudiantDto.cs → RIIS.Academic.Domain/Enums/AptitudeMedicale.cs
- `EtudiantDto` --references--> `Sexe`  [EXTRACTED]
  RIIS.Academic.Application/Etudiants/Dtos/EtudiantDto.cs → RIIS.Academic.Domain/Enums/Sexe.cs
- `InscriptionDto` --references--> `StatutInscription`  [EXTRACTED]
  RIIS.Academic.Application/Inscriptions/Dtos/InscriptionDto.cs → RIIS.Academic.Domain/Enums/StatutInscription.cs
- `SaisieNotesGrilleDto` --references--> `TypeEvaluation`  [EXTRACTED]
  RIIS.Academic.Application/Notes/Dtos/SaisieNotesGrilleDto.cs → RIIS.Academic.Domain/Enums/TypeEvaluation.cs
- `ProcesVerbalDto` --references--> `TypeProcesVerbal`  [EXTRACTED]
  RIIS.Academic.Application/ProcesVerbaux/Dtos/ProcesVerbalDto.cs → RIIS.Academic.Domain/Enums/TypeProcesVerbal.cs

## Import Cycles
- None detected.

## Communities (203 total, 0 thin omitted)

### Community 0 - "LookupDto"
Cohesion: 0.06
Nodes (29): RIIS.Academic.Application.Programmes.Dtos, LookupDto, ElementConstitutifDto, ElementConstitutifHierarchyDto, DateOnly, MaquettePedagogiqueDto, DateOnly, List (+21 more)

### Community 1 - "InscriptionsService"
Cohesion: 0.15
Nodes (11): DateOnly, InscriptionDto, CancellationToken, List, Task, IInscriptionsService, CancellationToken, IReadOnlyCollection (+3 more)

### Community 2 - "ReleveNoteTemplateWordExportService"
Cohesion: 0.16
Nodes (8): CancellationToken, IReadOnlyCollection, List, Task, XDocument, XElement, XNamespace, ReleveNoteTemplateWordExportService

### Community 3 - "DashboardAcademique.razor"
Cohesion: 0.03
Nodes (63): ClasseSyntheseDashboard, DashboardFilterOption, DashboardParcoursFilterOption, EcRisqueDashboard, EvaluationProgressPoint, HeaderTemplate, RadzenAreaSeries, RadzenCategoryAxis (+55 more)

### Community 4 - "FinancesScolariteService"
Cohesion: 0.25
Nodes (8): List, FinanceDossierScolariteDto, PaiementLibreDossierDto, CancellationToken, IReadOnlyCollection, List, Task, FinancesScolariteService

### Community 5 - "RIIS.Academic.Infrastructure.Persistence.Configurations"
Cohesion: 0.06
Nodes (22): RIIS.Academic.Infrastructure.Persistence.Configurations, IEntityTypeConfiguration, EntityTypeBuilder, InscriptionConfiguration, EntityTypeBuilder, ClassePedagogiqueConfiguration, EntityTypeBuilder, ParcoursAcademiqueConfiguration (+14 more)

### Community 6 - "DossiersScolarite.razor"
Cohesion: 0.03
Nodes (57): CodeLibelleOption, DisplayAutorisationParcours, DisplayExistence, DisplayParcours, DisplayStatut, FiliereBelongsToCycle, FilterMatches, FormatCodeLibelle (+49 more)

### Community 7 - "FinancesScolarite.razor"
Cohesion: 0.04
Nodes (54): CodeLibelleOption, DisplayParcours, DisplayStatut, EnregistrerPaiementLibre, FiliereBelongsToCycle, FilterMatches, FormatCodeLibelle, FormatMoney (+46 more)

### Community 8 - "Evaluations.razor"
Cohesion: 0.04
Nodes (55): Cancel, Create, Delete, Edit, EnsureSelectedValueIsAvailable, GetDefaultPonderation, GetShortTypeLibelle, GetTypeCode (+47 more)

### Community 9 - "TarifScolariteDto"
Cohesion: 0.29
Nodes (6): TarifScolariteDto, CancellationToken, IReadOnlyCollection, List, Task, TarifsScolariteService

### Community 10 - "TarifsScolarite.razor"
Cohesion: 0.05
Nodes (36): Cancel, Create, Delete, Edit, LoadTarifs, NotifyError, NotifySuccess, OnInitializedAsync (+28 more)

### Community 11 - "Inscription"
Cohesion: 0.16
Nodes (20): DashboardData, DashboardGroupeKey, GroupeDashboard, DashboardAcademiqueFilterDto, CancellationToken, IEnumerable, IReadOnlyCollection, List (+12 more)

### Community 12 - "EvaluationsService"
Cohesion: 0.14
Nodes (12): DateTime, EvaluationAcademiqueDto, CancellationToken, IReadOnlyCollection, List, Task, EvaluationsService, CancellationToken (+4 more)

### Community 13 - "ProcesVerbaux.razor"
Cohesion: 0.05
Nodes (42): ApplySearch, Contains, EnsureSelectedValueIsAvailable, ExportExcel, ExportTemplateWord, ExportWord, FormatCredit, FormatNote (+34 more)

### Community 14 - "Maquettes.razor"
Cohesion: 0.05
Nodes (41): ApplyFilter, BuildVersionOptions, Cancel, Create, Delete, Edit, Load, LoadLookups (+33 more)

### Community 15 - "UnitesEnseignement.razor"
Cohesion: 0.05
Nodes (39): ApplyFilters, CancelUe, CreateUe, DeleteUe, EditUe, EnsureSelectedValueIsAvailable, Load, NotifyError (+31 more)

### Community 16 - "ElementsConstitutifs.razor"
Cohesion: 0.06
Nodes (35): Cancel, Create, Delete, Edit, Load, LoadLookups, NotifyError, NotifySuccess (+27 more)

### Community 17 - "EtudiantDto"
Cohesion: 0.14
Nodes (12): RIIS.Academic.Application.Etudiants.Services, RIIS.Academic.Application.Etudiants.Dtos, DateOnly, EtudiantDto, CancellationToken, List, Task, EtudiantsService (+4 more)

### Community 18 - "Inscriptions.razor"
Cohesion: 0.05
Nodes (38): ApplyFilter, Cancel, Create, Delete, Edit, Load, LoadLookups, NotifyError (+30 more)

### Community 19 - "ParcoursAcademiques.razor"
Cohesion: 0.05
Nodes (38): Cancel, Create, Delete, Edit, LoadParcours, LoadReferentiels, NotifyError, NotifySuccess (+30 more)

### Community 20 - "TypesElementsScolarite.razor"
Cohesion: 0.05
Nodes (37): CategorieOption, Cancel, CategorieOption, Create, Delete, DisplayCategorie, Edit, Load (+29 more)

### Community 21 - "SemestrePedagogique"
Cohesion: 0.11
Nodes (12): DateTime, ICollection, ProcesVerbal, ProcesVerbalLigne, ICollection, SemestrePedagogique, EntityTypeBuilder, ProcesVerbalConfiguration (+4 more)

### Community 22 - "Releves.razor"
Cohesion: 0.05
Nodes (37): RadzenPanel, ApplySearch, Contains, ExportTemplateWord, ExportWord, FormatCredit, FormatNote, Load (+29 more)

### Community 23 - "RIIS.Academic.Domain"
Cohesion: 0.04
Nodes (25): RIIS.Academic.Application.Evaluations.Dtos, RIIS.Academic.Application.Evaluations.Services, RIIS.Academic.Domain, SaisieNoteLigneDto, DateTime, AuditableEntity, Entity, CanalNotificationScolarite (+17 more)

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
Cohesion: 0.20
Nodes (10): ClassePedagogiqueDto, CancellationToken, IReadOnlyCollection, List, Task, ClassesPedagogiquesService, CancellationToken, List (+2 more)

### Community 28 - "ClassesPedagogiques.razor"
Cohesion: 0.06
Nodes (30): Cancel, Create, Delete, Edit, Load, LoadLookups, NotifyError, NotifySuccess (+22 more)

### Community 29 - "ProcesVerbauxService"
Cohesion: 0.24
Nodes (7): JsonElement, CancellationToken, IReadOnlyCollection, List, Task, ProcesVerbauxService, TypeProcesVerbal

### Community 30 - "Semestres.razor"
Cohesion: 0.06
Nodes (32): Cancel, Create, Delete, Edit, Load, LoadLookups, NotifyError, NotifySuccess (+24 more)

### Community 31 - "_Imports.razor"
Cohesion: 0.06
Nodes (30): Microsoft.AspNetCore.Components, Microsoft.AspNetCore.Components.Routing, Microsoft.AspNetCore.Components.Web, Radzen.Blazor, RIIS.Academic.Application.ClassesPedagogiques.Dtos, RIIS.Academic.Application.ClassesPedagogiques.Services, RIIS.Academic.Application.Common.Dtos, RIIS.Academic.Application.Dashboard.Dtos (+22 more)

### Community 32 - "RiisAcademicDbContext"
Cohesion: 0.08
Nodes (27): DbContext, DbSet, ParcoursPlanItem, IReadOnlyCollection, DateTime, ResultatAnnuel, ICollection, ParcoursAcademique (+19 more)

### Community 33 - "ReferentielsService"
Cohesion: 0.06
Nodes (18): RIIS.Academic.Application.Referentiels.Dtos, RIIS.Academic.Application.Referentiels.Services, AnneeAcademiqueDto, CycleFormationDto, FiliereDto, FiliereLookupDto, NiveauEtudeDto, ParcoursAcademiqueDto (+10 more)

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
Cohesion: 0.09
Nodes (16): RIIS.Academic.Application.Dashboard.Dtos, RIIS.Academic.Application.Dashboard.Services, DateTime, List, DashboardAcademiqueDto, DashboardClasseSyntheseDto, DashboardEcRisqueDto, DashboardEvaluationCompletionDto (+8 more)

### Community 41 - "Core Components"
Cohesion: 0.06
Nodes (33): Academic Decision (DecisionAcademique), Academic Decision Workflow, Academic Validation Status (StatutValidationAcademique), Architecture Overview, Conclusion, Constituent Element Type (TypeElementConstitutif), Core Components, Curriculum Blueprint Status (StatutMaquettePedagogique) (+25 more)

### Community 42 - "ModePaiementScolariteDto"
Cohesion: 0.12
Nodes (13): ModePaiementScolariteDto, CancellationToken, List, Task, IModesPaiementScolariteService, CancellationToken, List, Task (+5 more)

### Community 43 - "TypeElementScolariteDto"
Cohesion: 0.15
Nodes (10): TypeElementScolariteDto, CancellationToken, List, Task, ITypesElementsScolariteService, CancellationToken, List, Task (+2 more)

### Community 44 - "ProcesVerbalExcelExportService"
Cohesion: 0.18
Nodes (8): CancellationToken, IReadOnlyCollection, List, string, Task, ZipArchive, ProcesVerbalExcelExportService, StringBuilder

### Community 45 - "RIIS.Academic.Application.Abstractions.Persistence"
Cohesion: 0.10
Nodes (10): RIIS.Academic.Application.ClassesPedagogiques.Services, RIIS.Academic.Application.Scolarite.Dtos, RIIS.Academic.Application.Programmes.Services, RIIS.Academic.Application.Abstractions.Persistence, RIIS.Academic.Application.ClassesPedagogiques.Dtos, RIIS.Academic.Application.Inscriptions.Dtos, RIIS.Academic.Infrastructure.Persistence.Repositories, RIIS.Academic.Application.Scolarite.Services (+2 more)

### Community 46 - "ReleveDetails.razor"
Cohesion: 0.08
Nodes (24): BackToList, ExportTemplateWord, ExportWord, FormatCredit, FormatNote, GetDecisionBadgeStyle, OnParametersSetAsync, Columns (+16 more)

### Community 47 - "RIIS.Academic.Application.ProcesVerbaux.Dtos"
Cohesion: 0.08
Nodes (19): RIIS.Academic.Application.ProcesVerbaux.Services, RIIS.Academic.Web.Extensions, RIIS.Academic.Application.Releves.Services, RIIS.Academic.Application.ProcesVerbaux.Dtos, RIIS.Academic.Infrastructure.Documents, IEndpointRouteBuilder, ProcesVerbalExcelExportDto, ProcesVerbalWordExportDto (+11 more)

### Community 48 - "ProcesVerbalWordExportService"
Cohesion: 0.14
Nodes (9): CancellationToken, IReadOnlyCollection, Label, List, string, Task, Value, ZipArchive (+1 more)

### Community 49 - "RIIS.Academic.Infrastructure.csproj"
Cohesion: 0.11
Nodes (16): Microsoft.EntityFrameworkCore (10.0.10), Microsoft.EntityFrameworkCore.SqlServer (10.0.10), Microsoft.EntityFrameworkCore.Tools (10.0.10), Radzen.Blazor (11.1.5), net10.0, Microsoft.NET.Sdk.Web, net10.0, Microsoft.NET.Sdk (+8 more)

### Community 50 - "IRepository"
Cohesion: 0.18
Nodes (8): CancellationToken, List, Task, IRepository, CancellationToken, List, Task, EfRepository

### Community 51 - "Detailed Component Analysis"
Cohesion: 0.07
Nodes (29): Academic Path (ParcoursAcademiques), Academic Year (AnneesAcademiques), Admission and Validation, Appendices, Architecture Overview, Backup and Recovery Procedures, Common Queries and Access Patterns, Conclusion (+21 more)

### Community 52 - "ReleveNoteAnnuelDto"
Cohesion: 0.08
Nodes (20): RIIS.Academic.Application.Releves.Dtos, DateOnly, List, ReleveNoteAnnuelDto, ReleveNoteEtudiantDisponibleDto, ReleveNoteLigneDto, ReleveNoteResumeDto, List (+12 more)

### Community 53 - "ProcesVerbalDto"
Cohesion: 0.12
Nodes (11): DateTime, List, ProcesVerbalDto, CancellationToken, IReadOnlyCollection, List, Task, XDocument (+3 more)

### Community 54 - "DossiersScolariteService"
Cohesion: 0.06
Nodes (40): DossierScolariteContext, DossierScolariteSnapshot, DateTime, List, AdministrationDossierScolariteDto, DocumentAdministratifDossierDto, DateOnly, DateTime (+32 more)

### Community 55 - "Student Management APIs"
Cohesion: 0.07
Nodes (26): Architecture Overview, Authentication and Authorization, Base Path and Conventions, Common Use Cases, Conclusion, ContactUrgence (Emergency Contact), Core Components, Create Student (+18 more)

### Community 56 - "Academic Program Services"
Cohesion: 0.07
Nodes (26): Academic Calendar Coordination, Academic Program Services, Architecture Overview, Conclusion, Constituent Element (Element Constitutif), Core Components, Course Unit (Unite Enseignement), Creating a Program Structure (+18 more)

### Community 57 - "SaisieNotesService"
Cohesion: 0.07
Nodes (21): RIIS.Academic.Application.Notes.Services, RIIS.Academic.Application.Abstractions.Services, RIIS.Academic.Application.Notes.Dtos, ICalculNotesService, CancellationToken, Task, IInscriptionService, CancellationToken (+13 more)

### Community 58 - "EvaluationAcademique"
Cohesion: 0.11
Nodes (14): CancellationToken, IReadOnlyCollection, List, Task, RelevesNotesService, DateOnly, ICollection, EvaluationAcademique (+6 more)

### Community 59 - "Database Migrations"
Cohesion: 0.08
Nodes (25): Appendices, Architecture Overview, Conclusion, Connection String Resolution, Core Components, Creating New Migrations, Data Preservation Techniques, Data Reset Utility (+17 more)

### Community 60 - "Testing Strategy"
Cohesion: 0.08
Nodes (25): API Endpoints Testing, Appendices, Application Services Testing, Architecture Overview, Calculation Service Testing, Code Coverage Requirements, Conclusion, Continuous Integration Testing Pipeline (+17 more)

### Community 61 - "ReleveNoteWordExportService"
Cohesion: 0.17
Nodes (8): CancellationToken, IReadOnlyCollection, Label, string, Task, Value, ZipArchive, ReleveNoteWordExportService

### Community 62 - "Detailed Component Analysis"
Cohesion: 0.08
Nodes (23): Adding New Entities, Applying Migrations to Different Environments, Architecture Overview, Conclusion, Core Components, Creating New Migrations, Database Migrations, DbContext and Configuration (+15 more)

### Community 63 - "Dashboard.razor"
Cohesion: 0.15
Nodes (12): DashboardSection, DashboardShortcut, Open, NavigationManager, RadzenButton, RadzenCard, RadzenColumn, RadzenIcon (+4 more)

### Community 64 - "MaquetteElementConstitutif"
Cohesion: 0.16
Nodes (8): DateTime, ResultatElementConstitutif, ICollection, MaquetteElementConstitutif, EntityTypeBuilder, ResultatElementConstitutifConfiguration, EntityTypeBuilder, MaquetteElementConstitutifConfiguration

### Community 65 - "DashboardLayout.razor"
Cohesion: 0.18
Nodes (10): LayoutComponentBase, NavBar, RadzenBody, RadzenHeader, RadzenLayout, RadzenSidebar, RadzenSidebarToggle, RadzenStack (+2 more)

### Community 66 - "Database Persistence"
Cohesion: 0.08
Nodes (23): Architecture Overview, Conclusion, Connection String Management, Core Components, Database Initialization Processes, Database Persistence, Database Schema Design and Entity Mappings, Dependency Analysis (+15 more)

### Community 67 - "InitialCreate"
Cohesion: 0.33
Nodes (3): MigrationBuilder, ModelBuilder, InitialCreate

### Community 68 - "DossierScolarite"
Cohesion: 0.22
Nodes (6): StatutDossierScolarite, DateTime, ICollection, DossierScolarite, EntityTypeBuilder, DossierScolariteConfiguration

### Community 69 - "ModulePlaceholder.razor"
Cohesion: 0.20
Nodes (9): FormatWord, HumanizeSegment, RadzenAlert, RadzenButton, RadzenCard, RadzenIcon, RadzenStack, RadzenText (+1 more)

### Community 70 - "Routes.razor"
Cohesion: 0.40
Nodes (4): FocusOnNavigate, Found, Router, RouteView

### Community 71 - "Academic Programs"
Cohesion: 0.08
Nodes (23): Academic Programs, API Surface Summary, Appendices, Architecture Overview, Assigning Semesters to a Program, Conclusion, Core Components, Creating a Program Blueprint (+15 more)

### Community 72 - "Template Management"
Cohesion: 0.08
Nodes (23): Appendices, Architecture Overview, Backup Procedures, Conclusion, Core Components, Creating New Templates and Adding Placeholders, Dependency Analysis, Deployment Strategies (+15 more)

### Community 73 - "MaquettePedagogique"
Cohesion: 0.25
Nodes (6): DateOnly, DateTime, ICollection, MaquettePedagogique, EntityTypeBuilder, MaquettePedagogiqueConfiguration

### Community 74 - "RIIS.Academic.Web/Program.cs"
Cohesion: 0.50
Nodes (3): RIIS.Academic.Infrastructure, Radzen, RIIS.Academic.Web.Components

### Community 75 - ".InitializeRiisAcademicDatabaseAsync"
Cohesion: 0.25
Nodes (5): RIIS.Academic.Infrastructure.Persistence.Seeders, IServiceProvider, CancellationToken, Task, DatabaseInitializer

### Community 76 - "NavBar.razor"
Cohesion: 0.29
Nodes (6): RadzenPanelMenu, RadzenPanelMenuItem, RadzenCard, RadzenIcon, RadzenStack, RadzenText

### Community 77 - "ValidationInscription"
Cohesion: 0.33
Nodes (4): DateOnly, ValidationInscription, EntityTypeBuilder, ValidationInscriptionConfiguration

### Community 78 - "API Layer"
Cohesion: 0.09
Nodes (22): API Layer, Appendices, Architecture Overview, Common Use Cases, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis (+14 more)

### Community 79 - "Detailed Component Analysis"
Cohesion: 0.09
Nodes (22): Allocation (AffectationPaiementEcheance), Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Document (DocumentElementScolarite), Due Date (EcheanceScolarite) (+14 more)

### Community 80 - "Repository Pattern Implementation"
Cohesion: 0.09
Nodes (22): Advanced Query Methods: Filtering, Sorting, Pagination, Architecture Overview, Conclusion, Core Components, Custom Query Implementations, DashboardAcademiqueService, Dependency Analysis, Dependency Injection and Registration (+14 more)

### Community 81 - ".AddRiisAcademicInfrastructure"
Cohesion: 0.60
Nodes (3): IConfiguration, IServiceCollection, DependencyInjection

### Community 82 - "RIIS.Academic.Infrastructure.Persistence"
Cohesion: 0.19
Nodes (5): RIIS.Academic.Infrastructure.Persistence.Migrations, RIIS.Academic.Infrastructure.Persistence, ModelSnapshot, ModelBuilder, RiisAcademicDbContextModelSnapshot

### Community 83 - ".ExecuteAsync"
Cohesion: 0.70
Nodes (3): CancellationToken, Task, RiisAcademicDatabaseResetter

### Community 84 - "App.razor"
Cohesion: 0.50
Nodes (3): HeadOutlet, RadzenTheme, Routes

### Community 85 - "Dependency Injection & Service Registration"
Cohesion: 0.09
Nodes (22): Adding Middleware or Custom Endpoints, Appendices, Architecture Overview, Conclusion, Core Components, DatabaseInitializer, Dependency Analysis, Dependency Injection & Service Registration (+14 more)

### Community 86 - "Developer Guide"
Cohesion: 0.09
Nodes (22): Appendices, Architecture Overview, Coding Standards and Naming Conventions, Conclusion, Contribution Guidelines and Code Review Process, Core Components, Dependency Analysis, Detailed Component Analysis (+14 more)

### Community 87 - "Student Management"
Cohesion: 0.09
Nodes (22): Architecture Overview, Class Diagram: Etudiant and Related Types, Conclusion, ContactUrgence Value Object, Core Components, Create Student, Delete Student, Dependency Analysis (+14 more)

### Community 88 - "Infrastructure Layer"
Cohesion: 0.09
Nodes (22): Architecture Overview, Conclusion, Configuration Options and Connection String Management, Core Components, Data Seeding Process, Database Schema Mapping and Conventions, Dependency Analysis, Detailed Component Analysis (+14 more)

### Community 89 - "Troubleshooting"
Cohesion: 0.09
Nodes (22): Appendices, Architecture Overview, Common Issues and Step-by-Step Solutions, Conclusion, Configuration Problems, Core Components, Database Connectivity and Initialization, Dependency Analysis (+14 more)

### Community 90 - "Dashboard & Analytics"
Cohesion: 0.09
Nodes (22): Academic Dashboard Page (UI), Architecture Overview, Class Diagram: View Models and DTOs, Conclusion, Core Components, Dashboard & Analytics, Dashboard Service, Data Visualization Components (+14 more)

### Community 91 - "Detailed Component Analysis"
Cohesion: 0.09
Nodes (21): Architecture Overview, Class Diagram: Financial Entities, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, DossierScolarite: Central Financial Record, EcheanceScolarite: Installment Management (+13 more)

### Community 92 - "Detailed Component Analysis"
Cohesion: 0.09
Nodes (21): AffectationPaiementEcheance (Payment Allocation), Architecture Overview, Automated Payment Reminders, Conclusion, Core Components, Creating Payment Schedules, Dependency Analysis, Detailed Component Analysis (+13 more)

### Community 93 - "Evaluation Management"
Cohesion: 0.09
Nodes (21): Appendices, Architecture Overview, Conclusion, Core Components, Creating an Evaluation, Data Models and Relationships, Dependency Analysis, Detailed Component Analysis (+13 more)

### Community 94 - "Reference Data Services"
Cohesion: 0.10
Nodes (20): API Workflows, Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Domain Entities and Relationships, DTOs (+12 more)

### Community 95 - "Enrollment & Registration Services"
Cohesion: 0.10
Nodes (20): Admission Process Management, Approval Processes, Architecture Overview, Capacity Management and Class Assignment, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis (+12 more)

### Community 96 - "Reference Data Services"
Cohesion: 0.10
Nodes (20): Academic Pathway (ParcoursAcademique), Academic Year (AnneeAcademique), Architecture Overview, Conclusion, Core Components, Data Models Diagram, Dependency Analysis, Detailed Component Analysis (+12 more)

### Community 97 - "Registration and Validation"
Cohesion: 0.10
Nodes (20): Academic Validation via ValidationInscription, Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Enrollment Creation and Saving Workflow, Enrollment Lifecycle States (StatutInscription) (+12 more)

### Community 98 - "Document Generation"
Cohesion: 0.10
Nodes (20): Appendices, Architecture Overview, Class Diagram of Export Services, Conclusion, Core Components, Customization Options, Dependency Analysis, Detailed Component Analysis (+12 more)

### Community 99 - "Enrollment System"
Cohesion: 0.10
Nodes (20): Admission Processing Example, Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Document Verification and Enrollment Confirmation, DossierAdmission Management (+12 more)

### Community 100 - "Grade Management"
Cohesion: 0.10
Nodes (20): Appendices, Architecture Overview, Calculation Engine: Coefficients and Averages, Conclusion, Core Components, Data Models Summary, Dependency Analysis, Detailed Component Analysis (+12 more)

### Community 101 - "Student Management"
Cohesion: 0.10
Nodes (20): API Surface Summary, Appendices, Architecture Overview, Conclusion, Core Components, Data Normalization and Validation Rules, Dependency Analysis, Detailed Component Analysis (+12 more)

### Community 102 - "Data Seeding and Initialization"
Cohesion: 0.10
Nodes (20): Appendices, Architecture Overview, Conclusion, Core Components, Creating a New Seeder, Data Seeding and Initialization, DatabaseInitializer, Dependency Analysis (+12 more)

### Community 103 - "Repository Pattern Implementation"
Cohesion: 0.10
Nodes (20): API Surface Summary, Appendices, Architecture Overview, Conclusion, Core Components, Custom Repository Implementations and Advanced Querying, Dependency Analysis, Dependency Injection Registration (+12 more)

### Community 104 - "Document Generation APIs"
Cohesion: 0.10
Nodes (20): API Endpoints Summary, Appendices, Architecture Overview, Authentication and Security, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis (+12 more)

### Community 105 - "Transcript Generation"
Cohesion: 0.10
Nodes (20): Annual Transcript Generation, Appendices, Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Example: Bulk Transcript Generation (+12 more)

### Community 106 - "Financial Overview Dashboard"
Cohesion: 0.10
Nodes (20): Appendices, Architecture Overview, Balance Calculations and Status Visualization, Conclusion, Core Components, Data Models and Relationships, Dependency Analysis, Detailed Component Analysis (+12 more)

### Community 107 - "Enrollment System Domain"
Cohesion: 0.10
Nodes (19): Appendices, Architecture Overview, Conclusion, Core Components, Data Model Summary, Dependency Analysis, Detailed Component Analysis, DossierAdmission Entity (+11 more)

### Community 108 - "Entity Configurations"
Cohesion: 0.10
Nodes (19): Admission Dossier (DossierAdmission), Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Entity Configurations, Etablissements (+11 more)

### Community 109 - "Entity Framework Context"
Cohesion: 0.10
Nodes (19): Architecture Overview, Conclusion, Context Lifecycle Management and Connection Pooling, Core Components, Dependency Analysis, DependencyInjection and Connection String Setup, Detailed Component Analysis, Entity Framework Context (+11 more)

### Community 110 - "External Integrations & Abstractions"
Cohesion: 0.10
Nodes (19): Appendices, Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Error Handling Patterns, External Integrations & Abstractions (+11 more)

### Community 111 - "Configuration & Deployment"
Cohesion: 0.10
Nodes (19): Application Settings and Environment Selection, Architecture Overview, Conclusion, Configuration & Deployment, Connection String Management, Core Components, Database Initialization and Seeding, Dependency Analysis (+11 more)

### Community 112 - "Payment Methods and Handling"
Cohesion: 0.10
Nodes (19): Architecture Overview, Class Diagram: Entities and Relationships, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Financial Summary and Reporting (FinancesScolariteService), Flowchart: Payment Status Resolution (+11 more)

### Community 113 - "Financial Administration"
Cohesion: 0.10
Nodes (19): Architecture Overview, Conclusion, Core Components, Data Model Relationships, Dependency Analysis, Detailed Component Analysis, DossierScolarite Entity, DossiersScolariteService Implementation (+11 more)

### Community 114 - "Entity Framework Context"
Cohesion: 0.10
Nodes (19): Architecture Overview, Common Queries and Data Access Patterns, Conclusion, Connection String Configuration and Environment Settings, Context Lifecycle Management, Core Components, Dependency Analysis, Detailed Component Analysis (+11 more)

### Community 115 - "Excel Export Generation"
Cohesion: 0.10
Nodes (19): Architecture Overview, Batch Export Capabilities, Cell Types and Calculated Columns, Compatibility Across Excel Versions, Conclusion, Core Components, Data Validation Rules, Dependency Analysis (+11 more)

### Community 116 - "Course Units Management"
Cohesion: 0.10
Nodes (19): Appendices, Architecture Overview, Conclusion, Configuration and Constraints, Core Components, Course Units Management, Data Model and Relationships, Dependency Analysis (+11 more)

### Community 117 - "Semester Management"
Cohesion: 0.10
Nodes (19): Architecture Overview, Conclusion, Core Components, Data Model Relationships, Dependency Analysis, Detailed Component Analysis, Integration with Course Unit Assignments, Introduction (+11 more)

### Community 118 - "Student Management UI"
Cohesion: 0.10
Nodes (19): Architecture Overview, Bulk Operations and Export Functionality, Conclusion, Core Components, CRUD Operations, Data Models and Relationships, Dependency Analysis, Detailed Component Analysis (+11 more)

### Community 119 - "Web Application"
Cohesion: 0.10
Nodes (19): Architecture Overview, Conclusion, Core Components, Dashboard Layout and Navigation, Dependency Analysis, Detailed Component Analysis, Export Endpoints, Introduction (+11 more)

### Community 120 - "Enrollment Services"
Cohesion: 0.11
Nodes (18): Admission Processing and Enrollment Confirmation, Architecture Overview, Business Rules: Eligibility, Prerequisites, Capacity, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Domain Model and Lifecycle (+10 more)

### Community 121 - "Student Management Services"
Cohesion: 0.11
Nodes (18): Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Domain Entities and Relationships, EF Configurations and Constraints, Enums (+10 more)

### Community 122 - "Student Management Services"
Cohesion: 0.11
Nodes (18): Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, EtudiantDto Data Transfer Object, EtudiantsService Implementation, IEtudiantsService Interface (+10 more)

### Community 123 - "Domain Enums & Constants"
Cohesion: 0.11
Nodes (18): Academic Results and Decisions (StatutValidationAcademique and DecisionAcademique), Architecture Overview, Assessment Types and Attendance (TypeEvaluation, StatutPresenceEvaluation), Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Domain Enums & Constants (+10 more)

### Community 124 - "Grade & Evaluation Domain"
Cohesion: 0.11
Nodes (18): Architecture Overview, ClassePedagogique, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Evaluation Management Rules, EvaluationAcademique and NoteEvaluation (+10 more)

### Community 125 - "Reference Data Domain"
Cohesion: 0.11
Nodes (18): Academic Year (AnneeAcademique), Architecture Overview, Conclusion, Core Components, Department/Field (Filiere), Dependency Analysis, Detailed Component Analysis, Introduction (+10 more)

### Community 126 - "Student Management Domain"
Cohesion: 0.11
Nodes (18): Architecture Overview, Conclusion, Contact Management Example, ContactUrgence Entity, Core Components, Dependency Analysis, Detailed Component Analysis, Enrollment Lifecycle and State Transitions (+10 more)

### Community 127 - "Document Generation Services"
Cohesion: 0.11
Nodes (18): Appendices, Architecture Overview, Conclusion, Core Components, Customization Guidelines, Dependency Analysis, Detailed Component Analysis, Document Generation Services (+10 more)

### Community 128 - "Domain Layer"
Cohesion: 0.11
Nodes (18): Academic Evaluation (EvaluationAcademique), Academic Program (MaquettePedagogique), Admission and Validation Entities, Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis (+10 more)

### Community 129 - "Fee Structures and Calculations"
Cohesion: 0.11
Nodes (18): Applying Discounts via Tariff Selection, Architecture Overview, Automatic Fee Generation Based on Enrollment, Calculating Total Obligations and Breakdowns, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis (+10 more)

### Community 130 - "Database Persistence"
Cohesion: 0.11
Nodes (18): Architecture Overview, Conclusion, Connection String Management and Environment-Specific Configurations, Core Components, Database Initialization and Data Seeding, Database Persistence, Database Schema Design and Relationships, DbContext and Fluent API Mapping (+10 more)

### Community 131 - "Class Management UI"
Cohesion: 0.11
Nodes (18): Architecture Overview, Bulk Student Assignment, Capacity Management, Class Management UI, Class Performance Monitoring, Class Schedule Management and Conflict Resolution, Class Templates, Conclusion (+10 more)

### Community 132 - "Document Generation UI"
Cohesion: 0.11
Nodes (18): Application Service (PV Queries), Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Document Generation UI, Excel Export (+10 more)

### Community 133 - "Enrollment System UI"
Cohesion: 0.11
Nodes (18): Admission Workflow and Approval Records, Architecture Overview, Batch Enrollment Processing Example, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Document Upload Capabilities (+10 more)

### Community 134 - "Administrative Document Management"
Cohesion: 0.11
Nodes (18): Administrative Document Management, Architecture Overview, Bulk Operations and Batch Synchronization, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Document Validation Workflow (+10 more)

### Community 135 - "Etudiant"
Cohesion: 0.12
Nodes (11): AptitudeMedicale, Sexe, ContactUrgence, DateOnly, DateTime, ICollection, Etudiant, EntityTypeBuilder (+3 more)

### Community 136 - "Academic Program Services"
Cohesion: 0.11
Nodes (17): Academic Calendar Management (Lookups), Academic Program Services, Architecture Overview, Conclusion, Constituent Elements (EC), Core Components, Course Units (UE), Dependency Analysis (+9 more)

### Community 137 - "Application Layer"
Cohesion: 0.11
Nodes (17): Application Layer, Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Enrollment Processing (Inscriptions), Grade Calculation (Notes) (+9 more)

### Community 138 - "Service Architecture & Patterns"
Cohesion: 0.11
Nodes (17): Architecture Overview, Calcul Notes Service (Pure Business Logic), Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Domain Entities and Base Types, Inscription Workflows (Create/Update/Validate) (+9 more)

### Community 139 - "Infrastructure Layer"
Cohesion: 0.11
Nodes (17): Architecture Overview, Conclusion, Connection String Management and Environment-Specific Configuration, Core Components, Dependency Analysis, Dependency Injection Setup, Detailed Component Analysis, Document Generation Services (+9 more)

### Community 140 - "Web & API Layer"
Cohesion: 0.11
Nodes (17): API Project Setup, Application Services Integration, Architecture Overview, Blazor Server Setup and Routing, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis (+9 more)

### Community 141 - "Admission Process"
Cohesion: 0.11
Nodes (17): Administrative Documents and Validations, Admission Process, Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, DossierAdmission Entity (+9 more)

### Community 142 - "Enrollment System"
Cohesion: 0.11
Nodes (17): Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, DossierAdmission Entity, Enrollment System, Enrollment Workflow Examples (+9 more)

### Community 143 - "Grade and Evaluation System"
Cohesion: 0.11
Nodes (17): Academic Standing Determinations, Architecture Overview, ClassePedagogique: Class Groups and Attendance Context, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, EvaluationAcademique and NoteEvaluation (+9 more)

### Community 144 - "Reference Data"
Cohesion: 0.11
Nodes (17): AnneeAcademique (Academic Year), Architecture Overview, Conclusion, Core Components, CycleFormation (Education Cycle), Dependency Analysis, Detailed Component Analysis, Filiere (Field of Study) (+9 more)

### Community 145 - "Feature Modules"
Cohesion: 0.11
Nodes (17): Academic Program Structure, Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Document Generation, Enrollment Workflows (+9 more)

### Community 146 - "Dependency Injection & Configuration"
Cohesion: 0.11
Nodes (17): API Composition, Architecture Overview, Central DI Registration (Infrastructure), Conclusion, Configuration Providers and Environment-Specific Settings, Core Components, DbContext and Persistence, Dependency Analysis (+9 more)

### Community 147 - "Constituent Elements Management"
Cohesion: 0.11
Nodes (17): Architecture Overview, Assessment Methods and Types, Business Rules and Validation (Application Layer), Conclusion, Constituent Elements Interface (Web Layer), Constituent Elements Management, Core Components, Data Model and Constraints (Domain & Infrastructure) (+9 more)

### Community 148 - "Meeting Minutes Management"
Cohesion: 0.11
Nodes (17): Architecture Overview, Conclusion, Core Components, Custom Template Usage for Official Meeting Minutes, Data Grid and Nested Student Details, Decision Tracking System, Dependency Analysis, Detailed Component Analysis (+9 more)

### Community 149 - "Payment Methods Configuration"
Cohesion: 0.11
Nodes (17): Adding New Payment Methods, Architecture Overview, Conclusion, Core Components, Deadlines, Allocations, and Notifications, Dependency Analysis, Detailed Component Analysis, Integrating with External Payment Processors (+9 more)

### Community 150 - "Financial Status Tracking"
Cohesion: 0.11
Nodes (17): Architecture Overview, Calculation Logic for Financial Status and Balances, Conclusion, Core Components, Data Models and Relationships, Dependency Analysis, Detailed Component Analysis, Financial Data Loading and Display via GetFinanceDossierScolariteAsync (+9 more)

### Community 151 - "Student Dossier Management"
Cohesion: 0.11
Nodes (17): Administrative Document Synchronization, Architecture Overview, Bulk Operations and Document Management, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Enrollment Integration and Automatic Dossier Creation (+9 more)

### Community 152 - "Grade Entry Interface"
Cohesion: 0.11
Nodes (17): Architecture Overview, Automatic Result Computation Integration, Bulk Grade Entry Grid Interface, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Grade Entry Interface (+9 more)

### Community 153 - "Export and Document Generation APIs"
Cohesion: 0.12
Nodes (16): Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Endpoint: Annual Transcript Template-Based Word Export, Endpoint: Annual Transcript Word Export, Endpoint: Meeting Minutes Excel Export (+8 more)

### Community 154 - "Program Management APIs"
Cohesion: 0.12
Nodes (16): Architecture Overview, Conclusion, Constituent Element (EC) Endpoints, Core Components, Course Unit (UE) Endpoints, Dependency Analysis, Detailed Component Analysis, Introduction (+8 more)

### Community 155 - "Document Generation Services"
Cohesion: 0.12
Nodes (16): Architecture Overview, Batch Generation Operations, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Document Generation Services, DTOs for Templates, Content Mapping, and Export Formats (+8 more)

### Community 156 - "Dashboard & Analytics Services"
Cohesion: 0.12
Nodes (16): Architecture Overview, Conclusion, Core Components, Dashboard & Analytics Services, Data Aggregation Strategies, Dependency Analysis, Detailed Component Analysis, DTOs and Data Models (+8 more)

### Community 157 - "Document Generation Services"
Cohesion: 0.12
Nodes (16): Architecture Overview, Batch Processing and Export Customization, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Document Generation Services, Formatting Options (+8 more)

### Community 158 - "Academic Programs Domain"
Cohesion: 0.12
Nodes (16): Academic Programs Domain, Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, DTOs and Hierarchy Views, ElementConstitutif (Constituent Element) (+8 more)

### Community 159 - "Domain Layer"
Cohesion: 0.12
Nodes (16): Academic Program Model: MaquettePedagogique, SemestrePedagogique, UniteEnseignement, ElementConstitutif, Architecture Overview, Base Types: Entity and AuditableEntity, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Domain Layer (+8 more)

### Community 160 - "Academic Programs"
Cohesion: 0.12
Nodes (16): Academic Programs, Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, ElementConstitutif (Constituent Element), Enrollment Workflow (Student to Program) (+8 more)

### Community 161 - "Program Hierarchy"
Cohesion: 0.12
Nodes (16): Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Educational Reference Model, Example Workflows and Status Management, Introduction (+8 more)

### Community 162 - "Semester and Course Structure"
Cohesion: 0.12
Nodes (16): Academic Program and Semester Relationship, Architecture Overview, Assessments and Grades Flow, Conclusion, Core Components, Credit Calculation and Workload Definitions, Dependency Analysis, Detailed Component Analysis (+8 more)

### Community 163 - "Student Financial Dossiers"
Cohesion: 0.12
Nodes (16): Administrative Lifecycle and Status Recalculation, Architecture Overview, Conclusion, Core Components, Creating a Financial Dossier from Enrollment, Dependency Analysis, Detailed Component Analysis, DossierScolarite Entity and Status Model (+8 more)

### Community 164 - "Result Aggregation"
Cohesion: 0.12
Nodes (16): Academic Standing and Decisions, Annual Results, Architecture Overview, Conclusion, Constituent Element Results, Core Components, Dependency Analysis, Detailed Component Analysis (+8 more)

### Community 165 - "Document Generation"
Cohesion: 0.12
Nodes (16): Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Document Generation, Excel Export (Proces Verbal), Introduction (+8 more)

### Community 166 - "Project Overview"
Cohesion: 0.12
Nodes (16): Academic Programs and Curricula, Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Financial Administration, Grade Management (+8 more)

### Community 167 - "Financial Elements Types"
Cohesion: 0.12
Nodes (16): Appendices, Architecture Overview, Conclusion, Configuration Examples, Core Components, Dependency Analysis, Detailed Component Analysis, Element Types Management (+8 more)

### Community 168 - "Tuition Fee Management"
Cohesion: 0.12
Nodes (16): Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Historical Fee Tracking and Validity, Integration with Financial Calculation Engines, Introduction (+8 more)

### Community 169 - "Evaluations Management"
Cohesion: 0.12
Nodes (16): Architecture Overview, Conclusion, Core Components, Creation Workflow, Data Grid Functionality, Dependency Analysis, Detailed Component Analysis, Evaluation Types and Configurations (+8 more)

### Community 170 - "Financial Administration APIs"
Cohesion: 0.12
Nodes (15): Architecture Overview, Conclusion, Core Components, Data Models and Relationships, Dependency Analysis, Detailed Component Analysis, Fee Structure Management (Tariffs), Financial Administration APIs (+7 more)

### Community 171 - "Financial Administration Services"
Cohesion: 0.12
Nodes (15): Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, DossiersScolariteService, FinancesScolariteService, Financial Administration Services (+7 more)

### Community 172 - "Grade Calculation Services"
Cohesion: 0.12
Nodes (15): Architecture Overview, CalculNotesService: Grade Computation Algorithms, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, DTOs: SaisieNotesGrilleDto and SaisieNoteLigneDto, Evaluation Types, Scoring Systems, and Academic Performance Metrics (+7 more)

### Community 173 - "Application Layer"
Cohesion: 0.12
Nodes (15): Application Layer, Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Enrollment Processing (Inscriptions), Grade Calculation Workflow (Notes) (+7 more)

### Community 174 - "Financial Administration Services"
Cohesion: 0.12
Nodes (15): Architecture Overview, Conclusion, Core Components, Data Models and Relationships, Dependency Analysis, Detailed Component Analysis, Financial Administration Services, IDossiersScolariteService and DossiersScolariteService (+7 more)

### Community 175 - "Grade & Evaluation Services"
Cohesion: 0.12
Nodes (15): Architecture Overview, CalculNotesService: Grade Calculation and Aggregation, Conclusion, Core Components, Data Models and Relationships, Dependency Analysis, Detailed Component Analysis, Grade & Evaluation Services (+7 more)

### Community 176 - "Architecture Guide"
Cohesion: 0.12
Nodes (15): Architecture Guide, Architecture Overview, Class Model (Domain and Application), Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Export Endpoints (Integration Pattern) (+7 more)

### Community 177 - "Core Entities & Base Classes"
Cohesion: 0.12
Nodes (15): Architecture Overview, AuditableEntity, Conclusion, Core Components, Core Entities & Base Classes, Dependency Analysis, Detailed Component Analysis, Entity (+7 more)

### Community 178 - "Supporting Entities"
Cohesion: 0.12
Nodes (15): Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Etablissement (Institution), Introduction, Official Document Generation (Word/Excel) (+7 more)

### Community 179 - "Core Entities"
Cohesion: 0.12
Nodes (15): Architecture Overview, AuditableEntity base class, Conclusion, Concrete entities and shared functionality, Core Components, Core Entities, Dependency Analysis, Detailed Component Analysis (+7 more)

### Community 180 - "Word Document Generation"
Cohesion: 0.12
Nodes (15): Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Introduction, Performance Considerations, Programmatic Process Verbal Export (+7 more)

### Community 181 - "Academic Programs UI"
Cohesion: 0.12
Nodes (15): Academic Programs UI, Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, ElementsConstitutifs (Constituent Elements), Introduction (+7 more)

### Community 182 - "Program Maquettes Management"
Cohesion: 0.12
Nodes (15): Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Hierarchy DTOs, Introduction, Maquettes Page (Curriculum Versions) (+7 more)

### Community 183 - "Financial Administration UI"
Cohesion: 0.12
Nodes (15): Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Financial Administration UI, Financial Dashboard (FinancesScolarite.razor), Financial Status Tracking (DossiersScolariteService) (+7 more)

### Community 184 - "Dossier Creation Workflow"
Cohesion: 0.12
Nodes (15): Administrative Documents and Status Recalculation, Architecture Overview, Conclusion, Core Components, Data Flow: From Enrollment to Complete Financial Dossier, Dependency Analysis, Detailed Component Analysis, Dossier Creation Workflow (+7 more)

### Community 185 - "Grade Management UI"
Cohesion: 0.12
Nodes (15): Academic Result Visualization, Architecture Overview, Bulk Grade Entry Interface, Calculation Engine and Weighted Aggregation, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis (+7 more)

### Community 186 - "Enrollment APIs"
Cohesion: 0.13
Nodes (14): Architecture Overview, Conclusion, Core Components, Data Model Relationships, Dependency Analysis, Detailed Component Analysis, Enrollment APIs, Enrollment Lifecycle Endpoints (+6 more)

### Community 187 - "Grade Management APIs"
Cohesion: 0.13
Nodes (14): Architecture Overview, Conclusion, Core Components, Dependency Analysis, Detailed Component Analysis, Evaluation Management Endpoints, Grade Entry Endpoints, Grade Management APIs (+6 more)

### Community 188 - "Dashboard & Analytics Services"
Cohesion: 0.13
Nodes (14): Architecture Overview, Conclusion, Core Components, Dashboard & Analytics Services, DashboardAcademiqueService, Dependency Analysis, Detailed Component Analysis, DTOs and Models (+6 more)

### Community 189 - "Getting Started"
Cohesion: 0.15
Nodes (12): Conclusion, Database Setup and Configuration, Environment Variables and Connection Strings, First Run Instructions, Getting Started, Installation Steps, Introduction, Prerequisites (+4 more)

### Community 190 - "EcheanceScolarite"
Cohesion: 0.17
Nodes (9): DateOnly, EcheanceFinanciereDossierDto, StatutEcheanceScolarite, DateOnly, DateTime, ICollection, EcheanceScolarite, EntityTypeBuilder (+1 more)

### Community 191 - "FinanceDossierScolariteDto.cs"
Cohesion: 0.25
Nodes (6): ElementFinancierDossierDto, PaiementTypeElementTarifOptionDto, CancellationToken, List, Task, IFinancesScolariteService

### Community 192 - "PaiementScolarite"
Cohesion: 0.20
Nodes (7): StatutPaiementScolarite, DateOnly, DateTime, ICollection, PaiementScolarite, EntityTypeBuilder, PaiementScolariteConfiguration

### Community 193 - "IProcesVerbauxService"
Cohesion: 0.51
Nodes (4): CancellationToken, List, Task, IProcesVerbauxService

### Community 194 - ".ToAffectationDto"
Cohesion: 0.22
Nodes (6): DateTime, AffectationPaiementDossierDto, DateTime, AffectationPaiementEcheance, EntityTypeBuilder, AffectationPaiementEcheanceConfiguration

### Community 195 - "ITarifsScolariteService"
Cohesion: 0.38
Nodes (4): CancellationToken, List, Task, ITarifsScolariteService

### Community 196 - "RefactorTarifScolariteParcours"
Cohesion: 0.25
Nodes (4): Migration, MigrationBuilder, ModelBuilder, RefactorTarifScolariteParcours

### Community 197 - "RemoveTarifValiditeDates"
Cohesion: 0.29
Nodes (3): MigrationBuilder, ModelBuilder, RemoveTarifValiditeDates

### Community 198 - "ResultatSemestre"
Cohesion: 0.33
Nodes (4): DateTime, ResultatSemestre, EntityTypeBuilder, ResultatSemestreConfiguration

### Community 199 - "ResultatUniteEnseignement"
Cohesion: 0.33
Nodes (4): DateTime, ResultatUniteEnseignement, EntityTypeBuilder, ResultatUniteEnseignementConfiguration

### Community 200 - "AddRefonteQoder"
Cohesion: 0.33
Nodes (3): MigrationBuilder, ModelBuilder, AddRefonteQoder

### Community 201 - "RefactorClassePedagogique"
Cohesion: 0.33
Nodes (3): MigrationBuilder, ModelBuilder, RefactorClassePedagogique

### Community 202 - "ProcesVerbalElementConstitutifLigneDto"
Cohesion: 0.33
Nodes (3): ProcesVerbalElementConstitutifLigneDto, List, ProcesVerbalLigneDto

## Knowledge Gaps
- **2912 isolated node(s):** `net10.0`, `Microsoft.NET.Sdk.Web`, `GroupeDashboard`, `DashboardGroupeKey`, `DashboardData` (+2907 more)
  These have ≤1 connection - possible missing edges or undocumented components.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `RIIS.Academic.Domain` connect `RIIS.Academic.Domain` to `LookupDto`, `RIIS.Academic.Infrastructure.Persistence.Configurations`, `Etudiant`, `EvaluationsService`, `EtudiantDto`, `SemestrePedagogique`, `ProcesVerbauxService`, `RiisAcademicDbContext`, `ReferentielsService`, `DashboardAcademiqueDto`, `ModePaiementScolariteDto`, `TypeElementScolariteDto`, `RIIS.Academic.Application.Abstractions.Persistence`, `RIIS.Academic.Application.ProcesVerbaux.Dtos`, `ProcesVerbalDto`, `DossiersScolariteService`, `SaisieNotesService`, `EvaluationAcademique`, `EcheanceScolarite`, `FinanceDossierScolariteDto.cs`, `PaiementScolarite`, `MaquetteElementConstitutif`, `.ToAffectationDto`, `DossierScolarite`, `ResultatSemestre`, `ResultatUniteEnseignement`, `MaquettePedagogique`, `ProcesVerbalElementConstitutifLigneDto`, `.InitializeRiisAcademicDatabaseAsync`, `ValidationInscription`, `RIIS.Academic.Infrastructure.Persistence`?**
  _High betweenness centrality (0.033) - this node is a cross-community bridge._
- **Why does `RiisAcademicDbContext` connect `RiisAcademicDbContext` to `LookupDto`, `Etudiant`, `Inscription`, `SemestrePedagogique`, `RIIS.Academic.Domain`, `ModePaiementScolariteDto`, `DossiersScolariteService`, `EvaluationAcademique`, `EcheanceScolarite`, `MaquetteElementConstitutif`, `PaiementScolarite`, `.ToAffectationDto`, `DossierScolarite`, `ResultatSemestre`, `ResultatUniteEnseignement`, `MaquettePedagogique`, `ValidationInscription`, `RIIS.Academic.Infrastructure.Persistence`, `.ExecuteAsync`?**
  _High betweenness centrality (0.012) - this node is a cross-community bridge._
- **Why does `RIIS.Academic.Application.Abstractions.Persistence` connect `RIIS.Academic.Application.Abstractions.Persistence` to `ReferentielsService`, `DashboardAcademiqueDto`, `RIIS.Academic.Application.ProcesVerbaux.Dtos`, `EtudiantDto`, `IRepository`, `RIIS.Academic.Domain`, `SaisieNotesService`?**
  _High betweenness centrality (0.007) - this node is a cross-community bridge._
- **What connects `net10.0`, `Microsoft.NET.Sdk.Web`, `GroupeDashboard` to the rest of the system?**
  _2912 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `LookupDto` be split into smaller, more focused modules?**
  _Cohesion score 0.05744449619624282 - nodes in this community are weakly interconnected._
- **Should `InscriptionsService` be split into smaller, more focused modules?**
  _Cohesion score 0.14634146341463414 - nodes in this community are weakly interconnected._
- **Should `DashboardAcademique.razor` be split into smaller, more focused modules?**
  _Cohesion score 0.03125 - nodes in this community are weakly interconnected._