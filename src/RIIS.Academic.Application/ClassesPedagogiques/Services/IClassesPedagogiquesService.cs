using RIIS.Academic.Application.ClassesPedagogiques.Dtos;
using RIIS.Academic.Application.Common.Dtos;

namespace RIIS.Academic.Application.ClassesPedagogiques.Services;

public interface IClassesPedagogiquesService
{
    Task<List<ClassePedagogiqueDto>> GetClassesPedagogiquesAsync(
        long? anneeAcademiqueId = null,
        CancellationToken cancellationToken = default);

    Task<ClassePedagogiqueDto?> GetClassePedagogiqueAsync(long id, CancellationToken cancellationToken = default);
    Task<ClassePedagogiqueDto> CreateDefaultClassePedagogiqueAsync(CancellationToken cancellationToken = default);
    Task SaveClassePedagogiqueAsync(ClassePedagogiqueDto dto, CancellationToken cancellationToken = default);
    Task DeleteClassePedagogiqueAsync(long id, CancellationToken cancellationToken = default);

    Task<List<LookupDto>> GetAnneesAcademiquesLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetParcoursAcademiquesLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetNiveauxEtudeLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetMaquettesPedagogiquesLookupAsync(CancellationToken cancellationToken = default);
}
