namespace RIIS.Academic.Domain;

public class Inscription
{
    public long Id { get; set; }
    public long AnneeAcademiqueId { get; set; }
    public long EtudiantId { get; set; }
    public long ParcoursAcademiqueId { get; set; }
    public long NiveauEtudeId { get; set; }
    public long? MaquettePedagogiqueId { get; set; }
    public long? ClassePedagogiqueId { get; set; }
    public DateOnly DateInscription { get; set; }
    public StatutInscription Statut { get; set; } = StatutInscription.EnAttente;
    public string? MentionSpeciale { get; set; }
    public string? TutelleAcademique { get; set; }
    public string? Observation { get; set; }
    public string? CodeAdministration { get; set; }
    public DateTime CreeLeUtc { get; set; } = DateTime.UtcNow;
    public byte[] Version { get; set; } = [];

    public AnneeAcademique AnneeAcademique { get; set; } = null!;
    public Etudiant Etudiant { get; set; } = null!;
    public ParcoursAcademique ParcoursAcademique { get; set; } = null!;
    public NiveauEtude NiveauEtude { get; set; } = null!;
    public MaquettePedagogique? MaquettePedagogique { get; set; }
    public ClassePedagogique? ClassePedagogique { get; set; }
    public DossierAdmission? DossierAdmission { get; set; }
    public ValidationInscription? ValidationInscription { get; set; }
    public DossierScolarite? DossierScolarite { get; set; }
    public ICollection<NoteEvaluation> NotesEvaluations { get; set; } = [];
    public ICollection<ResultatElementConstitutif> ResultatsElementsConstitutifs { get; set; } = [];
    public ICollection<ResultatUniteEnseignement> ResultatsUnitesEnseignement { get; set; } = [];
    public ICollection<ResultatSemestre> ResultatsSemestres { get; set; } = [];
    public ICollection<ResultatAnnuel> ResultatsAnnuels { get; set; } = [];
    public ICollection<ProcesVerbalLigne> LignesProcesVerbaux { get; set; } = [];
}
