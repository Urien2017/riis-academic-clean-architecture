namespace RIIS.Academic.Application.Scolarite.Dtos;

public class TarifScolariteDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public long TypeElementScolariteId { get; set; }
    public string TypeElementScolariteCode { get; set; } = string.Empty;
    public string TypeElementScolariteLibelle { get; set; } = string.Empty;
    public long ParcoursAcademiqueId { get; set; }
    public string ParcoursAcademiqueLibelle { get; set; } = string.Empty;
    public decimal Montant { get; set; }
    public string Devise { get; set; } = "XAF";
    public int Priorite { get; set; }
    public bool EstActif { get; set; } = true;
}
