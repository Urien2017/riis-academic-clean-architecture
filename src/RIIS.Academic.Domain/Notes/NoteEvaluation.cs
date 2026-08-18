namespace RIIS.Academic.Domain;

public class NoteEvaluation
{
    public long Id { get; set; }
    public long EvaluationAcademiqueId { get; set; }
    public long InscriptionId { get; set; }
    public decimal? Valeur { get; set; }
    public StatutPresenceEvaluation StatutPresence { get; set; } = StatutPresenceEvaluation.Present;
    public string? Observation { get; set; }
    public DateTime SaisieLeUtc { get; set; } = DateTime.UtcNow;
    public string? SaisiePar { get; set; }

    public EvaluationAcademique EvaluationAcademique { get; set; } = null!;
    public Inscription Inscription { get; set; } = null!;
}
