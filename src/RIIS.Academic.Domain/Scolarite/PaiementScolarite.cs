namespace RIIS.Academic.Domain;

public class PaiementScolarite
{
    public long Id { get; set; }
    public long DossierScolariteId { get; set; }
    public long? ElementScolariteEtudiantId { get; set; }
    public long? TypeElementScolariteId { get; set; }
    public long? TarifScolariteId { get; set; }
    public long? ModePaiementScolariteId { get; set; }
    public DateOnly DatePaiement { get; set; }
    public decimal Montant { get; set; }
    public required string ModePaiement { get; set; }
    public string? ReferencePaiement { get; set; }
    public string? EncaissePar { get; set; }
    public string? Observation { get; set; }
    public decimal MontantAffecte { get; set; }
    public decimal MontantNonAffecte { get; set; }
    public StatutPaiementScolarite Statut { get; set; } = StatutPaiementScolarite.NonAffecte;
    public DateTime DateCreationUtc { get; set; } = DateTime.UtcNow;

    public DossierScolarite DossierScolarite { get; set; } = null!;
    public ElementScolariteEtudiant? ElementScolariteEtudiant { get; set; }
    public TypeElementScolarite? TypeElementScolarite { get; set; }
    public TarifScolarite? TarifScolarite { get; set; }
    public ModePaiementScolarite? ModePaiementScolarite { get; set; }
    public ICollection<AffectationPaiementEcheance> Affectations { get; set; } = [];
}
