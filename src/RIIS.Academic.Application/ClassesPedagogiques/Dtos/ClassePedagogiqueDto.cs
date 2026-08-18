namespace RIIS.Academic.Application.ClassesPedagogiques.Dtos;

public class ClassePedagogiqueDto
{
    public long Id { get; set; }
    public long ParcoursAcademiqueId { get; set; }
    public string ParcoursAcademiqueLibelle { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public int Effectif { get; set; }
}
