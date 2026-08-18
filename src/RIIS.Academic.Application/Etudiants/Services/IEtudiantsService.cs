using RIIS.Academic.Application.Etudiants.Dtos;

namespace RIIS.Academic.Application.Etudiants.Services;

public interface IEtudiantsService
{
    Task<List<EtudiantDto>> GetEtudiantsAsync(string? recherche = null, CancellationToken cancellationToken = default);
    Task<EtudiantDto?> GetEtudiantAsync(long id, CancellationToken cancellationToken = default);
    EtudiantDto CreateDefaultEtudiant();
    Task SaveEtudiantAsync(EtudiantDto dto, CancellationToken cancellationToken = default);
    Task DeleteEtudiantAsync(long id, CancellationToken cancellationToken = default);
}
