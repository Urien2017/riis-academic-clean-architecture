namespace RIIS.Academic.Application.Dashboard.Dtos;

public class DashboardEvaluationCompletionDto
{
    public string CodeTypeEvaluation { get; set; } = string.Empty;
    public string LibelleTypeEvaluation { get; set; } = string.Empty;
    public int NombreEvaluations { get; set; }
    public int NombreNotesAttendues { get; set; }
    public int NombreNotesSaisies { get; set; }
    public decimal TauxCompletion { get; set; }
}
