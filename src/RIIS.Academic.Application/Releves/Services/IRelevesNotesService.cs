using RIIS.Academic.Application.Releves.Dtos;

namespace RIIS.Academic.Application.Releves.Services;

public interface IRelevesNotesService
{
    Task<List<ReleveNoteEtudiantDisponibleDto>> GetEtudiantsDisponiblesAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? niveauEtudeId = null,
        long? classePedagogiqueId = null,
        CancellationToken cancellationToken = default);

    Task<ReleveNoteAnnuelDto?> GenererReleveAnnuelAsync(
        long inscriptionId,
        CancellationToken cancellationToken = default);
}
