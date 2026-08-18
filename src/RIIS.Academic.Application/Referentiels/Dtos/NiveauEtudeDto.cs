namespace RIIS.Academic.Application.Referentiels.Dtos;

public class NiveauEtudeDto
{
    public long Id { get; set; }
    public byte Numero { get; set; } = 1;
    public string Libelle { get; set; } = string.Empty;
    public bool EstActif { get; set; } = true;
}
