namespace RIIS.Academic.Application.Dashboard.Dtos;

public class DashboardClasseSyntheseDto
{
    public long ClassePedagogiqueId { get; set; }
    public string ClassePedagogiqueCode { get; set; } = string.Empty;
    public string ClassePedagogiqueLibelle { get; set; } = string.Empty;
    public long? AnneeAcademiqueId { get; set; }
    public string AnneeAcademiqueLibelle { get; set; } = string.Empty;
    public long? CycleFormationId { get; set; }
    public string CycleFormationCode { get; set; } = string.Empty;
    public string CycleFormationLibelle { get; set; } = string.Empty;
    public long? NiveauEtudeId { get; set; }
    public string NiveauEtudeLibelle { get; set; } = string.Empty;
    public long? FiliereId { get; set; }
    public string FiliereLibelle { get; set; } = string.Empty;
    public long? SpecialiteId { get; set; }
    public string? SpecialiteLibelle { get; set; }
    public string ParcoursLibelle { get; set; } = string.Empty;
    public int NombreInscrits { get; set; }
    public int NombreNotesAttendues { get; set; }
    public int NombreNotesSaisies { get; set; }
    public decimal TauxNotesSaisies { get; set; }
    public decimal MoyenneClasse { get; set; }
    public decimal CreditsMoyensCapitalises { get; set; }
    public int NombreAdmis { get; set; }
    public int NombreRattrapage { get; set; }
    public int NombreEchec { get; set; }
    public int NombreNonCalcules { get; set; }
    public int NombreProcesVerbauxGeneres { get; set; }
    public int NombreRelevesDisponibles { get; set; }
}
