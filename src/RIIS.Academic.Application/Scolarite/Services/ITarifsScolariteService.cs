using RIIS.Academic.Application.Scolarite.Dtos;

namespace RIIS.Academic.Application.Scolarite.Services;

public interface ITarifsScolariteService
{
    Task<List<TarifScolariteDto>> GetTarifsScolariteAsync(
        long? typeElementScolariteId = null,
        long? parcoursAcademiqueId = null,
        bool inclureInactifs = false,
        CancellationToken cancellationToken = default);

    Task<TarifScolariteDto?> GetTarifScolariteAsync(
        long id,
        CancellationToken cancellationToken = default);

    TarifScolariteDto CreateDefaultTarifScolarite(long? typeElementScolariteId = null);

    Task SaveTarifScolariteAsync(
        TarifScolariteDto dto,
        CancellationToken cancellationToken = default);

    Task DeleteTarifScolariteAsync(
        long id,
        CancellationToken cancellationToken = default);

    Task<TarifScolariteDto?> ResolveTarifScolariteAsync(
        long typeElementScolariteId,
        long parcoursAcademiqueId,
        CancellationToken cancellationToken = default);
}
