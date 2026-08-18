namespace RIIS.Academic.Application.Dashboard.Dtos;

public class DashboardEvaluationCompletionParSemestreDto
{
    public long? SemestrePedagogiqueId { get; set; }
    public byte? SemestreNumero { get; set; }
    public string Periode { get; set; } = string.Empty;
    public decimal TauxCcon { get; set; }
    public decimal TauxCc { get; set; }
    public decimal TauxSn { get; set; }
    public decimal TauxSr { get; set; }
}
