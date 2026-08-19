namespace RIIS.Academic.Domain;

public class TarifScolarite
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public long TypeElementScolariteId { get; set; }
    public long ParcoursAcademiqueId { get; set; }
    public decimal Montant { get; set; }
    public string Devise { get; set; } = "XAF";
    public int Priorite { get; set; }
    public bool EstActif { get; set; } = true;

    public TypeElementScolarite TypeElementScolarite { get; set; } = null!;
    public ParcoursAcademique ParcoursAcademique { get; set; } = null!;
}
