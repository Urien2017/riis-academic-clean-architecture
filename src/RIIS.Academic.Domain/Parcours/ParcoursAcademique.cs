namespace RIIS.Academic.Domain;

public class ParcoursAcademique
{
    public long Id { get; set; }
    public long AnneeAcademiqueId { get; set; }
    public long CycleFormationId { get; set; }
    public long NiveauEtudeId { get; set; }
    public long FiliereId { get; set; }
    public long SpecialiteId { get; set; }
    public required string Code { get; set; }
    public required string Libelle { get; set; }
    public bool EstActive { get; set; } = true;

    public AnneeAcademique AnneeAcademique { get; set; } = null!;
    public CycleFormation CycleFormation { get; set; } = null!;
    public NiveauEtude NiveauEtude { get; set; } = null!;
    public Filiere Filiere { get; set; } = null!;
    public Specialite Specialite { get; set; } = null!;
    public ICollection<Inscription> Inscriptions { get; set; } = [];
    public ICollection<ClassePedagogique> ClassesPedagogiques { get; set; } = [];
    public ICollection<ProcesVerbal> ProcesVerbaux { get; set; } = [];
    public ICollection<MaquettePedagogique> MaquettesPedagogiques { get; set; } = [];
}
