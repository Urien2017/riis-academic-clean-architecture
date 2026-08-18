using RIIS.Academic.Application.Abstractions.Persistence;
using RIIS.Academic.Application.ClassesPedagogiques.Dtos;
using RIIS.Academic.Application.Common.Dtos;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.ClassesPedagogiques.Services;

public class ClassesPedagogiquesService(
    IRepository<ClassePedagogique> classesPedagogiques,
    IRepository<AnneeAcademique> anneesAcademiques,
    IRepository<ParcoursAcademique> parcoursAcademiques,
    IRepository<NiveauEtude> niveauxEtude,
    IRepository<MaquettePedagogique> maquettesPedagogiques,
    IRepository<Inscription> inscriptions) : IClassesPedagogiquesService
{
    public async Task<List<ClassePedagogiqueDto>> GetClassesPedagogiquesAsync(
        long? anneeAcademiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var classes = await classesPedagogiques.ListAsync(cancellationToken);
        var annees = await anneesAcademiques.ListAsync(cancellationToken);
        var ouvertures = await parcoursAcademiques.ListAsync(cancellationToken);
        var niveaux = await niveauxEtude.ListAsync(cancellationToken);
        var maquettes = await maquettesPedagogiques.ListAsync(cancellationToken);
        var inscriptionItems = await inscriptions.ListAsync(cancellationToken);

        if (anneeAcademiqueId is not null)
        {
            classes = classes
                .Where(x => x.AnneeAcademiqueId == anneeAcademiqueId)
                .ToList();
        }

        return classes
            .Select(classe => ToDto(classe, annees, ouvertures, niveaux, maquettes, inscriptionItems))
            .OrderByDescending(x => x.AnneeAcademiqueLibelle)
            .ThenBy(x => x.ParcoursAcademiqueLibelle)
            .ThenBy(x => x.NiveauEtudeLibelle)
            .ThenBy(x => x.Code)
            .ToList();
    }

    public async Task<ClassePedagogiqueDto?> GetClassePedagogiqueAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var classes = await classesPedagogiques.ListAsync(cancellationToken);
        var classe = classes.FirstOrDefault(x => x.Id == id);

        if (classe is null)
        {
            return null;
        }

        var annees = await anneesAcademiques.ListAsync(cancellationToken);
        var ouvertures = await parcoursAcademiques.ListAsync(cancellationToken);
        var niveaux = await niveauxEtude.ListAsync(cancellationToken);
        var maquettes = await maquettesPedagogiques.ListAsync(cancellationToken);
        var inscriptionItems = await inscriptions.ListAsync(cancellationToken);

        return ToDto(classe, annees, ouvertures, niveaux, maquettes, inscriptionItems);
    }

    public async Task<ClassePedagogiqueDto> CreateDefaultClassePedagogiqueAsync(
        CancellationToken cancellationToken = default)
    {
        var annee = await GetDefaultAnneeAcademiqueAsync(cancellationToken);

        return new ClassePedagogiqueDto
        {
            AnneeAcademiqueId = annee?.Id ?? 0,
            AnneeAcademiqueLibelle = annee?.Libelle ?? string.Empty,
            EstActive = true
        };
    }

    public async Task SaveClassePedagogiqueAsync(
        ClassePedagogiqueDto dto,
        CancellationToken cancellationToken = default)
    {
        if (dto.AnneeAcademiqueId <= 0)
        {
            throw new InvalidOperationException("L'année académique de la classe est obligatoire.");
        }

        if (dto.ParcoursAcademiqueId <= 0)
        {
            throw new InvalidOperationException("Le parcours cycle/filière/spécialité de la classe est obligatoire.");
        }

        if (dto.NiveauEtudeId <= 0)
        {
            throw new InvalidOperationException("Le niveau de la classe est obligatoire.");
        }

        dto.Code = RequireText(dto.Code, "Le code de la classe est obligatoire.").ToUpperInvariant();
        dto.Libelle = RequireText(dto.Libelle, "Le libellé de la classe est obligatoire.");

        await EnsureReferencesExistAsync(dto, cancellationToken);

        var existingClasses = await classesPedagogiques.ListAsync(cancellationToken);
        var duplicate = existingClasses.Any(x =>
            x.Id != dto.Id
            && x.AnneeAcademiqueId == dto.AnneeAcademiqueId
            && x.ParcoursAcademiqueId == dto.ParcoursAcademiqueId
            && x.NiveauEtudeId == dto.NiveauEtudeId
            && string.Equals(x.Code, dto.Code, StringComparison.OrdinalIgnoreCase));

        if (duplicate)
        {
            throw new InvalidOperationException("Une classe avec le même code existe déjà pour cette année, ce parcours et ce niveau.");
        }

        if (dto.Id == 0)
        {
            await classesPedagogiques.AddAsync(new ClassePedagogique
            {
                AnneeAcademiqueId = dto.AnneeAcademiqueId,
                ParcoursAcademiqueId = dto.ParcoursAcademiqueId,
                NiveauEtudeId = dto.NiveauEtudeId,
                MaquettePedagogiqueId = dto.MaquettePedagogiqueId,
                Code = dto.Code,
                Libelle = dto.Libelle,
                EstActive = dto.EstActive
            }, cancellationToken);
        }
        else
        {
            var entity = await classesPedagogiques.GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null) return;

            entity.AnneeAcademiqueId = dto.AnneeAcademiqueId;
            entity.ParcoursAcademiqueId = dto.ParcoursAcademiqueId;
            entity.NiveauEtudeId = dto.NiveauEtudeId;
            entity.MaquettePedagogiqueId = dto.MaquettePedagogiqueId;
            entity.Code = dto.Code;
            entity.Libelle = dto.Libelle;
            entity.EstActive = dto.EstActive;
        }

        await classesPedagogiques.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteClassePedagogiqueAsync(long id, CancellationToken cancellationToken = default)
    {
        await classesPedagogiques.DeleteByIdAsync(id, cancellationToken);
        await classesPedagogiques.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<LookupDto>> GetAnneesAcademiquesLookupAsync(CancellationToken cancellationToken = default)
    {
        var items = await anneesAcademiques.ListAsync(cancellationToken);

        return items
            .OrderByDescending(x => x.AnneeDebut)
            .Select(x => new LookupDto { Id = x.Id, Libelle = x.Libelle })
            .ToList();
    }

    public async Task<List<LookupDto>> GetParcoursAcademiquesLookupAsync(CancellationToken cancellationToken = default)
    {
        var items = await parcoursAcademiques.ListAsync(cancellationToken);

        return items
            .Where(x => x.EstActive)
            .OrderBy(x => x.Libelle)
            .Select(x => new LookupDto { Id = x.Id, Libelle = FormatCodeLibelle(x.Code, x.Libelle) })
            .ToList();
    }

    public async Task<List<LookupDto>> GetNiveauxEtudeLookupAsync(CancellationToken cancellationToken = default)
    {
        var items = await niveauxEtude.ListAsync(cancellationToken);

        return items
            .Where(x => x.EstActif)
            .OrderBy(x => x.Numero)
            .Select(x => new LookupDto { Id = x.Id, Libelle = x.Libelle })
            .ToList();
    }

    public async Task<List<LookupDto>> GetMaquettesPedagogiquesLookupAsync(CancellationToken cancellationToken = default)
    {
        var items = await maquettesPedagogiques.ListAsync(cancellationToken);

        return items
            .OrderBy(x => x.Libelle)
            .ThenBy(x => x.Version)
            .Select(x => new LookupDto { Id = x.Id, Libelle = $"{x.Libelle} - {x.Version}" })
            .ToList();
    }

    private async Task EnsureReferencesExistAsync(ClassePedagogiqueDto dto, CancellationToken cancellationToken)
    {
        var anneeExists = (await anneesAcademiques.ListAsync(cancellationToken)).Any(x => x.Id == dto.AnneeAcademiqueId);
        if (!anneeExists)
        {
            throw new InvalidOperationException("L'année académique sélectionnée est introuvable.");
        }

        var ouverture = (await parcoursAcademiques.ListAsync(cancellationToken)).FirstOrDefault(x => x.Id == dto.ParcoursAcademiqueId);
        if (ouverture is null)
        {
            throw new InvalidOperationException("Le parcours cycle/filière/spécialité sélectionné est introuvable.");
        }

        var niveauExists = (await niveauxEtude.ListAsync(cancellationToken)).Any(x => x.Id == dto.NiveauEtudeId);
        if (!niveauExists)
        {
            throw new InvalidOperationException("Le niveau sélectionné est introuvable.");
        }

        if (dto.MaquettePedagogiqueId is not null)
        {
            var maquette = (await maquettesPedagogiques.ListAsync(cancellationToken))
                .FirstOrDefault(x => x.Id == dto.MaquettePedagogiqueId);

            if (maquette is null)
            {
                throw new InvalidOperationException("La maquette pédagogique sélectionnée est introuvable.");
            }

            if (!IsMaquetteCompatibleWithParcours(maquette, ouverture))
            {
                throw new InvalidOperationException("La maquette sélectionnée n'appartient pas au parcours de la classe.");
            }
        }
    }

    private async Task<AnneeAcademique?> GetDefaultAnneeAcademiqueAsync(CancellationToken cancellationToken)
    {
        var annees = await anneesAcademiques.ListAsync(cancellationToken);

        return annees
            .OrderByDescending(x => x.EstActive)
            .ThenByDescending(x => x.AnneeDebut)
            .FirstOrDefault();
    }

    private static ClassePedagogiqueDto ToDto(
        ClassePedagogique classe,
        IReadOnlyCollection<AnneeAcademique> annees,
        IReadOnlyCollection<ParcoursAcademique> ouvertures,
        IReadOnlyCollection<NiveauEtude> niveaux,
        IReadOnlyCollection<MaquettePedagogique> maquettes,
        IReadOnlyCollection<Inscription> inscriptions)
    {
        var annee = annees.FirstOrDefault(x => x.Id == classe.AnneeAcademiqueId);
        var ouverture = ouvertures.FirstOrDefault(x => x.Id == classe.ParcoursAcademiqueId);
        var niveau = niveaux.FirstOrDefault(x => x.Id == classe.NiveauEtudeId);
        var maquette = classe.MaquettePedagogiqueId is null
            ? null
            : maquettes.FirstOrDefault(x => x.Id == classe.MaquettePedagogiqueId);

        return new ClassePedagogiqueDto
        {
            Id = classe.Id,
            AnneeAcademiqueId = classe.AnneeAcademiqueId,
            AnneeAcademiqueLibelle = annee?.Libelle ?? string.Empty,
            ParcoursAcademiqueId = classe.ParcoursAcademiqueId,
            ParcoursAcademiqueLibelle = ouverture is null
                ? string.Empty
                : FormatCodeLibelle(ouverture.Code, ouverture.Libelle),
            NiveauEtudeId = classe.NiveauEtudeId,
            NiveauEtudeLibelle = niveau?.Libelle ?? string.Empty,
            MaquettePedagogiqueId = classe.MaquettePedagogiqueId,
            MaquettePedagogiqueLibelle = maquette is null ? null : $"{maquette.Libelle} - {maquette.Version}",
            Code = classe.Code,
            Libelle = classe.Libelle,
            EstActive = classe.EstActive,
            Effectif = inscriptions.Count(x => x.ClassePedagogiqueId == classe.Id && x.Statut == StatutInscription.Validee)
        };
    }

    private static string FormatCodeLibelle(string code, string libelle)
        => string.IsNullOrWhiteSpace(code) ? libelle : $"{code} - {libelle}";

    private static bool IsMaquetteCompatibleWithParcours(MaquettePedagogique maquette, ParcoursAcademique parcours)
        => maquette.CycleFormationId == parcours.CycleFormationId
            && maquette.NiveauEtudeId == parcours.NiveauEtudeId
            && maquette.FiliereId == parcours.FiliereId
            && maquette.SpecialiteId == parcours.SpecialiteId;

    private static string RequireText(string? value, string errorMessage)
    {
        var normalized = value?.Trim();

        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new InvalidOperationException(errorMessage);
        }

        return normalized;
    }
}
