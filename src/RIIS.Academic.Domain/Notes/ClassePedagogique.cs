namespace RIIS.Academic.Domain;

public class ClassePedagogique
{
    public long Id { get; set; }
    public long AnneeAcademiqueId { get; set; }
    public long ParcoursAcademiqueId { get; set; }
    public long NiveauEtudeId { get; set; }
    public long? MaquettePedagogiqueId { get; set; }
    public required string Code { get; set; }
    public required string Libelle { get; set; }
    public bool EstActive { get; set; } = true;

    public AnneeAcademique AnneeAcademique { get; set; } = null!;
    public ParcoursAcademique ParcoursAcademique { get; set; } = null!;
    public NiveauEtude NiveauEtude { get; set; } = null!;
    public MaquettePedagogique? MaquettePedagogique { get; set; }
    public ICollection<Inscription> Inscriptions { get; set; } = [];
    public ICollection<ProcesVerbal> ProcesVerbaux { get; set; } = [];
}
