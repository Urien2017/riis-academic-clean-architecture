namespace RIIS.Academic.Domain;

public class ResultatUniteEnseignement
{
    public long Id { get; set; }
    public long InscriptionId { get; set; }
    public long UniteEnseignementId { get; set; }
    public decimal? Moyenne { get; set; }
    public decimal CreditsAcquis { get; set; }
    public decimal CreditsAttendus { get; set; }
    public StatutValidationAcademique StatutValidation { get; set; } = StatutValidationAcademique.NonCalcule;
    public DateTime CalculeLeUtc { get; set; } = DateTime.UtcNow;

    public Inscription Inscription { get; set; } = null!;
    public UniteEnseignement UniteEnseignement { get; set; } = null!;
}
