using RIIS.Academic.Application.Scolarite.Dtos;

namespace RIIS.Academic.Application.Scolarite.Services;

public interface ITarifsScolariteService
{
    Task<List<TarifScolariteDto>> GetTarifsScolariteAsync(
        long? typeElementScolariteId = null,
        string? anneeAcademiqueCode = null,
        string? cycleCode = null,
        int? niveauNumero = null,
        string? filiereCode = null,
        string? specialiteCode = null,
        bool inclureInactifs = false,
        CancellationToken cancellationToken = default);

    Task<TarifScolariteDto?> GetTarifScolariteAsync(
        long id,
        CancellationToken cancellationToken = default);

    TarifScolariteDto CreateDefaultTarifScolarite(long? typeElementScolariteId = null);

    string GenerateCode(TarifScolariteDto dto);

    Task SaveTarifScolariteAsync(
        TarifScolariteDto dto,
        CancellationToken cancellationToken = default);

    Task DeleteTarifScolariteAsync(
        long id,
        CancellationToken cancellationToken = default);

    Task<TarifScolariteDto?> ResolveTarifScolariteAsync(
        long typeElementScolariteId,
        TarifScolariteContexteDto contexte,
        CancellationToken cancellationToken = default);
}
