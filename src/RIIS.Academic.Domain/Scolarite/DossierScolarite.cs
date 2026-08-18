namespace RIIS.Academic.Domain;

public class DossierScolarite
{
    public long Id { get; set; }
    public long InscriptionId { get; set; }
    public string AnneeAcademiqueCode { get; set; } = string.Empty;
    public string AnneeAcademiqueLibelle { get; set; } = string.Empty;
    public string CycleCode { get; set; } = string.Empty;
    public string CycleLibelle { get; set; } = string.Empty;
    public string FiliereCode { get; set; } = string.Empty;
    public string FiliereLibelle { get; set; } = string.Empty;
    public string? SpecialiteCode { get; set; }
    public string? SpecialiteLibelle { get; set; }
    public int NiveauNumero { get; set; }
    public string NiveauLibelle { get; set; } = string.Empty;
    public StatutDossierScolarite StatutAdministratif { get; set; } = StatutDossierScolarite.EnPreparation;
    public StatutDossierScolarite StatutFinancier { get; set; } = StatutDossierScolarite.EnPreparation;
    public StatutDossierScolarite StatutGlobal { get; set; } = StatutDossierScolarite.EnPreparation;
    public DateTime DateCreationUtc { get; set; } = DateTime.UtcNow;
    public DateTime? DateDernierRecalculUtc { get; set; }
    public string? Observation { get; set; }

    public Inscription Inscription { get; set; } = null!;
    public ICollection<ElementScolariteEtudiant> ElementsScolarite { get; set; } = [];
    public ICollection<PaiementScolarite> Paiements { get; set; } = [];
    public ICollection<NotificationScolarite> Notifications { get; set; } = [];
}
