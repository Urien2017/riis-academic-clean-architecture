namespace RIIS.Academic.Domain;

public class ResultatSemestre
{
    public long Id { get; set; }
    public long InscriptionId { get; set; }
    public long SemestrePedagogiqueId { get; set; }
    public decimal? MoyenneControleContinu { get; set; }
    public decimal? MoyenneControleConnaissance { get; set; }
    public decimal? MoyenneSessionNormale { get; set; }
    public decimal? MoyenneSessionRattrapage { get; set; }
    public decimal? MoyenneSemestrielle { get; set; }
    public decimal CreditsAcquis { get; set; }
    public decimal CreditsRequis { get; set; } = 30m;
    public int? Rang { get; set; }
    public StatutValidationAcademique StatutValidation { get; set; } = StatutValidationAcademique.NonCalcule;
    public DecisionAcademique DecisionJury { get; set; } = DecisionAcademique.NonDeliberee;
    public DateTime CalculeLeUtc { get; set; } = DateTime.UtcNow;

    public Inscription Inscription { get; set; } = null!;
    public SemestrePedagogique SemestrePedagogique { get; set; } = null!;
}
