namespace RIIS.Academic.Domain;

public class ResultatAnnuel
{
    public long Id { get; set; }
    public long InscriptionId { get; set; }
    public long AnneeAcademiqueId { get; set; }
    public long NiveauEtudeId { get; set; }
    public decimal? MoyenneAnnuelle { get; set; }
    public decimal CreditsAcquis { get; set; }
    public decimal CreditsRequis { get; set; } = 60m;
    public int? Rang { get; set; }
    public StatutValidationAcademique StatutValidation { get; set; } = StatutValidationAcademique.NonCalcule;
    public DecisionAcademique DecisionJury { get; set; } = DecisionAcademique.NonDeliberee;
    public DateTime CalculeLeUtc { get; set; } = DateTime.UtcNow;

    public Inscription Inscription { get; set; } = null!;
    public AnneeAcademique AnneeAcademique { get; set; } = null!;
    public NiveauEtude NiveauEtude { get; set; } = null!;
}
