namespace RIIS.Academic.Domain;

public class ResultatElementConstitutif
{
    public long Id { get; set; }
    public long InscriptionId { get; set; }
    public long ElementConstitutifId { get; set; }
    public decimal? MoyenneControleContinu { get; set; }
    public decimal? MoyenneControleConnaissance { get; set; }
    public decimal? MoyenneSessionNormale { get; set; }
    public decimal? MoyenneSessionRattrapage { get; set; }
    public decimal? MoyenneAvantRattrapage { get; set; }
    public decimal? MoyenneApresRattrapage { get; set; }
    public decimal? MoyenneRetenue { get; set; }
    public decimal CreditsAcquis { get; set; }
    public StatutValidationAcademique StatutValidation { get; set; } = StatutValidationAcademique.NonCalcule;
    public bool EstEligibleRattrapage { get; set; }
    public DateTime CalculeLeUtc { get; set; } = DateTime.UtcNow;

    public Inscription Inscription { get; set; } = null!;
    public ElementConstitutif ElementConstitutif { get; set; } = null!;
}
