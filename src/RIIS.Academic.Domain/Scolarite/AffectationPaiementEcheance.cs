namespace RIIS.Academic.Domain;

public class AffectationPaiementEcheance
{
    public long Id { get; set; }
    public long PaiementScolariteId { get; set; }
    public long EcheanceScolariteId { get; set; }
    public decimal MontantAffecte { get; set; }
    public DateTime DateAffectationUtc { get; set; } = DateTime.UtcNow;
    public string? AffectePar { get; set; }

    public PaiementScolarite PaiementScolarite { get; set; } = null!;
    public EcheanceScolarite EcheanceScolarite { get; set; } = null!;
}
