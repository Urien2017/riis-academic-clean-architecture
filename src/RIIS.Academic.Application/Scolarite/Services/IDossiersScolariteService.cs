using RIIS.Academic.Application.Scolarite.Dtos;

namespace RIIS.Academic.Application.Scolarite.Services;

public interface IDossiersScolariteService
{
    Task<List<DossierScolariteDto>> GetDossiersScolariteAsync(
        bool inclureInscriptionsSansDossier = true,
        string? recherche = null,
        string? anneeAcademiqueCode = null,
        string? cycleCode = null,
        int? niveauNumero = null,
        string? filiereCode = null,
        string? specialiteCode = null,
        CancellationToken cancellationToken = default);

    Task<DossierScolariteDto?> GetDossierScolariteAsync(
        long id,
        CancellationToken cancellationToken = default);

    Task<DossierScolariteDto> GetOrCreateDossierDepuisInscriptionAsync(
        long inscriptionId,
        CancellationToken cancellationToken = default);

    Task<AdministrationDossierScolariteDto?> GetAdministrationDossierScolariteAsync(
        long dossierScolariteId,
        CancellationToken cancellationToken = default);

    Task<AdministrationDossierScolariteDto> SynchroniserDocumentsAdministratifsAsync(
        long dossierScolariteId,
        CancellationToken cancellationToken = default);

    Task<AdministrationDossierScolariteDto> SaveDocumentAdministratifAsync(
        DocumentAdministratifDossierDto dto,
        CancellationToken cancellationToken = default);

    Task<AdministrationDossierScolariteDto> RecalculerValidationAdministrativeAsync(
        long dossierScolariteId,
        CancellationToken cancellationToken = default);
}
