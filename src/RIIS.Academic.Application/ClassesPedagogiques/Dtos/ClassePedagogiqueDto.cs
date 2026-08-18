namespace RIIS.Academic.Application.ClassesPedagogiques.Dtos;

public class ClassePedagogiqueDto
{
    public long Id { get; set; }
    public long AnneeAcademiqueId { get; set; }
    public string AnneeAcademiqueLibelle { get; set; } = string.Empty;
    public long ParcoursAcademiqueId { get; set; }
    public string ParcoursAcademiqueLibelle { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public bool EstActive { get; set; } = true;
    public int Effectif { get; set; }
}
