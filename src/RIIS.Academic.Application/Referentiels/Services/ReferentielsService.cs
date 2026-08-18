using RIIS.Academic.Application.Abstractions.Persistence;
using RIIS.Academic.Application.Referentiels.Dtos;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Referentiels.Services;

public class ReferentielsService(
    IRepository<AnneeAcademique> anneesAcademiques,
    IRepository<CycleFormation> cyclesFormation,
    IRepository<ParcoursAcademique> parcoursAcademiques,
    IRepository<Filiere> filieres,
    IRepository<Specialite> specialites,
    IRepository<NiveauEtude> niveauxEtude) : IReferentielsService
{
    public async Task<List<AnneeAcademiqueDto>> GetAnneesAcademiquesAsync(CancellationToken cancellationToken = default)
    {
        var items = await anneesAcademiques.ListAsync(cancellationToken);

        return items
            .OrderByDescending(x => x.AnneeDebut)
            .Select(x => new AnneeAcademiqueDto
            {
                Id = x.Id,
                Libelle = x.Libelle,
                AnneeDebut = x.AnneeDebut,
                AnneeFin = x.AnneeFin,
                EstActive = x.EstActive
            })
            .ToList();
    }

    public AnneeAcademiqueDto CreateDefaultAnneeAcademique()
    {
        var anneeDebut = (short)DateTime.Today.Year;

        return new AnneeAcademiqueDto
        {
            Libelle = $"{anneeDebut}-{anneeDebut + 1}",
            AnneeDebut = anneeDebut,
            AnneeFin = (short)(anneeDebut + 1),
            EstActive = true
        };
    }

    public async Task SaveAnneeAcademiqueAsync(AnneeAcademiqueDto dto, CancellationToken cancellationToken = default)
    {
        dto.Libelle = RequireText(dto.Libelle, "Le libellé de l'année académique est obligatoire.");

        if (dto.AnneeFin != dto.AnneeDebut + 1)
        {
            throw new InvalidOperationException("L'année de fin doit être égale à l'année de début + 1.");
        }

        if (dto.Id == 0)
        {
            await anneesAcademiques.AddAsync(new AnneeAcademique
            {
                Libelle = dto.Libelle,
                AnneeDebut = dto.AnneeDebut,
                AnneeFin = dto.AnneeFin,
                EstActive = dto.EstActive
            }, cancellationToken);
        }
        else
        {
            var entity = await anneesAcademiques.GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null) return;

            entity.Libelle = dto.Libelle;
            entity.AnneeDebut = dto.AnneeDebut;
            entity.AnneeFin = dto.AnneeFin;
            entity.EstActive = dto.EstActive;
        }

        await anneesAcademiques.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAnneeAcademiqueAsync(long id, CancellationToken cancellationToken = default)
    {
        await anneesAcademiques.DeleteByIdAsync(id, cancellationToken);
        await anneesAcademiques.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<CycleFormationDto>> GetCyclesFormationAsync(CancellationToken cancellationToken = default)
    {
        var items = await cyclesFormation.ListAsync(cancellationToken);

        return items
            .OrderBy(x => x.OrdreAffichage)
            .ThenBy(x => x.Libelle)
            .Select(x => new CycleFormationDto
            {
                Id = x.Id,
                Code = x.Code,
                Libelle = x.Libelle,
                OrdreAffichage = x.OrdreAffichage,
                EstActif = x.EstActif
            })
            .ToList();
    }

    public async Task<List<ParcoursFormationDto>> GetParcoursFormationAsync(
        bool inclureInactifs = false,
        CancellationToken cancellationToken = default)
    {
        var parcoursItems = await parcoursAcademiques.ListAsync(cancellationToken);
        var cycles = await cyclesFormation.ListAsync(cancellationToken);
        var filiereItems = await filieres.ListAsync(cancellationToken);
        var specialiteItems = await specialites.ListAsync(cancellationToken);

        return parcoursItems
            .Where(x => inclureInactifs || x.EstActive)
            .Select(parcours =>
            {
                var cycle = cycles.FirstOrDefault(x => x.Id == parcours.CycleFormationId);
                var filiere = filiereItems.FirstOrDefault(x => x.Id == parcours.FiliereId);
                var specialite = specialiteItems.FirstOrDefault(x => x.Id == parcours.SpecialiteId)
                    ?? throw new InvalidOperationException(
                        $"Le parcours '{parcours.Code}' n'est rattaché à aucune spécialité valide.");

                if (cycle is null || filiere is null)
                {
                    return null;
                }

                return new ParcoursFormationDto
                {
                    Id = parcours.Id,
                    Code = parcours.Code,
                    Libelle = parcours.Libelle,
                    CycleFormationId = parcours.CycleFormationId,
                    CycleCode = cycle.Code,
                    CycleLibelle = cycle.Libelle,
                    FiliereId = parcours.FiliereId,
                    FiliereCode = filiere.Code,
                    FiliereLibelle = filiere.Libelle,
                    SpecialiteId = parcours.SpecialiteId,
                    SpecialiteCode = specialite.Code,
                    SpecialiteLibelle = specialite.Libelle,
                    EstActive = parcours.EstActive
                };
            })
            .OfType<ParcoursFormationDto>()
            .OrderBy(x => x.CycleLibelle)
            .ThenBy(x => x.FiliereLibelle)
            .ThenBy(x => x.SpecialiteLibelle)
            .ToList();
    }

    public async Task<List<ParcoursAcademiqueDto>> GetParcoursAcademiquesAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? niveauEtudeId = null,
        long? filiereId = null,
        long? specialiteId = null,
        bool inclureInactifs = false,
        CancellationToken cancellationToken = default)
    {
        var parcoursItems = await parcoursAcademiques.ListAsync(cancellationToken);
        var anneeItems = await anneesAcademiques.ListAsync(cancellationToken);
        var cycleItems = await cyclesFormation.ListAsync(cancellationToken);
        var niveauItems = await niveauxEtude.ListAsync(cancellationToken);
        var filiereItems = await filieres.ListAsync(cancellationToken);
        var specialiteItems = await specialites.ListAsync(cancellationToken);
        IEnumerable<ParcoursAcademique> query = parcoursItems;

        if (anneeAcademiqueId.HasValue)
        {
            query = query.Where(x => x.AnneeAcademiqueId == anneeAcademiqueId.Value);
        }

        if (cycleFormationId.HasValue)
        {
            query = query.Where(x => x.CycleFormationId == cycleFormationId.Value);
        }

        if (niveauEtudeId.HasValue)
        {
            query = query.Where(x => x.NiveauEtudeId == niveauEtudeId.Value);
        }

        if (filiereId.HasValue)
        {
            query = query.Where(x => x.FiliereId == filiereId.Value);
        }

        if (specialiteId.HasValue)
        {
            query = query.Where(x => x.SpecialiteId == specialiteId.Value);
        }

        if (!inclureInactifs)
        {
            query = query.Where(x => x.EstActive);
        }

        return query
            .Select(parcours => MapParcoursAcademique(
                parcours,
                anneeItems,
                cycleItems,
                niveauItems,
                filiereItems,
                specialiteItems))
            .OfType<ParcoursAcademiqueDto>()
            .OrderByDescending(x => x.AnneeDebut)
            .ThenBy(x => x.CycleLibelle)
            .ThenBy(x => x.NiveauNumero)
            .ThenBy(x => x.FiliereLibelle)
            .ThenBy(x => x.SpecialiteLibelle)
            .ToList();
    }

    public ParcoursAcademiqueDto CreateDefaultParcoursAcademique(long? anneeAcademiqueId = null)
        => new()
        {
            AnneeAcademiqueId = anneeAcademiqueId ?? 0,
            EstActive = true
        };

    public async Task SaveParcoursAcademiqueAsync(ParcoursAcademiqueDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.AnneeAcademiqueId <= 0)
        {
            throw new InvalidOperationException("L'annee academique est obligatoire.");
        }

        if (dto.CycleFormationId <= 0)
        {
            throw new InvalidOperationException("Le cycle est obligatoire.");
        }

        if (dto.NiveauEtudeId <= 0)
        {
            throw new InvalidOperationException("Le niveau est obligatoire.");
        }

        if (dto.FiliereId <= 0)
        {
            throw new InvalidOperationException("La filiere est obligatoire.");
        }

        if (dto.SpecialiteId <= 0)
        {
            throw new InvalidOperationException("La specialite est obligatoire.");
        }

        var annee = await GetRequiredAsync(anneesAcademiques, dto.AnneeAcademiqueId, "Annee academique introuvable.", cancellationToken);
        var cycle = await GetRequiredAsync(cyclesFormation, dto.CycleFormationId, "Cycle introuvable.", cancellationToken);
        var niveau = await GetRequiredAsync(niveauxEtude, dto.NiveauEtudeId, "Niveau introuvable.", cancellationToken);
        var filiere = await GetRequiredAsync(filieres, dto.FiliereId, "Filiere introuvable.", cancellationToken);
        var specialite = await GetRequiredAsync(specialites, dto.SpecialiteId, "Specialite introuvable.", cancellationToken);

        if (specialite.FiliereId != filiere.Id)
        {
            throw new InvalidOperationException("La specialite choisie n'appartient pas a la filiere selectionnee.");
        }

        var parcoursItems = await parcoursAcademiques.ListAsync(cancellationToken);
        var duplicate = parcoursItems.FirstOrDefault(x =>
            x.Id != dto.Id
            && x.AnneeAcademiqueId == dto.AnneeAcademiqueId
            && x.CycleFormationId == dto.CycleFormationId
            && x.NiveauEtudeId == dto.NiveauEtudeId
            && x.FiliereId == dto.FiliereId
            && x.SpecialiteId == dto.SpecialiteId);

        if (duplicate is not null)
        {
            throw new InvalidOperationException("Un parcours academique existe deja pour cette combinaison.");
        }

        var code = GenerateParcoursAcademiqueCode(annee, cycle, niveau, filiere, specialite);
        var libelle = string.IsNullOrWhiteSpace(dto.Libelle)
            ? GenerateParcoursAcademiqueLibelle(annee, cycle, niveau, filiere, specialite)
            : dto.Libelle.Trim();

        if (dto.Id == 0)
        {
            await parcoursAcademiques.AddAsync(new ParcoursAcademique
            {
                AnneeAcademiqueId = dto.AnneeAcademiqueId,
                CycleFormationId = dto.CycleFormationId,
                NiveauEtudeId = dto.NiveauEtudeId,
                FiliereId = dto.FiliereId,
                SpecialiteId = dto.SpecialiteId,
                Code = code,
                Libelle = libelle,
                EstActive = dto.EstActive
            }, cancellationToken);
        }
        else
        {
            var entity = await parcoursAcademiques.GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null) return;

            entity.AnneeAcademiqueId = dto.AnneeAcademiqueId;
            entity.CycleFormationId = dto.CycleFormationId;
            entity.NiveauEtudeId = dto.NiveauEtudeId;
            entity.FiliereId = dto.FiliereId;
            entity.SpecialiteId = dto.SpecialiteId;
            entity.Code = code;
            entity.Libelle = libelle;
            entity.EstActive = dto.EstActive;
        }

        await parcoursAcademiques.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteParcoursAcademiqueAsync(long id, CancellationToken cancellationToken = default)
    {
        await parcoursAcademiques.DeleteByIdAsync(id, cancellationToken);
        await parcoursAcademiques.SaveChangesAsync(cancellationToken);
    }

    public CycleFormationDto CreateDefaultCycleFormation() => new();

    public async Task SaveCycleFormationAsync(CycleFormationDto dto, CancellationToken cancellationToken = default)
    {
        dto.Code = NormalizeCode(dto.Code, "Le code du cycle est obligatoire.");
        dto.Libelle = RequireText(dto.Libelle, "Le libellé du cycle est obligatoire.");

        if (dto.Id == 0)
        {
            await cyclesFormation.AddAsync(new CycleFormation
            {
                Code = dto.Code,
                Libelle = dto.Libelle,
                OrdreAffichage = dto.OrdreAffichage,
                EstActif = dto.EstActif
            }, cancellationToken);
        }
        else
        {
            var entity = await cyclesFormation.GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null) return;

            entity.Code = dto.Code;
            entity.Libelle = dto.Libelle;
            entity.OrdreAffichage = dto.OrdreAffichage;
            entity.EstActif = dto.EstActif;
        }

        await cyclesFormation.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCycleFormationAsync(long id, CancellationToken cancellationToken = default)
    {
        await cyclesFormation.DeleteByIdAsync(id, cancellationToken);
        await cyclesFormation.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<FiliereDto>> GetFilieresAsync(CancellationToken cancellationToken = default)
    {
        var items = await filieres.ListAsync(cancellationToken);

        return items
            .OrderBy(x => x.Libelle)
            .Select(x => new FiliereDto
            {
                Id = x.Id,
                Code = x.Code,
                Libelle = x.Libelle,
                EstActive = x.EstActive
            })
            .ToList();
    }

    public FiliereDto CreateDefaultFiliere() => new();

    public async Task SaveFiliereAsync(FiliereDto dto, CancellationToken cancellationToken = default)
    {
        dto.Code = NormalizeCode(dto.Code, "Le code de la filière est obligatoire.");
        dto.Libelle = RequireText(dto.Libelle, "Le libellé de la filière est obligatoire.");

        if (dto.Id == 0)
        {
            await filieres.AddAsync(new Filiere
            {
                Code = dto.Code,
                Libelle = dto.Libelle,
                EstActive = dto.EstActive
            }, cancellationToken);
        }
        else
        {
            var entity = await filieres.GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null) return;

            entity.Code = dto.Code;
            entity.Libelle = dto.Libelle;
            entity.EstActive = dto.EstActive;
        }

        await filieres.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteFiliereAsync(long id, CancellationToken cancellationToken = default)
    {
        await filieres.DeleteByIdAsync(id, cancellationToken);
        await filieres.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<SpecialiteDto>> GetSpecialitesAsync(CancellationToken cancellationToken = default)
    {
        var specialiteItems = await specialites.ListAsync(cancellationToken);
        var filiereItems = await filieres.ListAsync(cancellationToken);

        return specialiteItems
            .Join(
                filiereItems,
                specialite => specialite.FiliereId,
                filiere => filiere.Id,
                (specialite, filiere) => new SpecialiteDto
                {
                    Id = specialite.Id,
                    FiliereId = specialite.FiliereId,
                    FiliereLibelle = filiere.Libelle,
                    Code = specialite.Code,
                    Libelle = specialite.Libelle,
                    EstActive = specialite.EstActive
                })
            .OrderBy(x => x.FiliereLibelle)
            .ThenBy(x => x.Libelle)
            .ToList();
    }

    public async Task<List<FiliereLookupDto>> GetFilieresActivesLookupAsync(CancellationToken cancellationToken = default)
    {
        var items = await filieres.ListAsync(cancellationToken);

        return items
            .Where(x => x.EstActive)
            .OrderBy(x => x.Libelle)
            .Select(x => new FiliereLookupDto
            {
                Id = x.Id,
                Libelle = x.Libelle
            })
            .ToList();
    }

    public SpecialiteDto CreateDefaultSpecialite(long? filiereId = null, long? cycleFormationId = null)
        => new() { CycleFormationId = cycleFormationId, FiliereId = filiereId ?? 0 };

    public async Task<string> GenerateSpecialiteCodeAsync(string? libelle, CancellationToken cancellationToken = default)
    {
        var key = NormalizeLibelleKey(libelle);
        if (string.IsNullOrWhiteSpace(key))
        {
            return string.Empty;
        }

        var specialiteItems = await specialites.ListAsync(cancellationToken);
        var existingSpecialite = specialiteItems
            .Where(x => !string.IsNullOrWhiteSpace(x.Code) && NormalizeLibelleKey(x.Libelle) == key)
            .OrderByDescending(x => x.EstActive)
            .ThenBy(x => x.Id)
            .FirstOrDefault();

        if (existingSpecialite is not null)
        {
            return NormalizeCode(existingSpecialite.Code, "Le code de la spécialité est obligatoire.");
        }

        return GenerateSpecialiteCodeFallback(key);
    }

    private static string GenerateSpecialiteCodeFallback(string normalizedLibelle)
    {
        var ignoredWords = new HashSet<string> { "DE", "DES", "DU", "D", "ET", "EN", "A", "AU", "AUX", "LA", "LE", "LES" };
        var words = normalizedLibelle.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(word => !ignoredWords.Contains(word))
            .ToList();

        if (words.Count == 0)
        {
            return string.Empty;
        }

        var code = string.Concat(words.Select(word => word[0]));
        return code.Length > 8 ? code[..8] : code;
    }

    private static ParcoursAcademiqueDto? MapParcoursAcademique(
        ParcoursAcademique parcours,
        IReadOnlyCollection<AnneeAcademique> anneeItems,
        IReadOnlyCollection<CycleFormation> cycleItems,
        IReadOnlyCollection<NiveauEtude> niveauItems,
        IReadOnlyCollection<Filiere> filiereItems,
        IReadOnlyCollection<Specialite> specialiteItems)
    {
        var annee = anneeItems.FirstOrDefault(x => x.Id == parcours.AnneeAcademiqueId);
        var cycle = cycleItems.FirstOrDefault(x => x.Id == parcours.CycleFormationId);
        var niveau = niveauItems.FirstOrDefault(x => x.Id == parcours.NiveauEtudeId);
        var filiere = filiereItems.FirstOrDefault(x => x.Id == parcours.FiliereId);
        var specialite = specialiteItems.FirstOrDefault(x => x.Id == parcours.SpecialiteId);
        if (annee is null || cycle is null || niveau is null || filiere is null || specialite is null)
        {
            return null;
        }

        return new ParcoursAcademiqueDto
        {
            Id = parcours.Id,
            AnneeAcademiqueId = parcours.AnneeAcademiqueId,
            AnneeAcademiqueLibelle = annee.Libelle,
            AnneeDebut = annee.AnneeDebut,
            CycleFormationId = parcours.CycleFormationId,
            CycleCode = cycle.Code,
            CycleLibelle = cycle.Libelle,
            NiveauEtudeId = parcours.NiveauEtudeId,
            NiveauNumero = niveau.Numero,
            NiveauLibelle = niveau.Libelle,
            FiliereId = parcours.FiliereId,
            FiliereCode = filiere.Code,
            FiliereLibelle = filiere.Libelle,
            SpecialiteId = parcours.SpecialiteId,
            SpecialiteCode = specialite.Code,
            SpecialiteLibelle = specialite.Libelle,
            Code = parcours.Code,
            Libelle = parcours.Libelle,
            EstActive = parcours.EstActive
        };
    }

    private static string GenerateParcoursAcademiqueCode(
        AnneeAcademique annee,
        CycleFormation cycle,
        NiveauEtude niveau,
        Filiere filiere,
        Specialite specialite)
        => $"{annee.AnneeDebut % 100:00}-{cycle.Code}-N{niveau.Numero}-{filiere.Code}-{specialite.Code}".ToUpperInvariant();

    private static string GenerateParcoursAcademiqueLibelle(
        AnneeAcademique annee,
        CycleFormation cycle,
        NiveauEtude niveau,
        Filiere filiere,
        Specialite specialite)
        => $"{annee.Libelle} - {cycle.Libelle} {niveau.Libelle} - {filiere.Libelle} - {specialite.Libelle}";

    private static async Task<TEntity> GetRequiredAsync<TEntity>(
        IRepository<TEntity> repository,
        long id,
        string errorMessage,
        CancellationToken cancellationToken)
        where TEntity : class
        => await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException(errorMessage);

    public async Task SaveSpecialiteAsync(SpecialiteDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.FiliereId <= 0)
        {
            throw new InvalidOperationException("La filière de la spécialité est obligatoire.");
        }

        dto.Libelle = RequireText(dto.Libelle, "Le libellé de la spécialité est obligatoire.");
        dto.Code = dto.Id == 0 || string.IsNullOrWhiteSpace(dto.Code)
            ? await GenerateSpecialiteCodeAsync(dto.Libelle, cancellationToken)
            : NormalizeCode(dto.Code, "Le code de la spécialité est obligatoire.");
        dto.Code = NormalizeCode(dto.Code, "Le code de la spécialité est obligatoire.");

        Specialite? specialite;

        if (dto.Id == 0)
        {
            specialite = new Specialite
            {
                FiliereId = dto.FiliereId,
                Code = dto.Code,
                Libelle = dto.Libelle,
                EstActive = dto.EstActive
            };

            await specialites.AddAsync(specialite, cancellationToken);
        }
        else
        {
            specialite = await specialites.GetByIdAsync(dto.Id, cancellationToken);
            if (specialite is null) return;

            specialite.FiliereId = dto.FiliereId;
            specialite.Code = dto.Code;
            specialite.Libelle = dto.Libelle;
            specialite.EstActive = dto.EstActive;
        }

        await specialites.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSpecialiteAsync(long id, CancellationToken cancellationToken = default)
    {
        await specialites.DeleteByIdAsync(id, cancellationToken);
        await specialites.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<NiveauEtudeDto>> GetNiveauxEtudeAsync(CancellationToken cancellationToken = default)
    {
        var items = await niveauxEtude.ListAsync(cancellationToken);

        return items
            .OrderBy(x => x.Numero)
            .Select(x => new NiveauEtudeDto
            {
                Id = x.Id,
                Numero = x.Numero,
                Libelle = x.Libelle,
                EstActif = x.EstActif
            })
            .ToList();
    }

    public async Task<NiveauEtudeDto> CreateDefaultNiveauEtudeAsync(CancellationToken cancellationToken = default)
    {
        var items = await niveauxEtude.ListAsync(cancellationToken);
        var maxNumero = items.Count == 0 ? 0 : items.Max(x => x.Numero);
        var nextNumero = maxNumero >= byte.MaxValue ? byte.MaxValue : (byte)(maxNumero + 1);

        return new NiveauEtudeDto { Numero = nextNumero };
    }

    public async Task SaveNiveauEtudeAsync(NiveauEtudeDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.Numero < 1)
        {
            throw new InvalidOperationException("Le numéro du niveau doit être supérieur ou égal à 1.");
        }

        dto.Libelle = RequireText(dto.Libelle, "Le libellé du niveau est obligatoire.");

        if (dto.Id == 0)
        {
            await niveauxEtude.AddAsync(new NiveauEtude
            {
                Numero = dto.Numero,
                Libelle = dto.Libelle,
                EstActif = dto.EstActif
            }, cancellationToken);
        }
        else
        {
            var entity = await niveauxEtude.GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null) return;

            entity.Numero = dto.Numero;
            entity.Libelle = dto.Libelle;
            entity.EstActif = dto.EstActif;
        }

        await niveauxEtude.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteNiveauEtudeAsync(long id, CancellationToken cancellationToken = default)
    {
        await niveauxEtude.DeleteByIdAsync(id, cancellationToken);
        await niveauxEtude.SaveChangesAsync(cancellationToken);
    }

    private static string NormalizeCode(string? value, string errorMessage)
        => RequireText(value, errorMessage).ToUpperInvariant();

    private static string NormalizeLibelleKey(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var normalized = value.Trim().Normalize(System.Text.NormalizationForm.FormD);
        var builder = new System.Text.StringBuilder(normalized.Length);
        var previousWasSpace = true;

        foreach (var character in normalized)
        {
            if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(character) == System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            if (char.IsLetterOrDigit(character))
            {
                builder.Append(char.ToUpperInvariant(character));
                previousWasSpace = false;
            }
            else if (!previousWasSpace)
            {
                builder.Append(' ');
                previousWasSpace = true;
            }
        }

        return builder.ToString().Trim();
    }

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
