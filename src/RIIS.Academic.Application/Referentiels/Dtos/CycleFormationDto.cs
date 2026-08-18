namespace RIIS.Academic.Application.Referentiels.Dtos;

public class CycleFormationDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public short OrdreAffichage { get; set; } = 1;
    public bool EstActif { get; set; } = true;
}
