using RIIS.Academic.Application.Common.Dtos;
using RIIS.Academic.Application.Inscriptions.Dtos;

namespace RIIS.Academic.Application.Inscriptions.Services;

public interface IInscriptionsService
{
    Task<List<InscriptionDto>> GetInscriptionsAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? niveauEtudeId = null,
        long? classePedagogiqueId = null,
        CancellationToken cancellationToken = default);

    Task<InscriptionDto?> GetInscriptionAsync(long id, CancellationToken cancellationToken = default);
    Task<InscriptionDto> CreateDefaultInscriptionAsync(CancellationToken cancellationToken = default);
    Task SaveInscriptionAsync(InscriptionDto dto, CancellationToken cancellationToken = default);
    Task DeleteInscriptionAsync(long id, CancellationToken cancellationToken = default);

    Task<List<LookupDto>> GetAnneesAcademiquesLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetEtudiantsLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetCyclesFormationLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetParcoursAcademiquesLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetNiveauxEtudeLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetClassesPedagogiquesLookupAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? niveauEtudeId = null,
        CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetMaquettesPedagogiquesLookupAsync(CancellationToken cancellationToken = default);
}
