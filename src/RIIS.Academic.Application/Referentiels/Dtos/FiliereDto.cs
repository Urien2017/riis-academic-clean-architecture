namespace RIIS.Academic.Application.Referentiels.Dtos;

public class FiliereDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public bool EstActive { get; set; } = true;
}
