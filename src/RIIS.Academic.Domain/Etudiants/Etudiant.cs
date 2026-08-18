namespace RIIS.Academic.Domain;

public class Etudiant
{
    public long Id { get; set; }
    public string? Matricule { get; set; }
    public required string Nom { get; set; }
    public required string Prenoms { get; set; }
    public DateOnly DateNaissance { get; set; }
    public required string LieuNaissance { get; set; }
    public Sexe Sexe { get; set; }
    public AptitudeMedicale AptitudeMedicale { get; set; }
    public required string Nationalite { get; set; }
    public string? RegionOrigine { get; set; }
    public required string TelephonePrincipal { get; set; }
    public string? TelephoneSecondaire { get; set; }
    public string? Email { get; set; }
    public string? NomPere { get; set; }
    public string? NomMere { get; set; }
    public string? LieuResidence { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime CreeLeUtc { get; set; } = DateTime.UtcNow;
    public byte[] Version { get; set; } = [];

    public ICollection<ContactUrgence> ContactsUrgence { get; set; } = [];
    public ICollection<Inscription> Inscriptions { get; set; } = [];
}
