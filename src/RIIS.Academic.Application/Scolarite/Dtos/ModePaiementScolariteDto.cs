namespace RIIS.Academic.Application.Scolarite.Dtos;

public class ModePaiementScolariteDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public int OrdreAffichage { get; set; }
    public bool EstActif { get; set; } = true;
}
