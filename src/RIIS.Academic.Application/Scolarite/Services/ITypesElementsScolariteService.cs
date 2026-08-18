using RIIS.Academic.Application.Scolarite.Dtos;

namespace RIIS.Academic.Application.Scolarite.Services;

public interface ITypesElementsScolariteService
{
    Task<List<TypeElementScolariteDto>> GetTypesElementsScolariteAsync(
        bool inclureInactifs = false,
        CancellationToken cancellationToken = default);

    Task<TypeElementScolariteDto?> GetTypeElementScolariteAsync(
        long id,
        CancellationToken cancellationToken = default);

    TypeElementScolariteDto CreateDefaultTypeElementScolarite();

    string GenerateCodeFromLibelle(string? libelle);

    Task SaveTypeElementScolariteAsync(
        TypeElementScolariteDto dto,
        CancellationToken cancellationToken = default);

    Task DeleteTypeElementScolariteAsync(
        long id,
        CancellationToken cancellationToken = default);
}
