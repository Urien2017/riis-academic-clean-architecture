using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Etudiants.Dtos;

public class EtudiantDto
{
    public long Id { get; set; }
    public string? Matricule { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenoms { get; set; } = string.Empty;
    public string NomComplet => $"{Nom} {Prenoms}".Trim();
    public DateOnly DateNaissance { get; set; }
    public string LieuNaissance { get; set; } = string.Empty;
    public Sexe Sexe { get; set; } = Sexe.NonRenseigne;
    public AptitudeMedicale AptitudeMedicale { get; set; } = AptitudeMedicale.NonRenseignee;
    public string Nationalite { get; set; } = string.Empty;
    public string? RegionOrigine { get; set; }
    public string TelephonePrincipal { get; set; } = string.Empty;
    public string? TelephoneSecondaire { get; set; }
    public string? Email { get; set; }
    public string? NomPere { get; set; }
    public string? NomMere { get; set; }
    public string? LieuResidence { get; set; }
    public string? PhotoUrl { get; set; }
}
