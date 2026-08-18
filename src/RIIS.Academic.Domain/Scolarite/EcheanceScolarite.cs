namespace RIIS.Academic.Domain;

public class EcheanceScolarite
{
    public long Id { get; set; }
    public long ElementScolariteEtudiantId { get; set; }
    public int Numero { get; set; }
    public required string Libelle { get; set; }
    public DateOnly DateExigibilite { get; set; }
    public decimal MontantAttendu { get; set; }
    public decimal MontantAffecte { get; set; }
    public decimal MontantRestant { get; set; }
    public StatutEcheanceScolarite Statut { get; set; } = StatutEcheanceScolarite.NonExigible;
    public DateTime? DateDernierRecalculUtc { get; set; }

    public ElementScolariteEtudiant ElementScolariteEtudiant { get; set; } = null!;
    public ICollection<AffectationPaiementEcheance> Affectations { get; set; } = [];
    public ICollection<NotificationScolarite> Notifications { get; set; } = [];
}
