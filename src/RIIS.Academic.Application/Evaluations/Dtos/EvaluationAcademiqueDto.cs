using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Evaluations.Dtos;

public class EvaluationAcademiqueDto
{
    public long Id { get; set; }
    public long AnneeAcademiqueId { get; set; }
    public string AnneeAcademiqueLibelle { get; set; } = string.Empty;
    public long MaquetteElementConstitutifId { get; set; }
    public string ElementConstitutifLibelle { get; set; } = string.Empty;
    public long UniteEnseignementId { get; set; }
    public string UniteEnseignementLibelle { get; set; } = string.Empty;
    public long SemestrePedagogiqueId { get; set; }
    public string SemestrePedagogiqueLibelle { get; set; } = string.Empty;
    public long CycleFormationId { get; set; }
    public string CycleFormationLibelle { get; set; } = string.Empty;
    public TypeEvaluation Type { get; set; } = TypeEvaluation.ControleContinu;
    public string TypeLibelle { get; set; } = string.Empty;
    public byte Numero { get; set; } = 1;
    public string Code { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public decimal Bareme { get; set; } = 20m;
    public decimal PonderationPourcentage { get; set; }
    public DateTime? DateEvaluation { get; set; }
    public long? EvaluationRemplaceeId { get; set; }
    public string? EvaluationRemplaceeLibelle { get; set; }
    public string? Observation { get; set; }
}
