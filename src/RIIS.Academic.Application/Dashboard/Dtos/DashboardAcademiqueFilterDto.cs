namespace RIIS.Academic.Application.Dashboard.Dtos;

public class DashboardAcademiqueFilterDto
{
    public long? AnneeAcademiqueId { get; set; }
    public long? CycleFormationId { get; set; }
    public long? NiveauEtudeId { get; set; }
    public long? FiliereId { get; set; }
    public long? SpecialiteId { get; set; }
    public long? ClassePedagogiqueId { get; set; }
    public long? MaquettePedagogiqueId { get; set; }
    public long? SemestrePedagogiqueId { get; set; }
    public string? CodeSession { get; set; }
}
