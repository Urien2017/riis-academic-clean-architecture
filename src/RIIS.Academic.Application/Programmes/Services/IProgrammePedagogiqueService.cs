using RIIS.Academic.Application.Common.Dtos;
using RIIS.Academic.Application.Programmes.Dtos;

namespace RIIS.Academic.Application.Programmes.Services;

public interface IProgrammePedagogiqueService
{
    Task<List<MaquettePedagogiqueDto>> GetMaquettesAsync(CancellationToken cancellationToken = default);
    Task<List<MaquettePedagogiqueHierarchyDto>> GetMaquettesHierarchyAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? maquettePedagogiqueId = null,
        CancellationToken cancellationToken = default);
    Task<MaquettePedagogiqueDto?> GetMaquetteAsync(long id, CancellationToken cancellationToken = default);
    MaquettePedagogiqueDto CreateDefaultMaquette(long? parcoursAcademiqueId = null);
    Task SaveMaquetteAsync(MaquettePedagogiqueDto dto, CancellationToken cancellationToken = default);
    Task DeleteMaquetteAsync(long id, CancellationToken cancellationToken = default);

    Task<List<SemestrePedagogiqueDto>> GetSemestresAsync(long? maquettePedagogiqueId = null, CancellationToken cancellationToken = default);
    Task<SemestrePedagogiqueDto?> GetSemestreAsync(long id, CancellationToken cancellationToken = default);
    Task<SemestrePedagogiqueDto> CreateDefaultSemestreAsync(long? maquettePedagogiqueId = null, CancellationToken cancellationToken = default);
    Task SaveSemestreAsync(SemestrePedagogiqueDto dto, CancellationToken cancellationToken = default);
    Task DeleteSemestreAsync(long id, CancellationToken cancellationToken = default);

    Task<List<UniteEnseignementDto>> GetUnitesEnseignementAsync(long? semestrePedagogiqueId = null, CancellationToken cancellationToken = default);
    Task<List<UniteEnseignementDto>> GetUnitesEnseignementByHierarchyAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? parcoursAcademiqueId = null,
        long? semestrePedagogiqueId = null,
        CancellationToken cancellationToken = default);
    Task<List<UniteEnseignementHierarchyDto>> GetUnitesEnseignementHierarchyAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? parcoursAcademiqueId = null,
        long? semestrePedagogiqueId = null,
        CancellationToken cancellationToken = default);
    Task<UniteEnseignementDto?> GetUniteEnseignementAsync(long id, CancellationToken cancellationToken = default);
    Task<UniteEnseignementDto> CreateDefaultUniteEnseignementAsync(long? semestrePedagogiqueId = null, CancellationToken cancellationToken = default);
    Task SaveUniteEnseignementAsync(UniteEnseignementDto dto, CancellationToken cancellationToken = default);
    Task DeleteUniteEnseignementAsync(long id, CancellationToken cancellationToken = default);

    Task<List<ElementConstitutifDto>> GetElementsConstitutifsAsync(long? uniteEnseignementId = null, CancellationToken cancellationToken = default);
    Task<ElementConstitutifDto?> GetElementConstitutifAsync(long id, CancellationToken cancellationToken = default);
    Task<ElementConstitutifDto> CreateDefaultElementConstitutifAsync(long? uniteEnseignementId = null, CancellationToken cancellationToken = default);
    Task SaveElementConstitutifAsync(ElementConstitutifDto dto, CancellationToken cancellationToken = default);
    Task DeleteElementConstitutifAsync(long id, CancellationToken cancellationToken = default);

    Task<List<LookupDto>> GetAnneesAcademiquesLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetCyclesFormationLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetParcoursAcademiquesLookupAsync(long? cycleFormationId = null, CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetMaquettesLookupAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetNiveauxEtudeLookupAsync(CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetSemestresLookupAsync(long? maquettePedagogiqueId = null, CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetSemestresHierarchyLookupAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? parcoursAcademiqueId = null,
        CancellationToken cancellationToken = default);
    Task<List<LookupDto>> GetUnitesEnseignementLookupAsync(long? semestrePedagogiqueId = null, CancellationToken cancellationToken = default);
}
