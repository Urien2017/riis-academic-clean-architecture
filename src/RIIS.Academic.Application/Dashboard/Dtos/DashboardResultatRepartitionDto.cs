namespace RIIS.Academic.Application.Dashboard.Dtos;

public class DashboardResultatRepartitionDto
{
    public string Code { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public int Nombre { get; set; }
    public decimal Taux { get; set; }
    public string Couleur { get; set; } = string.Empty;
}
