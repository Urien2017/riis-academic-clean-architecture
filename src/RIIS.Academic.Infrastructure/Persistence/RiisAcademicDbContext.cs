using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;

namespace RIIS.Academic.Infrastructure.Persistence;

public class RiisAcademicDbContext(DbContextOptions<RiisAcademicDbContext> options)
    : DbContext(options)
{
    public DbSet<Etablissement> Etablissements => Set<Etablissement>();
    public DbSet<Etudiant> Etudiants => Set<Etudiant>();
    public DbSet<ContactUrgence> ContactsUrgence => Set<ContactUrgence>();
    public DbSet<AnneeAcademique> AnneesAcademiques => Set<AnneeAcademique>();
    public DbSet<CycleFormation> CyclesFormation => Set<CycleFormation>();
    public DbSet<ParcoursAcademique> ParcoursAcademiques => Set<ParcoursAcademique>();
    public DbSet<NiveauEtude> NiveauxEtude => Set<NiveauEtude>();
    public DbSet<Filiere> Filieres => Set<Filiere>();
    public DbSet<Specialite> Specialites => Set<Specialite>();
    public DbSet<MaquettePedagogique> MaquettesPedagogiques => Set<MaquettePedagogique>();
    public DbSet<SemestrePedagogique> SemestresPedagogiques => Set<SemestrePedagogique>();
    public DbSet<UniteEnseignement> UnitesEnseignement => Set<UniteEnseignement>();
    public DbSet<ElementConstitutif> ElementsConstitutifs => Set<ElementConstitutif>();
    public DbSet<ClassePedagogique> ClassesPedagogiques => Set<ClassePedagogique>();
    public DbSet<EvaluationAcademique> EvaluationsAcademiques => Set<EvaluationAcademique>();
    public DbSet<NoteEvaluation> NotesEvaluations => Set<NoteEvaluation>();
    public DbSet<ResultatElementConstitutif> ResultatsElementsConstitutifs => Set<ResultatElementConstitutif>();
    public DbSet<ResultatUniteEnseignement> ResultatsUnitesEnseignement => Set<ResultatUniteEnseignement>();
    public DbSet<ResultatSemestre> ResultatsSemestres => Set<ResultatSemestre>();
    public DbSet<ResultatAnnuel> ResultatsAnnuels => Set<ResultatAnnuel>();
    public DbSet<ProcesVerbal> ProcesVerbaux => Set<ProcesVerbal>();
    public DbSet<ProcesVerbalLigne> ProcesVerbauxLignes => Set<ProcesVerbalLigne>();
    public DbSet<Inscription> Inscriptions => Set<Inscription>();
    public DbSet<DossierAdmission> DossiersAdmission => Set<DossierAdmission>();
    public DbSet<ValidationInscription> ValidationsInscriptions => Set<ValidationInscription>();
    public DbSet<DossierScolarite> DossiersScolarite => Set<DossierScolarite>();
    public DbSet<TypeElementScolarite> TypesElementsScolarite => Set<TypeElementScolarite>();
    public DbSet<ModePaiementScolarite> ModesPaiementScolarite => Set<ModePaiementScolarite>();
    public DbSet<TarifScolarite> TarifsScolarite => Set<TarifScolarite>();
    public DbSet<ElementScolariteEtudiant> ElementsScolariteEtudiants => Set<ElementScolariteEtudiant>();
    public DbSet<EcheanceScolarite> EcheancesScolarite => Set<EcheanceScolarite>();
    public DbSet<PaiementScolarite> PaiementsScolarite => Set<PaiementScolarite>();
    public DbSet<AffectationPaiementEcheance> AffectationsPaiementsEcheances => Set<AffectationPaiementEcheance>();
    public DbSet<DocumentElementScolarite> DocumentsElementsScolarite => Set<DocumentElementScolarite>();
    public DbSet<ValidationElementScolarite> ValidationsElementsScolarite => Set<ValidationElementScolarite>();
    public DbSet<NotificationScolarite> NotificationsScolarite => Set<NotificationScolarite>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RiisAcademicDbContext).Assembly);
    }
}
