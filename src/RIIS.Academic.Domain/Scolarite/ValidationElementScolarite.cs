namespace RIIS.Academic.Domain;

public class ValidationElementScolarite
{
    public long Id { get; set; }
    public long ElementScolariteEtudiantId { get; set; }
    public StatutValidationScolarite Statut { get; set; } = StatutValidationScolarite.EnAttente;
    public DateTime? DateValidationUtc { get; set; }
    public string? ValidePar { get; set; }
    public string? MotifRejet { get; set; }
    public string? Observation { get; set; }

    public ElementScolariteEtudiant ElementScolariteEtudiant { get; set; } = null!;
}
