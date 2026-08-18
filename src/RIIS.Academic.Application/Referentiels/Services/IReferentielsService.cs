using RIIS.Academic.Application.Referentiels.Dtos;

namespace RIIS.Academic.Application.Referentiels.Services;

public interface IReferentielsService
{
    Task<List<AnneeAcademiqueDto>> GetAnneesAcademiquesAsync(CancellationToken cancellationToken = default);
    AnneeAcademiqueDto CreateDefaultAnneeAcademique();
    Task SaveAnneeAcademiqueAsync(AnneeAcademiqueDto dto, CancellationToken cancellationToken = default);
    Task DeleteAnneeAcademiqueAsync(long id, CancellationToken cancellationToken = default);

    Task<List<CycleFormationDto>> GetCyclesFormationAsync(CancellationToken cancellationToken = default);
    Task<List<ParcoursFormationDto>> GetParcoursFormationAsync(bool inclureInactifs = false, CancellationToken cancellationToken = default);
    Task<List<ParcoursAcademiqueDto>> GetParcoursAcademiquesAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? niveauEtudeId = null,
        long? filiereId = null,
        long? specialiteId = null,
        bool inclureInactifs = false,
        CancellationToken cancellationToken = default);
    ParcoursAcademiqueDto CreateDefaultParcoursAcademique(long? anneeAcademiqueId = null);
    Task SaveParcoursAcademiqueAsync(ParcoursAcademiqueDto dto, CancellationToken cancellationToken = default);
    Task DeleteParcoursAcademiqueAsync(long id, CancellationToken cancellationToken = default);
    CycleFormationDto CreateDefaultCycleFormation();
    Task SaveCycleFormationAsync(CycleFormationDto dto, CancellationToken cancellationToken = default);
    Task DeleteCycleFormationAsync(long id, CancellationToken cancellationToken = default);

    Task<List<FiliereDto>> GetFilieresAsync(CancellationToken cancellationToken = default);
    FiliereDto CreateDefaultFiliere();
    Task SaveFiliereAsync(FiliereDto dto, CancellationToken cancellationToken = default);
    Task DeleteFiliereAsync(long id, CancellationToken cancellationToken = default);

    Task<List<SpecialiteDto>> GetSpecialitesAsync(CancellationToken cancellationToken = default);
    Task<List<FiliereLookupDto>> GetFilieresActivesLookupAsync(CancellationToken cancellationToken = default);
    SpecialiteDto CreateDefaultSpecialite(long? filiereId = null, long? cycleFormationId = null);
    Task<string> GenerateSpecialiteCodeAsync(string? libelle, CancellationToken cancellationToken = default);
    Task SaveSpecialiteAsync(SpecialiteDto dto, CancellationToken cancellationToken = default);
    Task DeleteSpecialiteAsync(long id, CancellationToken cancellationToken = default);

    Task<List<NiveauEtudeDto>> GetNiveauxEtudeAsync(CancellationToken cancellationToken = default);
    Task<NiveauEtudeDto> CreateDefaultNiveauEtudeAsync(CancellationToken cancellationToken = default);
    Task SaveNiveauEtudeAsync(NiveauEtudeDto dto, CancellationToken cancellationToken = default);
    Task DeleteNiveauEtudeAsync(long id, CancellationToken cancellationToken = default);
}
