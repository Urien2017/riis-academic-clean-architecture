namespace RIIS.Academic.Domain;

public class NotificationScolarite
{
    public long Id { get; set; }
    public long DossierScolariteId { get; set; }
    public long? ElementScolariteEtudiantId { get; set; }
    public long? EcheanceScolariteId { get; set; }
    public TypeNotificationScolarite Type { get; set; }
    public CanalNotificationScolarite Canal { get; set; } = CanalNotificationScolarite.Interne;
    public required string Titre { get; set; }
    public required string Message { get; set; }
    public DateTime DateGenerationUtc { get; set; } = DateTime.UtcNow;
    public DateTime? DateEnvoiUtc { get; set; }
    public StatutNotificationScolarite Statut { get; set; } = StatutNotificationScolarite.Generee;

    public DossierScolarite DossierScolarite { get; set; } = null!;
    public ElementScolariteEtudiant? ElementScolariteEtudiant { get; set; }
    public EcheanceScolarite? EcheanceScolarite { get; set; }
}
