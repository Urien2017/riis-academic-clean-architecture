namespace RIIS.Academic.Application.Dashboard.Dtos;

public class DashboardKpiDto
{
    public int NombreInscrits { get; set; }
    public int NombreClasses { get; set; }
    public int NombreEvaluations { get; set; }
    public int NombreNotesAttendues { get; set; }
    public int NombreNotesSaisies { get; set; }
    public decimal TauxNotesSaisies { get; set; }
    public int NombreAdmis { get; set; }
    public int NombreRattrapage { get; set; }
    public int NombreEchec { get; set; }
    public int NombreNonCalcules { get; set; }
    public decimal TauxReussite { get; set; }
    public decimal TauxRattrapage { get; set; }
    public decimal MoyenneGenerale { get; set; }
    public decimal CreditsMoyensCapitalises { get; set; }
    public int NombreProcesVerbauxGeneres { get; set; }
    public int NombreRelevesDisponibles { get; set; }
}
