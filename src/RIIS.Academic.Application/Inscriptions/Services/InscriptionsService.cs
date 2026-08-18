using RIIS.Academic.Application.Abstractions.Persistence;
using RIIS.Academic.Application.Common.Dtos;
using RIIS.Academic.Application.Inscriptions.Dtos;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Inscriptions.Services;

public class InscriptionsService(
    IRepository<Inscription> inscriptions,
    IRepository<AnneeAcademique> anneesAcademiques,
    IRepository<Etudiant> etudiants,
    IRepository<CycleFormation> cyclesFormation,
    IRepository<ParcoursAcademique> parcoursAcademiques,
    IRepository<NiveauEtude> niveauxEtude,
    IRepository<ClassePedagogique> classesPedagogiques,
    IRepository<MaquettePedagogique> maquettesPedagogiques) : IInscriptionsService
{
    public async Task<List<InscriptionDto>> GetInscriptionsAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? niveauEtudeId = null,
        long? classePedagogiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var inscriptionItems = await inscriptions.ListAsync(cancellationToken);
        var ouvertures = await parcoursAcademiques.ListAsync(cancellationToken);

        if (anneeAcademiqueId is not null)
        {
            inscriptionItems = inscriptionItems
                .Where(x => x.AnneeAcademiqueId == anneeAcademiqueId)
                .ToList();
        }

        if (cycleFormationId is not null)
        {
            var ouvertureIds = ouvertures
                .Where(x => x.CycleFormationId == cycleFormationId)
                .Select(x => x.Id)
                .ToHashSet();

            inscriptionItems = inscriptionItems
                .Where(x => ouvertureIds.Contains(x.ParcoursAcademiqueId))
                .ToList();
        }

        if (niveauEtudeId is not null)
        {
            inscriptionItems = inscriptionItems
                .Where(x => x.NiveauEtudeId == niveauEtudeId)
                .ToList();
        }

        if (classePedagogiqueId is not null)
        {
            inscriptionItems = inscriptionItems
                .Where(x => x.ClassePedagogiqueId == classePedagogiqueId)
                .ToList();
        }

        var annees = await anneesAcademiques.ListAsync(cancellationToken);
        var etudiantItems = await etudiants.ListAsync(cancellationToken);
        var niveaux = await niveauxEtude.ListAsync(cancellationToken);
        var classes = await classesPedagogiques.ListAsync(cancellationToken);
        var maquettes = await maquettesPedagogiques.ListAsync(cancellationToken);

        return inscriptionItems
            .Select(inscription => ToDto(inscription, annees, etudiantItems, ouvertures, niveaux, classes, maquettes))
            .OrderByDescending(x => x.AnneeAcademiqueLibelle)
            .ThenBy(x => x.ClassePedagogiqueLibelle)
            .ThenBy(x => x.EtudiantNomComplet)
            .ToList();
    }

    public async Task<InscriptionDto?> GetInscriptionAsync(long id, CancellationToken cancellationToken = default)
    {
        var inscriptionItems = await inscriptions.ListAsync(cancellationToken);
        var inscription = inscriptionItems.FirstOrDefault(x => x.Id == id);

        if (inscription is null)
        {
            return null;
        }

        var annees = await anneesAcademiques.ListAsync(cancellationToken);
        var etudiantItems = await etudiants.ListAsync(cancellationToken);
        var ouvertures = await parcoursAcademiques.ListAsync(cancellationToken);
        var niveaux = await niveauxEtude.ListAsync(cancellationToken);
        var classes = await classesPedagogiques.ListAsync(cancellationToken);
        var maquettes = await maquettesPedagogiques.ListAsync(cancellationToken);

        return ToDto(inscription, annees, etudiantItems, ouvertures, niveaux, classes, maquettes);
    }

    public async Task<InscriptionDto> CreateDefaultInscriptionAsync(CancellationToken cancellationToken = default)
    {
        var annee = await GetDefaultAnneeAcademiqueAsync(cancellationToken);

        return new InscriptionDto
        {
            AnneeAcademiqueId = annee?.Id ?? 0,
            AnneeAcademiqueLibelle = annee?.Libelle ?? string.Empty,
            DateInscription = DateOnly.FromDateTime(DateTime.Today),
            Statut = StatutInscription.EnAttente
        };
    }

    public async Task SaveInscriptionAsync(InscriptionDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.AnneeAcademiqueId <= 0)
        {
            throw new InvalidOperationException("L'année académique de l'inscription est obligatoire.");
        }

        if (dto.EtudiantId <= 0)
        {
            throw new InvalidOperationException("L'étudiant de l'inscription est obligatoire.");
        }

        if (dto.ParcoursAcademiqueId <= 0)
        {
            throw new InvalidOperationException("Le parcours cycle/filière/spécialité est obligatoire.");
        }

        if (dto.NiveauEtudeId <= 0)
        {
            throw new InvalidOperationException("Le niveau d'étude est obligatoire.");
        }

        if (dto.Statut == StatutInscription.Validee && dto.ClassePedagogiqueId is null)
        {
            throw new InvalidOperationException("Une inscription validée doit être affectée à une classe pédagogique.");
        }

        dto.MentionSpeciale = NormalizeNullable(dto.MentionSpeciale);
        dto.TutelleAcademique = NormalizeNullable(dto.TutelleAcademique);
        dto.Observation = NormalizeNullable(dto.Observation);
        dto.CodeAdministration = NormalizeNullable(dto.CodeAdministration);

        await EnsureReferencesAndCoherenceAsync(dto, cancellationToken);

        var inscriptionItems = await inscriptions.ListAsync(cancellationToken);
        var alreadyRegistered = inscriptionItems.Any(x =>
            x.Id != dto.Id
            && x.AnneeAcademiqueId == dto.AnneeAcademiqueId
            && x.EtudiantId == dto.EtudiantId);

        if (alreadyRegistered)
        {
            throw new InvalidOperationException("Cet étudiant possède déjà une inscription pour cette année académique.");
        }

        if (dto.Id == 0)
        {
            await inscriptions.AddAsync(new Inscription
            {
                AnneeAcademiqueId = dto.AnneeAcademiqueId,
                EtudiantId = dto.EtudiantId,
                ParcoursAcademiqueId = dto.ParcoursAcademiqueId,
                NiveauEtudeId = dto.NiveauEtudeId,
                MaquettePedagogiqueId = dto.MaquettePedagogiqueId,
                ClassePedagogiqueId = dto.ClassePedagogiqueId,
                DateInscription = dto.DateInscription,
                Statut = dto.Statut,
                MentionSpeciale = dto.MentionSpeciale,
                TutelleAcademique = dto.TutelleAcademique,
                Observation = dto.Observation,
                CodeAdministration = dto.CodeAdministration
            }, cancellationToken);
        }
        else
        {
            var entity = await inscriptions.GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null) return;

            entity.AnneeAcademiqueId = dto.AnneeAcademiqueId;
            entity.EtudiantId = dto.EtudiantId;
            entity.ParcoursAcademiqueId = dto.ParcoursAcademiqueId;
            entity.NiveauEtudeId = dto.NiveauEtudeId;
            entity.MaquettePedagogiqueId = dto.MaquettePedagogiqueId;
            entity.ClassePedagogiqueId = dto.ClassePedagogiqueId;
            entity.DateInscription = dto.DateInscription;
            entity.Statut = dto.Statut;
            entity.MentionSpeciale = dto.MentionSpeciale;
            entity.TutelleAcademique = dto.TutelleAcademique;
            entity.Observation = dto.Observation;
            entity.CodeAdministration = dto.CodeAdministration;
        }

        await inscriptions.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteInscriptionAsync(long id, CancellationToken cancellationToken = default)
    {
        await inscriptions.DeleteByIdAsync(id, cancellationToken);
        await inscriptions.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<LookupDto>> GetAnneesAcademiquesLookupAsync(CancellationToken cancellationToken = default)
    {
        var items = await anneesAcademiques.ListAsync(cancellationToken);

        return items
            .OrderByDescending(x => x.AnneeDebut)
            .Select(x => new LookupDto { Id = x.Id, Libelle = x.Libelle })
            .ToList();
    }

    public async Task<List<LookupDto>> GetEtudiantsLookupAsync(CancellationToken cancellationToken = default)
    {
        var items = await etudiants.ListAsync(cancellationToken);

        return items
            .OrderBy(x => x.Nom)
            .ThenBy(x => x.Prenoms)
            .Select(x => new LookupDto
            {
                Id = x.Id,
                Libelle = string.IsNullOrWhiteSpace(x.Matricule)
                    ? $"{x.Nom} {x.Prenoms}"
                    : $"{x.Matricule} - {x.Nom} {x.Prenoms}"
            })
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

    public async Task<List<LookupDto>> GetClassesPedagogiquesLookupAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? niveauEtudeId = null,
        CancellationToken cancellationToken = default)
    {
        var items = await classesPedagogiques.ListAsync(cancellationToken);
        var ouvertures = await parcoursAcademiques.ListAsync(cancellationToken);

        if (anneeAcademiqueId is not null)
        {
            items = items
                .Where(x => x.AnneeAcademiqueId == anneeAcademiqueId)
                .ToList();
        }

        if (cycleFormationId is not null)
        {
            var ouvertureIds = ouvertures
                .Where(x => x.CycleFormationId == cycleFormationId)
                .Select(x => x.Id)
                .ToHashSet();

            items = items
                .Where(x => ouvertureIds.Contains(x.ParcoursAcademiqueId))
                .ToList();
        }

        if (niveauEtudeId is not null)
        {
            items = items
                .Where(x => x.NiveauEtudeId == niveauEtudeId)
                .ToList();
        }

        return items
            .Where(x => x.EstActive)
            .OrderBy(x => x.Code)
            .Select(x => new LookupDto { Id = x.Id, Libelle = FormatCodeLibelle(x.Code, x.Libelle) })
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

    private async Task EnsureReferencesAndCoherenceAsync(InscriptionDto dto, CancellationToken cancellationToken)
    {
        var anneeExists = (await anneesAcademiques.ListAsync(cancellationToken)).Any(x => x.Id == dto.AnneeAcademiqueId);
        if (!anneeExists)
        {
            throw new InvalidOperationException("L'année académique sélectionnée est introuvable.");
        }

        var etudiantExists = (await etudiants.ListAsync(cancellationToken)).Any(x => x.Id == dto.EtudiantId);
        if (!etudiantExists)
        {
            throw new InvalidOperationException("L'étudiant sélectionné est introuvable.");
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

        if (dto.ClassePedagogiqueId is not null)
        {
            var classe = (await classesPedagogiques.ListAsync(cancellationToken))
                .FirstOrDefault(x => x.Id == dto.ClassePedagogiqueId);

            if (classe is null)
            {
                throw new InvalidOperationException("La classe pédagogique sélectionnée est introuvable.");
            }

            if (classe.AnneeAcademiqueId != dto.AnneeAcademiqueId)
            {
                throw new InvalidOperationException("La classe sélectionnée n'appartient pas à l'année académique de l'inscription.");
            }

            if (classe.ParcoursAcademiqueId != dto.ParcoursAcademiqueId)
            {
                throw new InvalidOperationException("La classe sélectionnée n'appartient pas au parcours de l'inscription.");
            }

            if (classe.NiveauEtudeId != dto.NiveauEtudeId)
            {
                throw new InvalidOperationException("La classe sélectionnée n'appartient pas au niveau de l'inscription.");
            }

            dto.MaquettePedagogiqueId ??= classe.MaquettePedagogiqueId;
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
                throw new InvalidOperationException("La maquette sélectionnée n'appartient pas au parcours de l'inscription.");
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

    private static InscriptionDto ToDto(
        Inscription inscription,
        IReadOnlyCollection<AnneeAcademique> annees,
        IReadOnlyCollection<Etudiant> etudiants,
        IReadOnlyCollection<ParcoursAcademique> ouvertures,
        IReadOnlyCollection<NiveauEtude> niveaux,
        IReadOnlyCollection<ClassePedagogique> classes,
        IReadOnlyCollection<MaquettePedagogique> maquettes)
    {
        var annee = annees.FirstOrDefault(x => x.Id == inscription.AnneeAcademiqueId);
        var etudiant = etudiants.FirstOrDefault(x => x.Id == inscription.EtudiantId);
        var ouverture = ouvertures.FirstOrDefault(x => x.Id == inscription.ParcoursAcademiqueId);
        var niveau = niveaux.FirstOrDefault(x => x.Id == inscription.NiveauEtudeId);
        var classe = inscription.ClassePedagogiqueId is null
            ? null
            : classes.FirstOrDefault(x => x.Id == inscription.ClassePedagogiqueId);
        var maquette = inscription.MaquettePedagogiqueId is null
            ? null
            : maquettes.FirstOrDefault(x => x.Id == inscription.MaquettePedagogiqueId);

        return new InscriptionDto
        {
            Id = inscription.Id,
            AnneeAcademiqueId = inscription.AnneeAcademiqueId,
            AnneeAcademiqueLibelle = annee?.Libelle ?? string.Empty,
            EtudiantId = inscription.EtudiantId,
            EtudiantMatricule = etudiant?.Matricule ?? string.Empty,
            EtudiantNomComplet = etudiant is null ? string.Empty : $"{etudiant.Nom} {etudiant.Prenoms}",
            ParcoursAcademiqueId = inscription.ParcoursAcademiqueId,
            ParcoursAcademiqueLibelle = ouverture is null
                ? string.Empty
                : FormatCodeLibelle(ouverture.Code, ouverture.Libelle),
            NiveauEtudeId = inscription.NiveauEtudeId,
            NiveauEtudeLibelle = niveau?.Libelle ?? string.Empty,
            MaquettePedagogiqueId = inscription.MaquettePedagogiqueId,
            MaquettePedagogiqueLibelle = maquette is null ? null : $"{maquette.Libelle} - {maquette.Version}",
            ClassePedagogiqueId = inscription.ClassePedagogiqueId,
            ClassePedagogiqueLibelle = classe is null ? null : FormatCodeLibelle(classe.Code, classe.Libelle),
            DateInscription = inscription.DateInscription,
            Statut = inscription.Statut,
            MentionSpeciale = inscription.MentionSpeciale,
            TutelleAcademique = inscription.TutelleAcademique,
            Observation = inscription.Observation,
            CodeAdministration = inscription.CodeAdministration
        };
    }

    private static string FormatCodeLibelle(string code, string libelle)
        => string.IsNullOrWhiteSpace(code) ? libelle : $"{code} - {libelle}";

    private static bool IsMaquetteCompatibleWithParcours(MaquettePedagogique maquette, ParcoursAcademique parcours)
        => maquette.CycleFormationId == parcours.CycleFormationId
            && maquette.NiveauEtudeId == parcours.NiveauEtudeId
            && maquette.FiliereId == parcours.FiliereId
            && maquette.SpecialiteId == parcours.SpecialiteId;

    private static string? NormalizeNullable(string? value)
    {
        var normalized = value?.Trim();

        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }
}
