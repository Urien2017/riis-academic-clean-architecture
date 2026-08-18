namespace RIIS.Academic.Domain;

public class DocumentElementScolarite
{
    public long Id { get; set; }
    public long ElementScolariteEtudiantId { get; set; }
    public required string NomDocument { get; set; }
    public string? UrlFichier { get; set; }
    public StatutDocumentScolarite Statut { get; set; } = StatutDocumentScolarite.EnAttente;
    public DateTime? DateDepotUtc { get; set; }
    public DateTime? DateVerificationUtc { get; set; }
    public string? VerifiePar { get; set; }
    public string? Observation { get; set; }

    public ElementScolariteEtudiant ElementScolariteEtudiant { get; set; } = null!;
}
