using RIIS.Academic.Application.Abstractions.Persistence;
using RIIS.Academic.Application.Common.Dtos;
using RIIS.Academic.Application.ProcesVerbaux.Dtos;
using RIIS.Academic.Domain;
using System.Text.Json;

namespace RIIS.Academic.Application.ProcesVerbaux.Services;

public class ProcesVerbauxService(
    IRepository<ProcesVerbal> procesVerbaux,
    IRepository<ProcesVerbalLigne> procesVerbauxLignes,
    IRepository<AnneeAcademique> anneesAcademiques,
    IRepository<CycleFormation> cyclesFormation,
    IRepository<ParcoursAcademique> parcoursAcademiques,
    IRepository<ClassePedagogique> classesPedagogiques,
    IRepository<SemestrePedagogique> semestresPedagogiques) : IProcesVerbauxService
{
    public async Task<List<ProcesVerbalDto>> GetProcesVerbauxAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? classePedagogiqueId = null,
        byte? semestreNumero = null,
        TypeProcesVerbal? type = null,
        CancellationToken cancellationToken = default)
    {
        var pvItems = await procesVerbaux.ListAsync(cancellationToken);
        var ligneItems = await procesVerbauxLignes.ListAsync(cancellationToken);
        var anneeItems = await anneesAcademiques.ListAsync(cancellationToken);
        var cycleItems = await cyclesFormation.ListAsync(cancellationToken);
        var parcoursItems = await parcoursAcademiques.ListAsync(cancellationToken);
        var classeItems = await classesPedagogiques.ListAsync(cancellationToken);
        var semestreItems = await semestresPedagogiques.ListAsync(cancellationToken);

        pvItems = ApplyFilters(
            pvItems,
            parcoursItems,
            classeItems,
            semestreItems,
            anneeAcademiqueId,
            cycleFormationId,
            classePedagogiqueId,
            semestreNumero,
            type);

        return pvItems
            .Select(pv => ToDto(
                pv,
                ligneItems.Where(x => x.ProcesVerbalId == pv.Id).ToList(),
                anneeItems,
                cycleItems,
                parcoursItems,
                classeItems,
                semestreItems))
            .OrderByDescending(x => x.AnneeAcademiqueLibelle)
            .ThenBy(x => x.CycleFormationLibelle)
            .ThenBy(x => x.ParcoursLibelle)
            .ThenBy(x => x.SemestreNumero)
            .ThenBy(x => x.ClassePedagogiqueLibelle)
            .ToList();
    }

    public async Task<ProcesVerbalDto?> GetProcesVerbalAsync(long id, CancellationToken cancellationToken = default)
    {
        var pvItems = await procesVerbaux.ListAsync(cancellationToken);
        var pv = pvItems.FirstOrDefault(x => x.Id == id);

        if (pv is null)
        {
            return null;
        }

        var ligneItems = await procesVerbauxLignes.ListAsync(cancellationToken);
        var anneeItems = await anneesAcademiques.ListAsync(cancellationToken);
        var cycleItems = await cyclesFormation.ListAsync(cancellationToken);
        var parcoursItems = await parcoursAcademiques.ListAsync(cancellationToken);
        var classeItems = await classesPedagogiques.ListAsync(cancellationToken);
        var semestreItems = await semestresPedagogiques.ListAsync(cancellationToken);

        return ToDto(
            pv,
            ligneItems.Where(x => x.ProcesVerbalId == pv.Id).ToList(),
            anneeItems,
            cycleItems,
            parcoursItems,
            classeItems,
            semestreItems);
    }

    public async Task<List<LookupDto>> GetAnneesAcademiquesLookupAsync(CancellationToken cancellationToken = default)
    {
        var items = await anneesAcademiques.ListAsync(cancellationToken);

        return items
            .OrderByDescending(x => x.AnneeDebut)
            .Select(x => new LookupDto { Id = x.Id, Libelle = x.Libelle })
            .ToList();
    }

    public async Task<List<LookupDto>> GetCyclesFormationLookupAsync(CancellationToken cancellationToken = default)
    {
        var items = await cyclesFormation.ListAsync(cancellationToken);

        return items
            .Where(x => x.EstActif)
            .OrderBy(x => x.OrdreAffichage)
            .ThenBy(x => x.Libelle)
            .Select(x => new LookupDto { Id = x.Id, Libelle = FormatCodeLibelle(x.Code, x.Libelle) })
            .ToList();
    }

    public async Task<List<LookupDto>> GetClassesPedagogiquesLookupAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        CancellationToken cancellationToken = default)
    {
        var pvItems = await procesVerbaux.ListAsync(cancellationToken);
        var classeItems = await classesPedagogiques.ListAsync(cancellationToken);
        var parcoursItems = await parcoursAcademiques.ListAsync(cancellationToken);

        var classeIds = pvItems
            .Where(x => anneeAcademiqueId is null || x.AnneeAcademiqueId == anneeAcademiqueId)
            .Select(x => x.ClassePedagogiqueId)
            .ToHashSet();

        var filteredClasses = classeItems
            .Where(x => classeIds.Contains(x.Id));

        if (cycleFormationId is not null)
        {
            var parcoursIds = parcoursItems
                .Where(x => x.CycleFormationId == cycleFormationId)
                .Select(x => x.Id)
                .ToHashSet();

            filteredClasses = filteredClasses
                .Where(x => parcoursIds.Contains(x.ParcoursAcademiqueId));
        }

        return filteredClasses
            .OrderBy(x => x.Libelle)
            .Select(x => new LookupDto { Id = x.Id, Libelle = x.Libelle })
            .ToList();
    }

    public async Task<List<LookupDto>> GetSemestresLookupAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? classePedagogiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var pvItems = await procesVerbaux.ListAsync(cancellationToken);
        var classeItems = await classesPedagogiques.ListAsync(cancellationToken);
        var parcoursItems = await parcoursAcademiques.ListAsync(cancellationToken);
        var semestreItems = await semestresPedagogiques.ListAsync(cancellationToken);

        pvItems = ApplyFilters(
            pvItems,
            parcoursItems,
            classeItems,
            semestreItems,
            anneeAcademiqueId,
            cycleFormationId,
            classePedagogiqueId,
            semestreNumero: null,
            type: null);

        var semestreIds = pvItems
            .Where(x => x.SemestrePedagogiqueId is not null)
            .Select(x => x.SemestrePedagogiqueId!.Value)
            .ToHashSet();

        return semestreItems
            .Where(x => semestreIds.Contains(x.Id))
            .GroupBy(x => x.NumeroSemestre)
            .OrderBy(x => x.Key)
            .Select(x => new LookupDto { Id = x.Key, Libelle = $"Semestre {x.Key}" })
            .ToList();
    }

    private static List<ProcesVerbal> ApplyFilters(
        List<ProcesVerbal> pvItems,
        IReadOnlyCollection<ParcoursAcademique> parcoursItems,
        IReadOnlyCollection<ClassePedagogique> classeItems,
        IReadOnlyCollection<SemestrePedagogique> semestreItems,
        long? anneeAcademiqueId,
        long? cycleFormationId,
        long? classePedagogiqueId,
        byte? semestreNumero,
        TypeProcesVerbal? type)
    {
        if (anneeAcademiqueId is not null)
        {
            pvItems = pvItems
                .Where(x => x.AnneeAcademiqueId == anneeAcademiqueId)
                .ToList();
        }

        if (cycleFormationId is not null)
        {
            var parcoursIds = parcoursItems
                .Where(x => x.CycleFormationId == cycleFormationId)
                .Select(x => x.Id)
                .ToHashSet();

            var classeIds = classeItems
                .Where(x => parcoursIds.Contains(x.ParcoursAcademiqueId))
                .Select(x => x.Id)
                .ToHashSet();

            pvItems = pvItems
                .Where(x => classeIds.Contains(x.ClassePedagogiqueId))
                .ToList();
        }

        if (classePedagogiqueId is not null)
        {
            pvItems = pvItems
                .Where(x => x.ClassePedagogiqueId == classePedagogiqueId)
                .ToList();
        }

        if (semestreNumero is not null)
        {
            var semestreIds = semestreItems
                .Where(x => x.NumeroSemestre == semestreNumero)
                .Select(x => x.Id)
                .ToHashSet();

            pvItems = pvItems
                .Where(x => x.SemestrePedagogiqueId is not null
                    && semestreIds.Contains(x.SemestrePedagogiqueId.Value))
                .ToList();
        }

        if (type is not null)
        {
            pvItems = pvItems
                .Where(x => x.Type == type)
                .ToList();
        }

        return pvItems;
    }

    private static ProcesVerbalDto ToDto(
        ProcesVerbal pv,
        IReadOnlyCollection<ProcesVerbalLigne> lignes,
        IReadOnlyCollection<AnneeAcademique> annees,
        IReadOnlyCollection<CycleFormation> cycles,
        IReadOnlyCollection<ParcoursAcademique> parcours,
        IReadOnlyCollection<ClassePedagogique> classes,
        IReadOnlyCollection<SemestrePedagogique> semestres)
    {
        var annee = annees.FirstOrDefault(x => x.Id == pv.AnneeAcademiqueId);
        var classe = classes.FirstOrDefault(x => x.Id == pv.ClassePedagogiqueId);
        var parcoursItem = classe is null
            ? null
            : parcours.FirstOrDefault(x => x.Id == classe.ParcoursAcademiqueId);
        var cycle = parcoursItem is null
            ? null
            : cycles.FirstOrDefault(x => x.Id == parcoursItem.CycleFormationId);
        var semestre = pv.SemestrePedagogiqueId is null
            ? null
            : semestres.FirstOrDefault(x => x.Id == pv.SemestrePedagogiqueId);

        var moyennes = lignes
            .Where(x => x.MoyenneGenerale.HasValue)
            .Select(x => x.MoyenneGenerale!.Value)
            .ToList();

        return new ProcesVerbalDto
        {
            Id = pv.Id,
            AnneeAcademiqueId = pv.AnneeAcademiqueId,
            ClassePedagogiqueId = pv.ClassePedagogiqueId,
            SemestrePedagogiqueId = pv.SemestrePedagogiqueId,
            CycleFormationId = cycle?.Id,
            AnneeAcademiqueLibelle = annee?.Libelle ?? string.Empty,
            CycleFormationLibelle = cycle is null ? string.Empty : FormatCodeLibelle(cycle.Code, cycle.Libelle),
            ParcoursLibelle = parcoursItem is null ? string.Empty : FormatCodeLibelle(parcoursItem.Code, parcoursItem.Libelle),
            ClassePedagogiqueLibelle = classe?.Libelle ?? string.Empty,
            SemestrePedagogiqueLibelle = semestre is null ? "Annuel" : $"Semestre {semestre.NumeroSemestre}",
            SemestreNumero = semestre?.NumeroSemestre,
            Type = pv.Type,
            CodeSession = pv.CodeSession,
            Titre = pv.Titre,
            DateEditionUtc = pv.DateEditionUtc,
            EstDefinitif = pv.EstDefinitif,
            Observation = pv.Observation,
            NombreLignes = lignes.Count,
            MoyenneMin = moyennes.Count == 0 ? null : moyennes.Min(),
            MoyenneMax = moyennes.Count == 0 ? null : moyennes.Max(),
            NombreValides = lignes.Count(x => x.DecisionJury == DecisionAcademique.Valide),
            NombreRattrapage = lignes.Count(x => x.DecisionJury == DecisionAcademique.AutoriseRattrapage),
            NombreNonDeliberes = lignes.Count(x => x.DecisionJury == DecisionAcademique.NonDeliberee),
            Lignes = lignes
                .OrderBy(x => x.Rang ?? int.MaxValue)
                .ThenBy(x => x.NomCompletSnapshot)
                .Select(ToLigneDto)
                .ToList()
        };
    }

    private static ProcesVerbalLigneDto ToLigneDto(ProcesVerbalLigne ligne)
        => new()
        {
            Id = ligne.Id,
            ProcesVerbalId = ligne.ProcesVerbalId,
            InscriptionId = ligne.InscriptionId,
            Matricule = ligne.MatriculeSnapshot,
            EtudiantNomComplet = ligne.NomCompletSnapshot,
            MoyenneGenerale = ligne.MoyenneGenerale,
            CreditsAcquis = ligne.CreditsAcquis,
            Rang = ligne.Rang,
            DecisionJury = ligne.DecisionJury,
            ElementsConstitutifs = ParseDetailsNotes(ligne.DetailsNotesJson)
        };

    private static string FormatCodeLibelle(string code, string libelle)
        => string.IsNullOrWhiteSpace(code) ? libelle : $"{code} - {libelle}";

    private static List<ProcesVerbalElementConstitutifLigneDto> ParseDetailsNotes(string? detailsNotesJson)
    {
        if (string.IsNullOrWhiteSpace(detailsNotesJson))
        {
            return [];
        }

        try
        {
            using var document = JsonDocument.Parse(detailsNotesJson);

            if (!document.RootElement.TryGetProperty("elementsConstitutifs", out var elements)
                || elements.ValueKind != JsonValueKind.Array)
            {
                return [];
            }

            return elements
                .EnumerateArray()
                .Select(x => new ProcesVerbalElementConstitutifLigneDto
                {
                    UniteEnseignementCode = GetString(x, "uniteEnseignementCode"),
                    UniteEnseignementLibelle = GetString(x, "uniteEnseignementLibelle"),
                    ElementConstitutifCode = GetString(x, "elementConstitutifCode"),
                    ElementConstitutifLibelle = GetString(x, "elementConstitutifLibelle"),
                    Credits = GetDecimal(x, "credits") ?? 0m,
                    MoyenneCcon = GetDecimal(x, "moyenneCcon"),
                    MoyenneCc = GetDecimal(x, "moyenneCc"),
                    MoyenneSn = GetDecimal(x, "moyenneSn"),
                    MoyenneFinale = GetDecimal(x, "moyenneFinale"),
                    CreditsAcquis = GetDecimal(x, "creditsAcquis") ?? 0m,
                    Decision = GetString(x, "decision")
                })
                .ToList();
        }
        catch (JsonException)
        {
            return [];
        }
    }

    private static string GetString(JsonElement element, string propertyName)
        => element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.String
            ? property.GetString() ?? string.Empty
            : string.Empty;

    private static decimal? GetDecimal(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property)
            || property.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        return property.TryGetDecimal(out var value) ? value : null;
    }
}
