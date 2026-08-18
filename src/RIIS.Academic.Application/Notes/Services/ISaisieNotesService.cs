using RIIS.Academic.Application.Common.Dtos;
using RIIS.Academic.Application.Notes.Dtos;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Notes.Services;

public interface ISaisieNotesService
{
    Task<List<LookupDto>> GetAnneesAcademiquesLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetClassesPedagogiquesLookupAsync(long? anneeAcademiqueId = null, CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetUnitesEnseignementLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetElementsConstitutifsLookupAsync(long? uniteEnseignementId = null, CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetEvaluationsLookupAsync(
        long? anneeAcademiqueId = null,
        long? uniteEnseignementId = null,
        long? elementConstitutifId = null,
        TypeEvaluation? typeEvaluation = null,
        CancellationToken cancellationToken = default);

    Task<SaisieNotesGrilleDto> GetGrilleSaisieAsync(
        long evaluationAcademiqueId,
        long classePedagogiqueId,
        CancellationToken cancellationToken = default);

    Task SaveNotesAsync(SaisieNotesGrilleDto grille, string? saisiePar = null, CancellationToken cancellationToken = default);
}
