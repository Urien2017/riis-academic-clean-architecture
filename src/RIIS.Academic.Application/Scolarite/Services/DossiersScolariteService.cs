using RIIS.Academic.Application.Abstractions.Persistence;
using RIIS.Academic.Application.Scolarite.Dtos;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Scolarite.Services;

public class DossiersScolariteService(
    IRepository<DossierScolarite> dossiersScolarite,
    IRepository<Inscription> inscriptions,
    IRepository<Etudiant> etudiants,
    IRepository<AnneeAcademique> anneesAcademiques,
    IRepository<ParcoursAcademique> parcoursAcademiques,
    IRepository<CycleFormation> cyclesFormation,
    IRepository<Filiere> filieres,
    IRepository<Specialite> specialites,
    IRepository<NiveauEtude> niveauxEtude,
    IRepository<TypeElementScolarite> typesElementsScolarite,
    IRepository<ElementScolariteEtudiant> elementsScolariteEtudiants,
    IRepository<DocumentElementScolarite> documentsElementsScolarite,
    IRepository<ValidationElementScolarite> validationsElementsScolarite,
    IRepository<PaiementScolarite> paiementsScolarite) : IDossiersScolariteService
{
    public async Task<List<DossierScolariteDto>> GetDossiersScolariteAsync(
        bool inclureInscriptionsSansDossier = true,
        string? recherche = null,
        string? anneeAcademiqueCode = null,
        string? cycleCode = null,
        int? niveauNumero = null,
        string? filiereCode = null,
        string? specialiteCode = null,
        CancellationToken cancellationToken = default)
    {
        var dossierItems = await dossiersScolarite.ListAsync(cancellationToken);
        var inscriptionItems = await inscriptions.ListAsync(cancellationToken);
        var context = await LoadContextAsync(cancellationToken);

        if (!inclureInscriptionsSansDossier)
        {
            inscriptionItems = inscriptionItems
                .Where(inscription => dossierItems.Any(dossier => dossier.InscriptionId == inscription.Id))
                .ToList();
        }

        var query = inscriptionItems
            .Select(inscription =>
            {
                var dossier = dossierItems.FirstOrDefault(x => x.InscriptionId == inscription.Id);
                return ToDto(inscription, dossier, context);
            });

        var normalizedSearch = NormalizeSearch(recherche);
        var normalizedAnnee = NormalizeNullableCode(anneeAcademiqueCode);
        var normalizedCycle = NormalizeNullableCode(cycleCode);
        var normalizedFiliere = NormalizeNullableCode(filiereCode);
        var normalizedSpecialite = NormalizeNullableCode(specialiteCode);

        if (normalizedSearch is not null)
        {
            query = query.Where(x =>
                NormalizeSearch(x.EtudiantNomComplet)?.Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase) == true
                || NormalizeSearch(x.EtudiantMatricule)?.Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase) == true);
        }

        if (normalizedAnnee is not null)
        {
            query = query.Where(x => SameCode(x.AnneeAcademiqueCode, normalizedAnnee));
        }

        if (normalizedCycle is not null)
        {
            query = query.Where(x => SameCode(x.CycleCode, normalizedCycle));
        }

        if (niveauNumero is not null)
        {
            query = query.Where(x => x.NiveauNumero == niveauNumero.Value);
        }

        if (normalizedFiliere is not null)
        {
            query = query.Where(x => SameCode(x.FiliereCode, normalizedFiliere));
        }

        if (normalizedSpecialite is not null)
        {
            query = query.Where(x => SameCode(x.SpecialiteCode, normalizedSpecialite));
        }

        return query
            .OrderByDescending(x => x.AnneeAcademiqueLibelle)
            .ThenBy(x => x.EtudiantNomComplet)
            .ToList();
    }

    public async Task<DossierScolariteDto?> GetDossierScolariteAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var dossier = await dossiersScolarite.GetByIdAsync(id, cancellationToken);
        if (dossier is null)
        {
            return null;
        }

        var inscription = await inscriptions.GetByIdAsync(dossier.InscriptionId, cancellationToken);
        if (inscription is null)
        {
            return null;
        }

        var context = await LoadContextAsync(cancellationToken);
        return ToDto(inscription, dossier, context);
    }

    public async Task<AdministrationDossierScolariteDto?> GetAdministrationDossierScolariteAsync(
        long dossierScolariteId,
        CancellationToken cancellationToken = default)
    {
        var dossier = await dossiersScolarite.GetByIdAsync(dossierScolariteId, cancellationToken);
        if (dossier is null)
        {
            return null;
        }

        return await BuildAdministrationDtoAsync(dossierScolariteId, cancellationToken);
    }

    public async Task<AdministrationDossierScolariteDto> SynchroniserDocumentsAdministratifsAsync(
        long dossierScolariteId,
        CancellationToken cancellationToken = default)
    {
        var dossier = await dossiersScolarite.GetByIdAsync(dossierScolariteId, cancellationToken)
            ?? throw new InvalidOperationException("Le dossier scolarite selectionne est introuvable.");

        var types = await GetTypesAdministratifsAsync(cancellationToken);
        var elements = await elementsScolariteEtudiants.ListAsync(cancellationToken);
        var dossierElements = elements.Where(x => x.DossierScolariteId == dossier.Id).ToList();

        foreach (var type in types)
        {
            if (dossierElements.Any(x => x.TypeElementScolariteId == type.Id))
            {
                continue;
            }

            await elementsScolariteEtudiants.AddAsync(new ElementScolariteEtudiant
            {
                DossierScolariteId = dossier.Id,
                TypeElementScolariteId = type.Id,
                Code = type.Code,
                Libelle = type.Libelle,
                MontantAttendu = 0,
                MontantAffecte = 0,
                MontantRestant = 0,
                Statut = type.EstObligatoire ? StatutElementScolarite.EnAttente : StatutElementScolarite.NonApplicable,
                DateCreationUtc = DateTime.UtcNow
            }, cancellationToken);
        }

        await elementsScolariteEtudiants.SaveChangesAsync(cancellationToken);
        return await BuildAdministrationDtoAsync(dossier.Id, cancellationToken);
    }

    public async Task<AdministrationDossierScolariteDto> SaveDocumentAdministratifAsync(
        DocumentAdministratifDossierDto dto,
        CancellationToken cancellationToken = default)
    {
        var element = await elementsScolariteEtudiants.GetByIdAsync(dto.ElementScolariteEtudiantId, cancellationToken)
            ?? throw new InvalidOperationException("L'element administratif selectionne est introuvable.");
        var type = await typesElementsScolarite.GetByIdAsync(element.TypeElementScolariteId, cancellationToken)
            ?? throw new InvalidOperationException("Le type d'element administratif est introuvable.");

        DocumentElementScolarite? document = null;
        if (type.EstDocumentaire)
        {
            document = dto.DocumentId is null
                ? null
                : await documentsElementsScolarite.GetByIdAsync(dto.DocumentId.Value, cancellationToken);

            if (document is null)
            {
                document = new DocumentElementScolarite
                {
                    ElementScolariteEtudiantId = element.Id,
                    NomDocument = NormalizeText(dto.NomDocument) ?? element.Libelle
                };

                await documentsElementsScolarite.AddAsync(document, cancellationToken);
            }

            document.NomDocument = NormalizeText(dto.NomDocument) ?? element.Libelle;
            document.UrlFichier = NormalizeText(dto.UrlFichier);
            document.Statut = dto.DocumentStatut;
            document.VerifiePar = NormalizeText(dto.VerifiePar);
            document.Observation = NormalizeText(dto.Observation);

            if (document.Statut is StatutDocumentScolarite.Depose or StatutDocumentScolarite.Valide && document.DateDepotUtc is null)
            {
                document.DateDepotUtc = DateTime.UtcNow;
            }

            if (document.Statut is StatutDocumentScolarite.Valide or StatutDocumentScolarite.Rejete && document.DateVerificationUtc is null)
            {
                document.DateVerificationUtc = DateTime.UtcNow;
            }
        }

        ValidationElementScolarite? validation = null;
        if (type.EstSoumisValidation)
        {
            validation = dto.ValidationId is null
                ? null
                : await validationsElementsScolarite.GetByIdAsync(dto.ValidationId.Value, cancellationToken);

            if (validation is null)
            {
                validation = new ValidationElementScolarite
                {
                    ElementScolariteEtudiantId = element.Id
                };

                await validationsElementsScolarite.AddAsync(validation, cancellationToken);
            }

            validation.Statut = dto.ValidationStatut;
            validation.ValidePar = NormalizeText(dto.ValidePar);
            validation.MotifRejet = NormalizeText(dto.MotifRejet);
            validation.Observation = NormalizeText(dto.Observation);

            if (validation.Statut is StatutValidationScolarite.Validee or StatutValidationScolarite.Rejetee && validation.DateValidationUtc is null)
            {
                validation.DateValidationUtc = DateTime.UtcNow;
            }
        }

        element.Statut = ResolveElementStatut(type, document, validation);
        element.DateDernierRecalculUtc = DateTime.UtcNow;
        element.Observation = NormalizeText(dto.Observation);

        if (document is not null)
        {
            await documentsElementsScolarite.SaveChangesAsync(cancellationToken);
        }

        if (validation is not null)
        {
            await validationsElementsScolarite.SaveChangesAsync(cancellationToken);
        }

        await elementsScolariteEtudiants.SaveChangesAsync(cancellationToken);
        return await RecalculerValidationAdministrativeAsync(element.DossierScolariteId, cancellationToken);
    }

    public async Task<AdministrationDossierScolariteDto> RecalculerValidationAdministrativeAsync(
        long dossierScolariteId,
        CancellationToken cancellationToken = default)
    {
        var dossier = await dossiersScolarite.GetByIdAsync(dossierScolariteId, cancellationToken)
            ?? throw new InvalidOperationException("Le dossier scolarite selectionne est introuvable.");
        var administration = await BuildAdministrationDtoAsync(dossier.Id, cancellationToken);

        dossier.StatutAdministratif = administration.PiecesObligatoiresCompletes
            ? StatutDossierScolarite.Regulier
            : StatutDossierScolarite.Incomplet;
        dossier.StatutFinancier = administration.PaiementInitialEffectue
            ? StatutDossierScolarite.EnCours
            : StatutDossierScolarite.EnPreparation;
        dossier.StatutGlobal = administration.SuiteParcoursAutorisee
            ? StatutDossierScolarite.EnCours
            : StatutDossierScolarite.Incomplet;
        dossier.DateDernierRecalculUtc = DateTime.UtcNow;

        await dossiersScolarite.SaveChangesAsync(cancellationToken);
        return await BuildAdministrationDtoAsync(dossier.Id, cancellationToken);
    }

    public async Task<DossierScolariteDto> GetOrCreateDossierDepuisInscriptionAsync(
        long inscriptionId,
        CancellationToken cancellationToken = default)
    {
        var inscription = await inscriptions.GetByIdAsync(inscriptionId, cancellationToken)
            ?? throw new InvalidOperationException("L'inscription sélectionnée est introuvable.");

        var dossierItems = await dossiersScolarite.ListAsync(cancellationToken);
        var existing = dossierItems.FirstOrDefault(x => x.InscriptionId == inscriptionId);
        var context = await LoadContextAsync(cancellationToken);

        if (existing is not null)
        {
            return ToDto(inscription, existing, context);
        }

        var snapshot = BuildSnapshot(inscription, context);
        var dossier = new DossierScolarite
        {
            InscriptionId = inscription.Id,
            AnneeAcademiqueCode = snapshot.AnneeAcademiqueCode,
            AnneeAcademiqueLibelle = snapshot.AnneeAcademiqueLibelle,
            CycleCode = snapshot.CycleCode,
            CycleLibelle = snapshot.CycleLibelle,
            FiliereCode = snapshot.FiliereCode,
            FiliereLibelle = snapshot.FiliereLibelle,
            SpecialiteCode = snapshot.SpecialiteCode,
            SpecialiteLibelle = snapshot.SpecialiteLibelle,
            NiveauNumero = snapshot.NiveauNumero,
            NiveauLibelle = snapshot.NiveauLibelle,
            StatutAdministratif = StatutDossierScolarite.EnPreparation,
            StatutFinancier = StatutDossierScolarite.EnPreparation,
            StatutGlobal = StatutDossierScolarite.EnPreparation,
            DateCreationUtc = DateTime.UtcNow
        };

        await dossiersScolarite.AddAsync(dossier, cancellationToken);
        await dossiersScolarite.SaveChangesAsync(cancellationToken);

        return ToDto(inscription, dossier, context);
    }

    private async Task<DossierScolariteContext> LoadContextAsync(CancellationToken cancellationToken)
        => new(
            await etudiants.ListAsync(cancellationToken),
            await anneesAcademiques.ListAsync(cancellationToken),
            await parcoursAcademiques.ListAsync(cancellationToken),
            await cyclesFormation.ListAsync(cancellationToken),
            await filieres.ListAsync(cancellationToken),
            await specialites.ListAsync(cancellationToken),
            await niveauxEtude.ListAsync(cancellationToken));

    private async Task<List<TypeElementScolarite>> GetTypesAdministratifsAsync(CancellationToken cancellationToken)
    {
        var types = await typesElementsScolarite.ListAsync(cancellationToken);

        return types
            .Where(x => x.EstActif)
            .Where(x =>
                x.EstDocumentaire
                || x.EstSoumisValidation
                || x.Categorie is CategorieTypeElementScolarite.Document or CategorieTypeElementScolarite.Validation)
            .OrderBy(x => x.OrdreAffichage)
            .ThenBy(x => x.Libelle)
            .ToList();
    }

    private async Task<AdministrationDossierScolariteDto> BuildAdministrationDtoAsync(
        long dossierScolariteId,
        CancellationToken cancellationToken)
    {
        var types = await typesElementsScolarite.ListAsync(cancellationToken);
        var elements = (await elementsScolariteEtudiants.ListAsync(cancellationToken))
            .Where(x => x.DossierScolariteId == dossierScolariteId)
            .ToList();
        var documents = await documentsElementsScolarite.ListAsync(cancellationToken);
        var validations = await validationsElementsScolarite.ListAsync(cancellationToken);
        var paiements = await paiementsScolarite.ListAsync(cancellationToken);

        var documentItems = elements
            .Select(element => new
            {
                Element = element,
                Type = types.FirstOrDefault(x => x.Id == element.TypeElementScolariteId)
            })
            .Where(x => x.Type is not null)
            .Where(x =>
                x.Type!.EstDocumentaire
                || x.Type.EstSoumisValidation
                || x.Type.Categorie is CategorieTypeElementScolarite.Document or CategorieTypeElementScolarite.Validation)
            .Select(x => ToDocumentDto(
                x.Element,
                x.Type!,
                documents.Where(document => document.ElementScolariteEtudiantId == x.Element.Id),
                validations.Where(validation => validation.ElementScolariteEtudiantId == x.Element.Id)))
            .OrderBy(x => x.Libelle)
            .ToList();

        var obligatoryItems = documentItems.Where(x => x.EstObligatoire).ToList();
        var activePayments = paiements
            .Where(x => x.DossierScolariteId == dossierScolariteId)
            .Where(x => x.Statut != StatutPaiementScolarite.Annule)
            .Where(x => x.Montant > 0)
            .ToList();
        var piecesCompletes = obligatoryItems.All(x => x.EstComplet);
        var paiementInitial = activePayments.Any();

        return new AdministrationDossierScolariteDto
        {
            DossierScolariteId = dossierScolariteId,
            PiecesObligatoiresCompletes = piecesCompletes,
            PaiementInitialEffectue = paiementInitial,
            MontantPaiementsActifs = activePayments.Sum(x => x.Montant),
            NombrePiecesObligatoires = obligatoryItems.Count,
            NombrePiecesObligatoiresCompletes = obligatoryItems.Count(x => x.EstComplet),
            MotifAutorisation = ResolveMotifAutorisation(piecesCompletes, paiementInitial),
            Documents = documentItems
        };
    }

    private static DossierScolariteDto ToDto(
        Inscription inscription,
        DossierScolarite? dossier,
        DossierScolariteContext context)
    {
        var etudiant = context.Etudiants.FirstOrDefault(x => x.Id == inscription.EtudiantId);
        var snapshot = dossier is null
            ? BuildSnapshot(inscription, context)
            : new DossierScolariteSnapshot(
                dossier.AnneeAcademiqueCode,
                dossier.AnneeAcademiqueLibelle,
                dossier.CycleCode,
                dossier.CycleLibelle,
                dossier.FiliereCode,
                dossier.FiliereLibelle,
                dossier.SpecialiteCode,
                dossier.SpecialiteLibelle,
                dossier.NiveauNumero,
                dossier.NiveauLibelle);

        return new DossierScolariteDto
        {
            Id = dossier?.Id ?? 0,
            InscriptionId = inscription.Id,
            EtudiantMatricule = etudiant?.Matricule ?? string.Empty,
            EtudiantNomComplet = etudiant is null ? string.Empty : $"{etudiant.Nom} {etudiant.Prenoms}",
            DateInscription = inscription.DateInscription,
            AnneeAcademiqueCode = snapshot.AnneeAcademiqueCode,
            AnneeAcademiqueLibelle = snapshot.AnneeAcademiqueLibelle,
            CycleCode = snapshot.CycleCode,
            CycleLibelle = snapshot.CycleLibelle,
            FiliereCode = snapshot.FiliereCode,
            FiliereLibelle = snapshot.FiliereLibelle,
            SpecialiteCode = snapshot.SpecialiteCode,
            SpecialiteLibelle = snapshot.SpecialiteLibelle,
            NiveauNumero = snapshot.NiveauNumero,
            NiveauLibelle = snapshot.NiveauLibelle,
            StatutAdministratif = dossier?.StatutAdministratif ?? StatutDossierScolarite.EnPreparation,
            StatutFinancier = dossier?.StatutFinancier ?? StatutDossierScolarite.EnPreparation,
            StatutGlobal = dossier?.StatutGlobal ?? StatutDossierScolarite.EnPreparation,
            DateCreationUtc = dossier?.DateCreationUtc,
            DateDernierRecalculUtc = dossier?.DateDernierRecalculUtc,
            Observation = dossier?.Observation
        };
    }

    private static DossierScolariteSnapshot BuildSnapshot(
        Inscription inscription,
        DossierScolariteContext context)
    {
        var annee = context.AnneesAcademiques.FirstOrDefault(x => x.Id == inscription.AnneeAcademiqueId)
            ?? throw new InvalidOperationException("L'année académique de l'inscription est introuvable.");
        var parcours = context.Parcours.FirstOrDefault(x => x.Id == inscription.ParcoursAcademiqueId)
            ?? throw new InvalidOperationException("Le parcours de l'inscription est introuvable.");
        var cycle = context.Cycles.FirstOrDefault(x => x.Id == parcours.CycleFormationId);
        var filiere = context.Filieres.FirstOrDefault(x => x.Id == parcours.FiliereId);
        var specialite = context.Specialites.FirstOrDefault(x => x.Id == parcours.SpecialiteId)
            ?? throw new InvalidOperationException(
                $"Le parcours '{parcours.Code}' n'est rattaché à aucune spécialité valide.");
        var niveau = context.Niveaux.FirstOrDefault(x => x.Id == inscription.NiveauEtudeId)
            ?? throw new InvalidOperationException("Le niveau de l'inscription est introuvable.");

        return new DossierScolariteSnapshot(
            annee.Libelle,
            annee.Libelle,
            cycle?.Code ?? string.Empty,
            cycle?.Libelle ?? string.Empty,
            filiere?.Code ?? string.Empty,
            filiere?.Libelle ?? string.Empty,
            specialite.Code,
            specialite.Libelle,
            niveau.Numero,
            niveau.Libelle);
    }

    private static string? NormalizeNullableCode(string? value)
    {
        var normalized = value?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized.ToUpperInvariant();
    }

    private static string? NormalizeSearch(string? value)
    {
        var normalized = value?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }

    private static DocumentAdministratifDossierDto ToDocumentDto(
        ElementScolariteEtudiant element,
        TypeElementScolarite type,
        IEnumerable<DocumentElementScolarite> documents,
        IEnumerable<ValidationElementScolarite> validations)
    {
        var document = documents
            .OrderByDescending(x => x.DateDepotUtc ?? x.DateVerificationUtc ?? DateTime.MinValue)
            .ThenByDescending(x => x.Id)
            .FirstOrDefault();
        var validation = validations
            .OrderByDescending(x => x.DateValidationUtc ?? DateTime.MinValue)
            .ThenByDescending(x => x.Id)
            .FirstOrDefault();
        var isComplete = IsAdministrativeItemComplete(type, document, validation);

        return new DocumentAdministratifDossierDto
        {
            ElementScolariteEtudiantId = element.Id,
            TypeElementScolariteId = type.Id,
            Code = element.Code,
            Libelle = element.Libelle,
            EstObligatoire = type.EstObligatoire,
            EstDocumentaire = type.EstDocumentaire,
            EstSoumisValidation = type.EstSoumisValidation,
            ElementStatut = element.Statut,
            DocumentId = document?.Id,
            NomDocument = document?.NomDocument ?? element.Libelle,
            UrlFichier = document?.UrlFichier,
            DocumentStatut = document?.Statut ?? StatutDocumentScolarite.EnAttente,
            DateDepotUtc = document?.DateDepotUtc,
            DateVerificationUtc = document?.DateVerificationUtc,
            VerifiePar = document?.VerifiePar,
            ValidationId = validation?.Id,
            ValidationStatut = validation?.Statut ?? StatutValidationScolarite.EnAttente,
            DateValidationUtc = validation?.DateValidationUtc,
            ValidePar = validation?.ValidePar,
            MotifRejet = validation?.MotifRejet,
            Observation = validation?.Observation ?? document?.Observation ?? element.Observation,
            EstComplet = isComplete
        };
    }

    private static StatutElementScolarite ResolveElementStatut(
        TypeElementScolarite type,
        DocumentElementScolarite? document,
        ValidationElementScolarite? validation)
    {
        if (document?.Statut == StatutDocumentScolarite.Rejete || validation?.Statut == StatutValidationScolarite.Rejetee)
        {
            return StatutElementScolarite.Bloque;
        }

        if (IsAdministrativeItemComplete(type, document, validation))
        {
            return StatutElementScolarite.Valide;
        }

        if (document?.Statut is StatutDocumentScolarite.Depose or StatutDocumentScolarite.Valide
            || validation?.Statut == StatutValidationScolarite.Validee)
        {
            return StatutElementScolarite.Partiel;
        }

        return type.EstObligatoire ? StatutElementScolarite.EnAttente : StatutElementScolarite.NonApplicable;
    }

    private static bool IsAdministrativeItemComplete(
        TypeElementScolarite type,
        DocumentElementScolarite? document,
        ValidationElementScolarite? validation)
    {
        var documentComplete = !type.EstDocumentaire
            || document?.Statut is StatutDocumentScolarite.Depose or StatutDocumentScolarite.Valide;
        var validationComplete = !type.EstSoumisValidation
            || validation?.Statut == StatutValidationScolarite.Validee;

        if (type.EstDocumentaire && type.EstSoumisValidation)
        {
            documentComplete = document?.Statut == StatutDocumentScolarite.Valide;
        }

        return documentComplete && validationComplete;
    }

    private static string ResolveMotifAutorisation(bool piecesCompletes, bool paiementInitial)
    {
        if (piecesCompletes)
        {
            return "Suite autorisee : pieces obligatoires completes.";
        }

        if (paiementInitial)
        {
            return "Suite autorisee vers la Gestion financiere sous reserve des documents et pieces justificatives.";
        }

        return "Pieces obligatoires incompletes et aucun paiement initial enregistre.";
    }

    private static string? NormalizeText(string? value)
    {
        var normalized = value?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }

    private static bool SameCode(string? left, string? right)
        => string.Equals(left, right, StringComparison.OrdinalIgnoreCase);

    private sealed record DossierScolariteContext(
        IReadOnlyCollection<Etudiant> Etudiants,
        IReadOnlyCollection<AnneeAcademique> AnneesAcademiques,
        IReadOnlyCollection<ParcoursAcademique> Parcours,
        IReadOnlyCollection<CycleFormation> Cycles,
        IReadOnlyCollection<Filiere> Filieres,
        IReadOnlyCollection<Specialite> Specialites,
        IReadOnlyCollection<NiveauEtude> Niveaux);

    private sealed record DossierScolariteSnapshot(
        string AnneeAcademiqueCode,
        string AnneeAcademiqueLibelle,
        string CycleCode,
        string CycleLibelle,
        string FiliereCode,
        string FiliereLibelle,
        string? SpecialiteCode,
        string? SpecialiteLibelle,
        int NiveauNumero,
        string NiveauLibelle);
}
