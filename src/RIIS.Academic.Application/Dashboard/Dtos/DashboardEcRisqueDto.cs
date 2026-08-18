namespace RIIS.Academic.Application.Dashboard.Dtos;

public class DashboardEcRisqueDto
{
    public long ElementConstitutifId { get; set; }
    public string ElementConstitutifCode { get; set; } = string.Empty;
    public string ElementConstitutifLibelle { get; set; } = string.Empty;
    public long? UniteEnseignementId { get; set; }
    public string UniteEnseignementLibelle { get; set; } = string.Empty;
    public long? ClassePedagogiqueId { get; set; }
    public string ClassePedagogiqueLibelle { get; set; } = string.Empty;
    public decimal MoyenneClasse { get; set; }
    public int NombreNotes { get; set; }
    public int NombreEchecs { get; set; }
    public decimal TauxEchec { get; set; }
}
