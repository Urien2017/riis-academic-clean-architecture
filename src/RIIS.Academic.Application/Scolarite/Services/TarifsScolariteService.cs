using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using RIIS.Academic.Application.Abstractions.Persistence;
using RIIS.Academic.Application.Scolarite.Dtos;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Scolarite.Services;

public class TarifsScolariteService(
    IRepository<TarifScolarite> tarifsScolarite,
    IRepository<TypeElementScolarite> typesElementsScolarite) : ITarifsScolariteService
{
    public async Task<List<TarifScolariteDto>> GetTarifsScolariteAsync(
        long? typeElementScolariteId = null,
        string? anneeAcademiqueCode = null,
        string? cycleCode = null,
        int? niveauNumero = null,
        string? filiereCode = null,
        string? specialiteCode = null,
        bool inclureInactifs = false,
        CancellationToken cancellationToken = default)
    {
        var items = await tarifsScolarite.ListAsync(cancellationToken);
        var types = await typesElementsScolarite.ListAsync(cancellationToken);

        var normalizedAnnee = NormalizeNullableCode(anneeAcademiqueCode);
        var normalizedCycle = NormalizeNullableCode(cycleCode);
        var normalizedFiliere = NormalizeNullableCode(filiereCode);
        var normalizedSpecialite = NormalizeNullableCode(specialiteCode);

        if (typeElementScolariteId is not null)
        {
            items = items.Where(x => x.TypeElementScolariteId == typeElementScolariteId).ToList();
        }

        if (normalizedAnnee is not null)
        {
            items = items.Where(x => x.AnneeAcademiqueCode == normalizedAnnee).ToList();
        }

        if (normalizedCycle is not null)
        {
            items = items.Where(x => x.CycleCode == normalizedCycle).ToList();
        }

        if (niveauNumero is not null)
        {
            items = items.Where(x => x.NiveauNumero == niveauNumero).ToList();
        }

        if (normalizedFiliere is not null)
        {
            items = items.Where(x => x.FiliereCode == normalizedFiliere).ToList();
        }

        if (normalizedSpecialite is not null)
        {
            items = items.Where(x => x.SpecialiteCode == normalizedSpecialite).ToList();
        }

        return items
            .Where(x => inclureInactifs || x.EstActif)
            .Select(x => ToDto(x, types))
            .OrderBy(x => x.TypeElementScolariteLibelle)
            .ThenByDescending(x => x.AnneeAcademiqueCode)
            .ThenByDescending(x => x.Priorite)
            .ThenBy(x => x.ContexteLibelle)
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

        return ToDto(entity, types);
    }

    public TarifScolariteDto CreateDefaultTarifScolarite(long? typeElementScolariteId = null)
        => new()
        {
            TypeElementScolariteId = typeElementScolariteId ?? 0,
            Devise = "XOF",
            DateDebutValidite = DateOnly.FromDateTime(DateTime.Today),
            EstActif = true
        };

    public string GenerateCode(TarifScolariteDto dto)
    {
        var typeSegment = NormalizeSegment(
            ResolveTypeCode(dto.TypeElementScolariteCode, dto.TypeElementScolariteLibelle),
            "Le type d'élément du tarif est obligatoire pour générer le code.");
        var parcoursSegment = BuildParcoursSegment(dto.CycleCode, dto.NiveauNumero);
        var anneeSegment = BuildAnneeSegment(dto.AnneeAcademiqueCode);

        return $"{typeSegment}-{parcoursSegment}-{anneeSegment}";
    }

    public async Task SaveTarifScolariteAsync(
        TarifScolariteDto dto,
        CancellationToken cancellationToken = default)
    {
        if (dto.TypeElementScolariteId <= 0)
        {
            throw new InvalidOperationException("Le type d'élément du tarif est obligatoire.");
        }

        dto.AnneeAcademiqueCode = NormalizeCode(dto.AnneeAcademiqueCode, "L'année académique du tarif est obligatoire.");
        dto.CycleCode = NormalizeNullableCode(dto.CycleCode);
        dto.FiliereCode = NormalizeNullableCode(dto.FiliereCode);
        dto.SpecialiteCode = NormalizeNullableCode(dto.SpecialiteCode);
        dto.Devise = NormalizeCode(dto.Devise, "La devise du tarif est obligatoire.");
        dto.Priorite = dto.Priorite <= 0 ? CalculateSpecificity(dto) : dto.Priorite;

        if (dto.Montant <= 0)
        {
            throw new InvalidOperationException("Le montant du tarif doit être supérieur à zéro.");
        }

        if (dto.NiveauNumero is <= 0)
        {
            throw new InvalidOperationException("Le niveau du tarif doit être supérieur à zéro.");
        }

        if (dto.DateFinValidite is not null && dto.DateFinValidite < dto.DateDebutValidite)
        {
            throw new InvalidOperationException("La date de fin de validité doit être postérieure à la date de début.");
        }

        var type = await EnsureTypeExistsAsync(dto.TypeElementScolariteId, cancellationToken);
        dto.TypeElementScolariteCode = type.Code;
        dto.TypeElementScolariteLibelle = FormatCodeLibelle(type.Code, type.Libelle);
        dto.Code = GenerateCode(dto);

        await EnsureNoDuplicateTarifAsync(dto, cancellationToken);

        if (dto.Id == 0)
        {
            await tarifsScolarite.AddAsync(new TarifScolarite
            {
                Code = dto.Code,
                TypeElementScolariteId = dto.TypeElementScolariteId,
                AnneeAcademiqueCode = dto.AnneeAcademiqueCode,
                CycleCode = dto.CycleCode,
                NiveauNumero = dto.NiveauNumero,
                FiliereCode = dto.FiliereCode,
                SpecialiteCode = dto.SpecialiteCode,
                Montant = dto.Montant,
                Devise = dto.Devise,
                DateDebutValidite = dto.DateDebutValidite,
                DateFinValidite = dto.DateFinValidite,
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
            entity.AnneeAcademiqueCode = dto.AnneeAcademiqueCode;
            entity.CycleCode = dto.CycleCode;
            entity.NiveauNumero = dto.NiveauNumero;
            entity.FiliereCode = dto.FiliereCode;
            entity.SpecialiteCode = dto.SpecialiteCode;
            entity.Montant = dto.Montant;
            entity.Devise = dto.Devise;
            entity.DateDebutValidite = dto.DateDebutValidite;
            entity.DateFinValidite = dto.DateFinValidite;
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
        TarifScolariteContexteDto contexte,
        CancellationToken cancellationToken = default)
    {
        if (typeElementScolariteId <= 0)
        {
            throw new InvalidOperationException("Le type d'élément du tarif est obligatoire.");
        }

        var anneeCode = NormalizeCode(contexte.AnneeAcademiqueCode, "L'année académique du contexte tarifaire est obligatoire.");
        var cycleCode = NormalizeNullableCode(contexte.CycleCode);
        var filiereCode = NormalizeNullableCode(contexte.FiliereCode);
        var specialiteCode = NormalizeNullableCode(contexte.SpecialiteCode);
        var dateReference = contexte.DateReference ?? DateOnly.FromDateTime(DateTime.Today);

        var items = await tarifsScolarite.ListAsync(cancellationToken);
        var types = await typesElementsScolarite.ListAsync(cancellationToken);

        var matchingContext = items
            .Where(x => x.EstActif
                && x.TypeElementScolariteId == typeElementScolariteId
                && x.AnneeAcademiqueCode == anneeCode
                && MatchesNullable(x.CycleCode, cycleCode)
                && MatchesNullable(x.NiveauNumero, contexte.NiveauNumero)
                && MatchesNullable(x.FiliereCode, filiereCode)
                && MatchesNullable(x.SpecialiteCode, specialiteCode))
            .ToList();

        var resolved = matchingContext
            .Where(x => x.DateDebutValidite <= dateReference
                && (x.DateFinValidite is null || x.DateFinValidite >= dateReference))
            .OrderByDescending(x => x.Priorite)
            .ThenByDescending(CalculateSpecificity)
            .ThenByDescending(x => x.DateDebutValidite)
            .FirstOrDefault()
            ?? matchingContext
                .OrderByDescending(x => x.Priorite)
                .ThenByDescending(CalculateSpecificity)
                .ThenByDescending(x => x.DateDebutValidite)
                .FirstOrDefault();

        return resolved is null ? null : ToDto(resolved, types);
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

    private async Task EnsureNoDuplicateTarifAsync(TarifScolariteDto dto, CancellationToken cancellationToken)
    {
        var items = await tarifsScolarite.ListAsync(cancellationToken);
        var duplicate = items.Any(x =>
            x.Id != dto.Id
            && x.TypeElementScolariteId == dto.TypeElementScolariteId
            && x.AnneeAcademiqueCode == dto.AnneeAcademiqueCode
            && x.CycleCode == dto.CycleCode
            && x.NiveauNumero == dto.NiveauNumero
            && x.FiliereCode == dto.FiliereCode
            && x.SpecialiteCode == dto.SpecialiteCode
            && x.DateDebutValidite == dto.DateDebutValidite);

        if (duplicate)
        {
            throw new InvalidOperationException(
                "Un tarif existe déjà pour ce type d'élément, ce contexte et cette date de début.");
        }
    }

    private static TarifScolariteDto ToDto(
        TarifScolarite entity,
        IReadOnlyCollection<TypeElementScolarite> types)
    {
        var type = types.FirstOrDefault(x => x.Id == entity.TypeElementScolariteId);

        return new TarifScolariteDto
        {
            Id = entity.Id,
            Code = entity.Code,
            TypeElementScolariteId = entity.TypeElementScolariteId,
            TypeElementScolariteCode = type?.Code ?? string.Empty,
            TypeElementScolariteLibelle = type is null
                ? string.Empty
                : FormatCodeLibelle(type.Code, type.Libelle),
            AnneeAcademiqueCode = entity.AnneeAcademiqueCode,
            CycleCode = entity.CycleCode,
            NiveauNumero = entity.NiveauNumero,
            FiliereCode = entity.FiliereCode,
            SpecialiteCode = entity.SpecialiteCode,
            Montant = entity.Montant,
            Devise = entity.Devise,
            DateDebutValidite = entity.DateDebutValidite,
            DateFinValidite = entity.DateFinValidite,
            Priorite = entity.Priorite,
            EstActif = entity.EstActif,
            ContexteLibelle = BuildContexteLibelle(entity)
        };
    }

    private static string BuildContexteLibelle(TarifScolarite tarif)
    {
        var parts = new List<string> { tarif.AnneeAcademiqueCode };

        if (!string.IsNullOrWhiteSpace(tarif.CycleCode)) parts.Add(tarif.CycleCode);
        if (tarif.NiveauNumero is not null) parts.Add($"Niveau {tarif.NiveauNumero}");
        if (!string.IsNullOrWhiteSpace(tarif.FiliereCode)) parts.Add(tarif.FiliereCode);
        if (!string.IsNullOrWhiteSpace(tarif.SpecialiteCode)) parts.Add(tarif.SpecialiteCode);

        return string.Join(" / ", parts);
    }

    private static bool MatchesNullable(string? tarifValue, string? contexteValue)
        => tarifValue is null || string.Equals(tarifValue, contexteValue, StringComparison.OrdinalIgnoreCase);

    private static bool MatchesNullable(int? tarifValue, int? contexteValue)
        => tarifValue is null || tarifValue == contexteValue;

    private static int CalculateSpecificity(TarifScolariteDto tarif)
        => 1
            + (tarif.CycleCode is null ? 0 : 1)
            + (tarif.NiveauNumero is null ? 0 : 1)
            + (tarif.FiliereCode is null ? 0 : 1)
            + (tarif.SpecialiteCode is null ? 0 : 1);

    private static int CalculateSpecificity(TarifScolarite tarif)
        => 1
            + (tarif.CycleCode is null ? 0 : 1)
            + (tarif.NiveauNumero is null ? 0 : 1)
            + (tarif.FiliereCode is null ? 0 : 1)
            + (tarif.SpecialiteCode is null ? 0 : 1);

    private static string FormatCodeLibelle(string code, string libelle)
        => string.IsNullOrWhiteSpace(code) ? libelle : $"{code} - {libelle}";

    private static string ResolveTypeCode(string? typeCode, string? typeLibelle)
    {
        if (!string.IsNullOrWhiteSpace(typeCode))
        {
            return typeCode;
        }

        var displayCode = typeLibelle?.Split('-', 2, StringSplitOptions.TrimEntries)[0];

        return string.IsNullOrWhiteSpace(displayCode) ? string.Empty : displayCode;
    }

    private static string BuildParcoursSegment(string? cycleCode, int? niveauNumero)
    {
        var cycleSegment = string.IsNullOrWhiteSpace(cycleCode)
            ? "GEN"
            : NormalizeSegment(cycleCode, "Le cycle du tarif est invalide.");

        cycleSegment = cycleSegment switch
        {
            "LICENCE" => "LI",
            "MASTER" => "MA",
            _ => cycleSegment
        };

        return niveauNumero is null ? cycleSegment : $"{cycleSegment}{niveauNumero}";
    }

    private static string BuildAnneeSegment(string? anneeAcademiqueCode)
    {
        var annee = RequireText(anneeAcademiqueCode, "L'année académique du tarif est obligatoire pour générer le code.");
        var matches = Regex.Matches(annee, @"\d{2,4}");

        if (matches.Count == 0)
        {
            return NormalizeSegment(annee, "L'année académique du tarif est invalide.");
        }

        var lastYear = matches[matches.Count - 1].Value;

        return lastYear.Length <= 2 ? lastYear.PadLeft(2, '0') : lastYear[^2..];
    }

    private static string NormalizeSegment(string? value, string errorMessage)
    {
        var normalized = RemoveDiacritics(RequireText(value, errorMessage)).ToUpperInvariant();
        var segment = new string(normalized.Where(char.IsLetterOrDigit).ToArray());

        if (string.IsNullOrWhiteSpace(segment))
        {
            throw new InvalidOperationException(errorMessage);
        }

        return segment;
    }

    private static string RemoveDiacritics(string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(capacity: normalized.Length);

        foreach (var character in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(character);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }

    private static string NormalizeCode(string? value, string errorMessage)
        => RequireText(value, errorMessage).ToUpperInvariant();

    private static string? NormalizeNullableCode(string? value)
    {
        var normalized = value?.Trim();

        return string.IsNullOrWhiteSpace(normalized) ? null : normalized.ToUpperInvariant();
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
