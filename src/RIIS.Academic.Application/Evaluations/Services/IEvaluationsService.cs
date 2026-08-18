using RIIS.Academic.Application.Common.Dtos;
using RIIS.Academic.Application.Evaluations.Dtos;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Evaluations.Services;

public interface IEvaluationsService
{
    Task<List<EvaluationAcademiqueDto>> GetEvaluationsAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? semestrePedagogiqueId = null,
        long? uniteEnseignementId = null,
        long? elementConstitutifId = null,
        TypeEvaluation? type = null,
        CancellationToken cancellationToken = default);

    Task<EvaluationAcademiqueDto?> GetEvaluationAsync(long id, CancellationToken cancellationToken = default);

    Task<EvaluationAcademiqueDto> CreateDefaultEvaluationAsync(
        long? anneeAcademiqueId = null,
        long? elementConstitutifId = null,
        TypeEvaluation type = TypeEvaluation.ControleContinu,
        CancellationToken cancellationToken = default);

    Task SaveEvaluationAsync(EvaluationAcademiqueDto dto, CancellationToken cancellationToken = default);
    Task DeleteEvaluationAsync(long id, CancellationToken cancellationToken = default);

    Task<List<LookupDto>> GetAnneesAcademiquesLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetCyclesFormationLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetSemestresLookupAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetUnitesEnseignementLookupAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? semestrePedagogiqueId = null,
        CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetElementsConstitutifsLookupAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? semestrePedagogiqueId = null,
        long? uniteEnseignementId = null,
        CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetSessionsNormalesLookupAsync(long? anneeAcademiqueId = null, long? elementConstitutifId = null, CancellationToken cancellationToken = default);
}
