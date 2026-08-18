namespace RIIS.Academic.Domain;

public class ClassePedagogique
{
    public long Id { get; set; }
    public long ParcoursAcademiqueId { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public int Effectif { get; set; }
}
