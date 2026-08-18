namespace RIIS.Academic.Application.Dashboard.Dtos;

public class DashboardTauxReussiteParGroupeDto
{
    public string TypeGroupe { get; set; } = string.Empty;
    public long? GroupeId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public int NombreInscrits { get; set; }
    public int NombreAdmis { get; set; }
    public int NombreRattrapage { get; set; }
    public int NombreEchec { get; set; }
    public decimal TauxReussite { get; set; }
}
