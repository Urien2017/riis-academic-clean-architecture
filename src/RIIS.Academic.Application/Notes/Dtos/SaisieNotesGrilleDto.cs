using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Notes.Dtos;

public class SaisieNotesGrilleDto
{
    public long EvaluationAcademiqueId { get; set; }
    public long ClassePedagogiqueId { get; set; }
    public string EvaluationLibelle { get; set; } = string.Empty;
    public string ClassePedagogiqueLibelle { get; set; } = string.Empty;
    public string AnneeAcademiqueLibelle { get; set; } = string.Empty;
    public string ElementConstitutifLibelle { get; set; } = string.Empty;
    public string UniteEnseignementLibelle { get; set; } = string.Empty;
    public TypeEvaluation TypeEvaluation { get; set; }
    public decimal Bareme { get; set; } = 20m;
    public decimal PonderationPourcentage { get; set; }
    public string? Avertissement { get; set; }
    public List<SaisieNoteLigneDto> Lignes { get; set; } = [];
}
