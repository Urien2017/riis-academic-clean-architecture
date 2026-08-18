using RIIS.Academic.Application.Abstractions.Persistence;
using RIIS.Academic.Application.Scolarite.Dtos;
using RIIS.Academic.Domain;
using System.Globalization;
using System.Text;

namespace RIIS.Academic.Application.Scolarite.Services;

public class TypesElementsScolariteService(
    IRepository<TypeElementScolarite> typesElementsScolarite) : ITypesElementsScolariteService
{
    public async Task<List<TypeElementScolariteDto>> GetTypesElementsScolariteAsync(
        bool inclureInactifs = false,
        CancellationToken cancellationToken = default)
    {
        var items = await typesElementsScolarite.ListAsync(cancellationToken);

        return items
            .Where(x => inclureInactifs || x.EstActif)
            .OrderBy(x => x.OrdreAffichage)
            .ThenBy(x => x.Libelle)
            .Select(ToDto)
            .ToList();
    }

    public async Task<TypeElementScolariteDto?> GetTypeElementScolariteAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await typesElementsScolarite.GetByIdAsync(id, cancellationToken);

        return entity is null ? null : ToDto(entity);
    }

    public TypeElementScolariteDto CreateDefaultTypeElementScolarite()
        => new()
        {
            Categorie = CategorieTypeElementScolarite.Frais,
            EstPayable = true,
            EstObligatoire = true,
            EstActif = true
        };

    public string GenerateCodeFromLibelle(string? libelle)
    {
        var normalized = RemoveDiacritics(RequireText(libelle, "Le libelle du type d'element est obligatoire."))
            .ToUpperInvariant();
        var words = normalized
            .Split([' ', '-', '_', '\'', '/', '\\', '.', ','], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(x => new string(x.Where(char.IsLetterOrDigit).ToArray()))
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        if (words.Count == 0)
        {
            throw new InvalidOperationException("Le libelle du type d'element doit contenir au moins une lettre ou un chiffre.");
        }

        return words.Count == 1
            ? words[0][..Math.Min(3, words[0].Length)]
            : string.Concat(words.Select(x => x[0]))[..Math.Min(8, words.Count)];
    }

    public async Task SaveTypeElementScolariteAsync(
        TypeElementScolariteDto dto,
        CancellationToken cancellationToken = default)
    {
        dto.Libelle = RequireText(dto.Libelle, "Le libellé du type d'élément est obligatoire.");
        dto.Code = GenerateCodeFromLibelle(dto.Libelle);

        if (!dto.EstPayable && !dto.EstDocumentaire && !dto.EstSoumisValidation)
        {
            throw new InvalidOperationException(
                "Un type d'élément doit être payable, documentaire ou soumis à validation.");
        }

        var items = await typesElementsScolarite.ListAsync(cancellationToken);
        var duplicate = items.Any(x =>
            x.Id != dto.Id
            && string.Equals(x.Code, dto.Code, StringComparison.OrdinalIgnoreCase));

        if (duplicate)
        {
            throw new InvalidOperationException("Un type d'élément scolarité existe déjà avec ce code.");
        }

        if (dto.Id == 0)
        {
            await typesElementsScolarite.AddAsync(new TypeElementScolarite
            {
                Code = dto.Code,
                Libelle = dto.Libelle,
                Categorie = dto.Categorie,
                EstPayable = dto.EstPayable,
                EstDocumentaire = dto.EstDocumentaire,
                EstSoumisValidation = dto.EstSoumisValidation,
                EstObligatoire = dto.EstObligatoire,
                OrdreAffichage = dto.OrdreAffichage,
                EstActif = dto.EstActif
            }, cancellationToken);
        }
        else
        {
            var entity = await typesElementsScolarite.GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null) return;

            entity.Code = dto.Code;
            entity.Libelle = dto.Libelle;
            entity.Categorie = dto.Categorie;
            entity.EstPayable = dto.EstPayable;
            entity.EstDocumentaire = dto.EstDocumentaire;
            entity.EstSoumisValidation = dto.EstSoumisValidation;
            entity.EstObligatoire = dto.EstObligatoire;
            entity.OrdreAffichage = dto.OrdreAffichage;
            entity.EstActif = dto.EstActif;
        }

        await typesElementsScolarite.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteTypeElementScolariteAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        await typesElementsScolarite.DeleteByIdAsync(id, cancellationToken);
        await typesElementsScolarite.SaveChangesAsync(cancellationToken);
    }

    private static TypeElementScolariteDto ToDto(TypeElementScolarite entity)
        => new()
        {
            Id = entity.Id,
            Code = entity.Code,
            Libelle = entity.Libelle,
            Categorie = entity.Categorie,
            EstPayable = entity.EstPayable,
            EstDocumentaire = entity.EstDocumentaire,
            EstSoumisValidation = entity.EstSoumisValidation,
            EstObligatoire = entity.EstObligatoire,
            OrdreAffichage = entity.OrdreAffichage,
            EstActif = entity.EstActif
        };

    private static string RequireText(string? value, string errorMessage)
    {
        var normalized = value?.Trim();

        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new InvalidOperationException(errorMessage);
        }

        return normalized;
    }

    private static string RemoveDiacritics(string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(normalized.Length);

        foreach (var character in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(character);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}
