namespace RIIS.Academic.Domain;

public class ContactUrgence
{
    public long Id { get; set; }
    public long EtudiantId { get; set; }
    public required string NomComplet { get; set; }
    public string? LienParente { get; set; }
    public required string TelephonePrincipal { get; set; }
    public string? TelephoneSecondaire { get; set; }
    public bool EstPrincipal { get; set; } = true;

    public Etudiant Etudiant { get; set; } = null!;
}
