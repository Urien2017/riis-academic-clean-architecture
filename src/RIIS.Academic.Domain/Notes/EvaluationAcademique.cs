namespace RIIS.Academic.Domain;

public class EvaluationAcademique
{
    public long Id { get; set; }
    public long AnneeAcademiqueId { get; set; }
    public long MaquetteElementConstitutifId { get; set; }
    public TypeEvaluation Type { get; set; }
    public byte Numero { get; set; } = 1;
    public required string Code { get; set; }
    public required string Libelle { get; set; }
    public decimal Bareme { get; set; } = 20m;
    public decimal PonderationPourcentage { get; set; }
    public DateOnly? DateEvaluation { get; set; }
    public long? EvaluationRemplaceeId { get; set; }
    public string? Observation { get; set; }

    public AnneeAcademique AnneeAcademique { get; set; } = null!;
    public MaquetteElementConstitutif MaquetteElementConstitutif { get; set; } = null!;
    public EvaluationAcademique? EvaluationRemplacee { get; set; }
    public ICollection<EvaluationAcademique> EvaluationsDeRattrapage { get; set; } = [];
    public ICollection<NoteEvaluation> Notes { get; set; } = [];
}
