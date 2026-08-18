using RIIS.Academic.Application.Common.Dtos;
using RIIS.Academic.Application.ProcesVerbaux.Dtos;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.ProcesVerbaux.Services;

public interface IProcesVerbauxService
{
    Task<List<ProcesVerbalDto>> GetProcesVerbauxAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? classePedagogiqueId = null,
        byte? semestreNumero = null,
        TypeProcesVerbal? type = null,
        CancellationToken cancellationToken = default);

    Task<ProcesVerbalDto?> GetProcesVerbalAsync(long id, CancellationToken cancellationToken = default);

    Task<List<LookupDto>> GetAnneesAcademiquesLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetCyclesFormationLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetClassesPedagogiquesLookupAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetSemestresLookupAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? classePedagogiqueId = null,
        CancellationToken cancellationToken = default);
}
