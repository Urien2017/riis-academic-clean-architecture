namespace RIIS.Academic.Domain;

public class Etablissement
{
    public long Id { get; set; }
    public required string NomOfficiel { get; set; }
    public string? Sigle { get; set; }
    public string? NumeroAutorisation { get; set; }
    public string? NumeroRegistreCommerce { get; set; }
    public string? Banque { get; set; }
    public string? NumeroCompteBancaire { get; set; }
    public string? TelephonePrincipal { get; set; }
    public string? TelephoneSecondaire { get; set; }
    public string? BoitePostale { get; set; }
    public string? Ville { get; set; }
    public string? Pays { get; set; }
    public string? Email { get; set; }
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public string? Adresse { get; set; }
    public string? LogoUrl { get; set; }
    public bool EstActif { get; set; } = true;
}
