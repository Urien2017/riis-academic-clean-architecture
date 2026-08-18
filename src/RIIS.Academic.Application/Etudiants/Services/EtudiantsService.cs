using RIIS.Academic.Application.Abstractions.Persistence;
using RIIS.Academic.Application.Etudiants.Dtos;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Etudiants.Services;

public class EtudiantsService(IRepository<Etudiant> etudiants) : IEtudiantsService
{
    public async Task<List<EtudiantDto>> GetEtudiantsAsync(
        string? recherche = null,
        CancellationToken cancellationToken = default)
    {
        var items = await etudiants.ListAsync(cancellationToken);
        var normalizedSearch = recherche?.Trim();

        if (!string.IsNullOrWhiteSpace(normalizedSearch))
        {
            items = items
                .Where(x =>
                    Contains(x.Matricule, normalizedSearch)
                    || Contains(x.Nom, normalizedSearch)
                    || Contains(x.Prenoms, normalizedSearch)
                    || Contains(x.TelephonePrincipal, normalizedSearch)
                    || Contains(x.Email, normalizedSearch))
                .ToList();
        }

        return items
            .OrderBy(x => x.Nom)
            .ThenBy(x => x.Prenoms)
            .Select(ToDto)
            .ToList();
    }

    public async Task<EtudiantDto?> GetEtudiantAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await etudiants.GetByIdAsync(id, cancellationToken);

        return entity is null ? null : ToDto(entity);
    }

    public EtudiantDto CreateDefaultEtudiant()
        => new()
        {
            DateNaissance = new DateOnly(2000, 1, 1),
            LieuNaissance = "Non renseigné",
            Sexe = Sexe.NonRenseigne,
            AptitudeMedicale = AptitudeMedicale.NonRenseignee,
            Nationalite = "Camerounaise",
            TelephonePrincipal = "Non renseigné"
        };

    public async Task SaveEtudiantAsync(EtudiantDto dto, CancellationToken cancellationToken = default)
    {
        dto.Matricule = NormalizeNullable(dto.Matricule);
        dto.Nom = RequireText(dto.Nom, "Le nom de l'étudiant est obligatoire.").ToUpperInvariant();
        dto.Prenoms = RequireText(dto.Prenoms, "Les prénoms de l'étudiant sont obligatoires.");
        dto.LieuNaissance = RequireText(dto.LieuNaissance, "Le lieu de naissance est obligatoire.");
        dto.Nationalite = RequireText(dto.Nationalite, "La nationalité est obligatoire.");
        dto.TelephonePrincipal = RequireText(dto.TelephonePrincipal, "Le téléphone principal est obligatoire.");

        if (dto.Matricule is not null)
        {
            var existing = await etudiants.ListAsync(cancellationToken);
            var matriculeAlreadyUsed = existing.Any(x =>
                x.Id != dto.Id
                && string.Equals(x.Matricule, dto.Matricule, StringComparison.OrdinalIgnoreCase));

            if (matriculeAlreadyUsed)
            {
                throw new InvalidOperationException($"Le matricule '{dto.Matricule}' est déjà utilisé.");
            }
        }

        if (dto.Id == 0)
        {
            await etudiants.AddAsync(new Etudiant
            {
                Matricule = dto.Matricule,
                Nom = dto.Nom,
                Prenoms = dto.Prenoms,
                DateNaissance = dto.DateNaissance,
                LieuNaissance = dto.LieuNaissance,
                Sexe = dto.Sexe,
                AptitudeMedicale = dto.AptitudeMedicale,
                Nationalite = dto.Nationalite,
                RegionOrigine = NormalizeNullable(dto.RegionOrigine),
                TelephonePrincipal = dto.TelephonePrincipal,
                TelephoneSecondaire = NormalizeNullable(dto.TelephoneSecondaire),
                Email = NormalizeNullable(dto.Email),
                NomPere = NormalizeNullable(dto.NomPere),
                NomMere = NormalizeNullable(dto.NomMere),
                LieuResidence = NormalizeNullable(dto.LieuResidence),
                PhotoUrl = NormalizeNullable(dto.PhotoUrl)
            }, cancellationToken);
        }
        else
        {
            var entity = await etudiants.GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null) return;

            entity.Matricule = dto.Matricule;
            entity.Nom = dto.Nom;
            entity.Prenoms = dto.Prenoms;
            entity.DateNaissance = dto.DateNaissance;
            entity.LieuNaissance = dto.LieuNaissance;
            entity.Sexe = dto.Sexe;
            entity.AptitudeMedicale = dto.AptitudeMedicale;
            entity.Nationalite = dto.Nationalite;
            entity.RegionOrigine = NormalizeNullable(dto.RegionOrigine);
            entity.TelephonePrincipal = dto.TelephonePrincipal;
            entity.TelephoneSecondaire = NormalizeNullable(dto.TelephoneSecondaire);
            entity.Email = NormalizeNullable(dto.Email);
            entity.NomPere = NormalizeNullable(dto.NomPere);
            entity.NomMere = NormalizeNullable(dto.NomMere);
            entity.LieuResidence = NormalizeNullable(dto.LieuResidence);
            entity.PhotoUrl = NormalizeNullable(dto.PhotoUrl);
        }

        await etudiants.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteEtudiantAsync(long id, CancellationToken cancellationToken = default)
    {
        await etudiants.DeleteByIdAsync(id, cancellationToken);
        await etudiants.SaveChangesAsync(cancellationToken);
    }

    private static EtudiantDto ToDto(Etudiant entity)
        => new()
        {
            Id = entity.Id,
            Matricule = entity.Matricule,
            Nom = entity.Nom,
            Prenoms = entity.Prenoms,
            DateNaissance = entity.DateNaissance,
            LieuNaissance = entity.LieuNaissance,
            Sexe = entity.Sexe,
            AptitudeMedicale = entity.AptitudeMedicale,
            Nationalite = entity.Nationalite,
            RegionOrigine = entity.RegionOrigine,
            TelephonePrincipal = entity.TelephonePrincipal,
            TelephoneSecondaire = entity.TelephoneSecondaire,
            Email = entity.Email,
            NomPere = entity.NomPere,
            NomMere = entity.NomMere,
            LieuResidence = entity.LieuResidence,
            PhotoUrl = entity.PhotoUrl
        };

    private static bool Contains(string? source, string value)
        => source?.Contains(value, StringComparison.OrdinalIgnoreCase) == true;

    private static string? NormalizeNullable(string? value)
    {
        var normalized = value?.Trim();

        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
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
