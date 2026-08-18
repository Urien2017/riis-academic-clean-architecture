namespace RIIS.Academic.Application.Programmes.Dtos;

public class UniteEnseignementDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public int NombreElementsConstitutifs { get; set; }
}
