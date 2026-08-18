namespace RIIS.Academic.Domain;

public class ElementScolariteEtudiant
{
    public long Id { get; set; }
    public long DossierScolariteId { get; set; }
    public long TypeElementScolariteId { get; set; }
    public required string Code { get; set; }
    public required string Libelle { get; set; }
    public decimal MontantAttendu { get; set; }
    public decimal MontantAffecte { get; set; }
    public decimal MontantRestant { get; set; }
    public StatutElementScolarite Statut { get; set; } = StatutElementScolarite.NonDemarre;
    public DateTime DateCreationUtc { get; set; } = DateTime.UtcNow;
    public DateTime? DateDernierRecalculUtc { get; set; }
    public string? Observation { get; set; }

    public DossierScolarite DossierScolarite { get; set; } = null!;
    public TypeElementScolarite TypeElementScolarite { get; set; } = null!;
    public ICollection<EcheanceScolarite> Echeances { get; set; } = [];
    public ICollection<PaiementScolarite> Paiements { get; set; } = [];
    public ICollection<DocumentElementScolarite> Documents { get; set; } = [];
    public ICollection<ValidationElementScolarite> Validations { get; set; } = [];
    public ICollection<NotificationScolarite> Notifications { get; set; } = [];
}
