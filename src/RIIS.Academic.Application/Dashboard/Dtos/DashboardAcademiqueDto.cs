namespace RIIS.Academic.Application.Dashboard.Dtos;

public class DashboardAcademiqueDto
{
    public DateTime DateGenerationUtc { get; set; } = DateTime.UtcNow;
    public DashboardAcademiqueFilterDto Filtres { get; set; } = new();
    public DashboardKpiDto Kpi { get; set; } = new();
    public List<DashboardInscriptionEvolutionDto> InscriptionsParPeriode { get; set; } = [];
    public List<DashboardEvaluationCompletionDto> ProgressionNotesParType { get; set; } = [];
    public List<DashboardEvaluationCompletionParSemestreDto> EvolutionSaisieNotes { get; set; } = [];
    public List<DashboardResultatRepartitionDto> RepartitionResultats { get; set; } = [];
    public List<DashboardTauxReussiteParGroupeDto> TauxReussiteParCycle { get; set; } = [];
    public List<DashboardTauxReussiteParGroupeDto> TauxReussiteParNiveau { get; set; } = [];
    public List<DashboardTauxReussiteParGroupeDto> TauxReussiteParParcours { get; set; } = [];
    public List<DashboardEcRisqueDto> ElementsConstitutifsARisque { get; set; } = [];
    public List<DashboardClasseSyntheseDto> Classes { get; set; } = [];
}
