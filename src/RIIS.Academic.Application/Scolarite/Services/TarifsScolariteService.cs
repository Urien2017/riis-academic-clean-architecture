using RIIS.Academic.Application.Abstractions.Persistence;
using RIIS.Academic.Application.Scolarite.Dtos;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Scolarite.Services;

public class TarifsScolariteService(
    IRepository<TarifScolarite> tarifsScolarite,
    IRepository<TypeElementScolarite> typesElementsScolarite,
    IRepository<ParcoursAcademique> parcoursAcademiques) : ITarifsScolariteService
{
    public async Task<List<TarifScolariteDto>> GetTarifsScolariteAsync(
        long? typeElementScolariteId = null,
        long? parcoursAcademiqueId = null,
        bool inclureInactifs = false,
        CancellationToken cancellationToken = default)
    {
        var items = await tarifsScolarite.ListAsync(cancellationToken);
        var types = await typesElementsScolarite.ListAsync(cancellationToken);
        var parcours = await parcoursAcademiques.ListAsync(cancellationToken);

        if (typeElementScolariteId is not null)
        {
            items = items.Where(x => x.TypeElementScolariteId == typeElementScolariteId).ToList();
        }

        if (parcoursAcademiqueId is not null)
        {
            items = items.Where(x => x.ParcoursAcademiqueId == parcoursAcademiqueId).ToList();
        }

        return items
            .Where(x => inclureInactifs || x.EstActif)
            .Select(x => ToDto(x, types, parcours))
            .OrderBy(x => x.TypeElementScolariteLibelle)
            .ThenBy(x => x.ParcoursAcademiqueLibelle)
            .ThenByDescending(x => x.Priorite)
            .ToList();
    }

    public async Task<TarifScolariteDto?> GetTarifScolariteAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await tarifsScolarite.GetByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        var types = await typesElementsScolarite.ListAsync(cancellationToken);
        var parcours = await parcoursAcademiques.ListAsync(cancellationToken);

        return ToDto(entity, types, parcours);
    }

    public TarifScolariteDto CreateDefaultTarifScolarite(long? typeElementScolariteId = null)
        => new()
        {
            TypeElementScolariteId = typeElementScolariteId ?? 0,
            Devise = "XAF",
            EstActif = true
        };

    public async Task SaveTarifScolariteAsync(
        TarifScolariteDto dto,
        CancellationToken cancellationToken = default)
    {
        if (dto.TypeElementScolariteId <= 0)
        {
            throw new InvalidOperationException("Le type d'élément du tarif est obligatoire.");
        }

        if (dto.ParcoursAcademiqueId <= 0)
        {
            throw new InvalidOperationException("Le parcours académique du tarif est obligatoire.");
        }

        if (dto.Montant <= 0)
        {
            throw new InvalidOperationException("Le montant du tarif doit être supérieur à zéro.");
        }

        var type = await EnsureTypeExistsAsync(dto.TypeElementScolariteId, cancellationToken);
        var parcours = await EnsureParcoursExistsAsync(dto.ParcoursAcademiqueId, cancellationToken);

        dto.TypeElementScolariteCode = type.Code;
        dto.TypeElementScolariteLibelle = FormatCodeLibelle(type.Code, type.Libelle);
        dto.ParcoursAcademiqueLibelle = parcours.Libelle;
        dto.Code = $"{type.Code}-{parcours.Code}";

        await EnsureNoDuplicateTarifAsync(dto, cancellationToken);

        if (dto.Id == 0)
        {
            await tarifsScolarite.AddAsync(new TarifScolarite
            {
                Code = dto.Code,
                TypeElementScolariteId = dto.TypeElementScolariteId,
                ParcoursAcademiqueId = dto.ParcoursAcademiqueId,
                Montant = dto.Montant,
                Devise = dto.Devise,
                Priorite = dto.Priorite,
                EstActif = dto.EstActif
            }, cancellationToken);
        }
        else
        {
            var entity = await tarifsScolarite.GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null) return;

            entity.Code = dto.Code;
            entity.TypeElementScolariteId = dto.TypeElementScolariteId;
            entity.ParcoursAcademiqueId = dto.ParcoursAcademiqueId;
            entity.Montant = dto.Montant;
            entity.Devise = dto.Devise;
            entity.Priorite = dto.Priorite;
            entity.EstActif = dto.EstActif;
        }

        await tarifsScolarite.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteTarifScolariteAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        await tarifsScolarite.DeleteByIdAsync(id, cancellationToken);
        await tarifsScolarite.SaveChangesAsync(cancellationToken);
    }

    public async Task<TarifScolariteDto?> ResolveTarifScolariteAsync(
        long typeElementScolariteId,
        long parcoursAcademiqueId,
        CancellationToken cancellationToken = default)
    {
        if (typeElementScolariteId <= 0)
        {
            throw new InvalidOperationException("Le type d'élément du tarif est obligatoire.");
        }

        if (parcoursAcademiqueId <= 0)
        {
            throw new InvalidOperationException("Le parcours académique du tarif est obligatoire.");
        }

        var items = await tarifsScolarite.ListAsync(cancellationToken);
        var types = await typesElementsScolarite.ListAsync(cancellationToken);
        var parcours = await parcoursAcademiques.ListAsync(cancellationToken);

        var resolved = items
            .FirstOrDefault(x => x.EstActif
                && x.TypeElementScolariteId == typeElementScolariteId
                && x.ParcoursAcademiqueId == parcoursAcademiqueId
                && x.Priorite == items
                    .Where(y => y.EstActif
                        && y.TypeElementScolariteId == typeElementScolariteId
                        && y.ParcoursAcademiqueId == parcoursAcademiqueId)
                    .Max(y => y.Priorite));

        return resolved is null ? null : ToDto(resolved, types, parcours);
    }

    private async Task<TypeElementScolarite> EnsureTypeExistsAsync(long typeElementScolariteId, CancellationToken cancellationToken)
    {
        var type = await typesElementsScolarite.GetByIdAsync(typeElementScolariteId, cancellationToken);
        if (type is null)
        {
            throw new InvalidOperationException("Le type d'élément scolarité sélectionné est introuvable.");
        }
        return type;
    }

    private async Task<ParcoursAcademique> EnsureParcoursExistsAsync(long parcoursAcademiqueId, CancellationToken cancellationToken)
    {
        var parcours = await parcoursAcademiques.GetByIdAsync(parcoursAcademiqueId, cancellationToken);
        if (parcours is null)
        {
            throw new InvalidOperationException("Le parcours académique sélectionné est introuvable.");
        }
        return parcours;
    }

    private async Task EnsureNoDuplicateTarifAsync(TarifScolariteDto dto, CancellationToken cancellationToken)
    {
        var items = await tarifsScolarite.ListAsync(cancellationToken);
        var duplicate = items.Any(x =>
            x.Id != dto.Id
            && x.TypeElementScolariteId == dto.TypeElementScolariteId
            && x.ParcoursAcademiqueId == dto.ParcoursAcademiqueId);

        if (duplicate)
        {
            throw new InvalidOperationException(
                "Un tarif existe déjà pour ce type d'élément et ce parcours.");
        }
    }

    private static TarifScolariteDto ToDto(
        TarifScolarite entity,
        IReadOnlyCollection<TypeElementScolarite> types,
        IReadOnlyCollection<ParcoursAcademique> parcours)
    {
        var type = types.FirstOrDefault(x => x.Id == entity.TypeElementScolariteId);
        var parcoursItem = parcours.FirstOrDefault(x => x.Id == entity.ParcoursAcademiqueId);

        return new TarifScolariteDto
        {
            Id = entity.Id,
            Code = entity.Code,
            TypeElementScolariteId = entity.TypeElementScolariteId,
            TypeElementScolariteCode = type?.Code ?? string.Empty,
            TypeElementScolariteLibelle = type is null
                ? string.Empty
                : FormatCodeLibelle(type.Code, type.Libelle),
            ParcoursAcademiqueId = entity.ParcoursAcademiqueId,
            ParcoursAcademiqueLibelle = parcoursItem?.Libelle ?? string.Empty,
            Montant = entity.Montant,
            Devise = entity.Devise,
            Priorite = entity.Priorite,
            EstActif = entity.EstActif
        };
    }

    private static string FormatCodeLibelle(string code, string libelle)
        => string.IsNullOrWhiteSpace(code) ? libelle : $"{code} - {libelle}";
}
