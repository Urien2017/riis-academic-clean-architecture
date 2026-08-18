namespace RIIS.Academic.Application.Referentiels.Dtos;

public class AnneeAcademiqueDto
{
    public long Id { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public short AnneeDebut { get; set; }
    public short AnneeFin { get; set; }
    public bool EstActive { get; set; } = true;
}
