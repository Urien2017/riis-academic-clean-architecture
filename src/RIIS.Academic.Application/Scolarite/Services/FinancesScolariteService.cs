using RIIS.Academic.Application.Abstractions.Persistence;
using RIIS.Academic.Application.Scolarite.Dtos;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Scolarite.Services;

public class FinancesScolariteService(
    IRepository<DossierScolarite> dossiersScolarite,
    IRepository<TypeElementScolarite> typesElementsScolarite,
    IRepository<ElementScolariteEtudiant> elementsScolariteEtudiants,
    IRepository<EcheanceScolarite> echeancesScolarite,
    IRepository<PaiementScolarite> paiementsScolarite,
    IRepository<ModePaiementScolarite> modesPaiementScolarite,
    IRepository<AffectationPaiementEcheance> affectationsPaiementsEcheances,
    IRepository<Inscription> inscriptions,
    ITarifsScolariteService tarifsScolariteService) : IFinancesScolariteService
{
    public async Task<FinanceDossierScolariteDto?> GetFinanceDossierScolariteAsync(
        long dossierScolariteId,
        CancellationToken cancellationToken = default)
    {
        var dossier = await dossiersScolarite.GetByIdAsync(dossierScolariteId, cancellationToken);

        return dossier is null
            ? null
            : await BuildFinanceDtoAsync(dossierScolariteId, cancellationToken);
    }

    public async Task<List<PaiementTypeElementTarifOptionDto>> GetOptionsTypesElementsTarifsPaiementAsync(
        long dossierScolariteId,
        CancellationToken cancellationToken = default)
    {
        var dossier = await dossiersScolarite.GetByIdAsync(dossierScolariteId, cancellationToken)
            ?? throw new InvalidOperationException("Le dossier scolarite selectionne est introuvable.");
        var types = await typesElementsScolarite.ListAsync(cancellationToken);
        var options = new List<PaiementTypeElementTarifOptionDto>();

        foreach (var type in types.Where(x => x.EstActif && x.EstPayable).OrderBy(x => x.OrdreAffichage).ThenBy(x => x.Libelle))
        {
            var tarif = await ResolveTarifForDossierAsync(dossier, type.Id, cancellationToken);
            if (tarif is null)
            {
                continue;
            }

            options.Add(new PaiementTypeElementTarifOptionDto
            {
                TypeElementScolariteId = type.Id,
                TypeElementScolariteLibelle = FormatCodeLibelle(type.Code, type.Libelle),
                TarifScolariteId = tarif.Id,
                TarifMontant = tarif.Montant,
                Devise = tarif.Devise
            });
        }

        return options;
    }

    public async Task<FinanceDossierScolariteDto> SavePaiementLibreAsync(
        PaiementLibreDossierDto dto,
        CancellationToken cancellationToken = default)
    {
        var dossier = await dossiersScolarite.GetByIdAsync(dto.DossierScolariteId, cancellationToken)
            ?? throw new InvalidOperationException("Le dossier scolarite selectionne est introuvable.");

        if (dto.Montant <= 0)
        {
            throw new InvalidOperationException("Le montant du paiement doit etre superieur a zero.");
        }

        if (dto.TypeElementScolariteId <= 0)
        {
            throw new InvalidOperationException("Le type d'element concerne par le paiement est obligatoire.");
        }

        if (dto.TarifScolariteId <= 0)
        {
            throw new InvalidOperationException("Le tarif concerne par le paiement est obligatoire.");
        }

        var mode = await modesPaiementScolarite.GetByIdAsync(dto.ModePaiementScolariteId, cancellationToken)
            ?? throw new InvalidOperationException("Le mode de paiement selectionne est introuvable.");

        if (!mode.EstActif)
        {
            throw new InvalidOperationException("Le mode de paiement selectionne est inactif.");
        }

        var tarif = await ResolveTarifForDossierAsync(dossier, dto.TypeElementScolariteId, cancellationToken)
            ?? throw new InvalidOperationException("Aucun tarif actif ne correspond a ce dossier pour le type d'element selectionne.");

        if (tarif.Id != dto.TarifScolariteId)
        {
            throw new InvalidOperationException("Le tarif selectionne ne correspond plus au tarif applicable pour ce dossier.");
        }

        if (dto.Id == 0)
        {
            await paiementsScolarite.AddAsync(new PaiementScolarite
            {
                DossierScolariteId = dossier.Id,
                TypeElementScolariteId = dto.TypeElementScolariteId,
                TarifScolariteId = dto.TarifScolariteId,
                ModePaiementScolariteId = dto.ModePaiementScolariteId,
                DatePaiement = dto.DatePaiement,
                Montant = dto.Montant,
                ModePaiement = mode.Libelle,
                ReferencePaiement = NormalizeText(dto.ReferencePaiement),
                EncaissePar = NormalizeText(dto.EncaissePar),
                Observation = NormalizeText(dto.Observation),
                MontantAffecte = 0,
                MontantNonAffecte = dto.Montant,
                Statut = StatutPaiementScolarite.NonAffecte,
                DateCreationUtc = DateTime.UtcNow
            }, cancellationToken);
        }
        else
        {
            var paiement = await paiementsScolarite.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new InvalidOperationException("Le paiement selectionne est introuvable.");

            if (paiement.MontantAffecte > dto.Montant)
            {
                throw new InvalidOperationException("Le montant du paiement ne peut pas devenir inferieur au montant deja affecte.");
            }

            paiement.DatePaiement = dto.DatePaiement;
            paiement.Montant = dto.Montant;
            paiement.TypeElementScolariteId = dto.TypeElementScolariteId;
            paiement.TarifScolariteId = dto.TarifScolariteId;
            paiement.ModePaiementScolariteId = dto.ModePaiementScolariteId;
            paiement.ModePaiement = mode.Libelle;
            paiement.ReferencePaiement = NormalizeText(dto.ReferencePaiement);
            paiement.EncaissePar = NormalizeText(dto.EncaissePar);
            paiement.Observation = NormalizeText(dto.Observation);
            paiement.MontantNonAffecte = dto.Montant - paiement.MontantAffecte;
            paiement.Statut = ResolvePaiementStatut(paiement.Montant, paiement.MontantAffecte);
        }

        await paiementsScolarite.SaveChangesAsync(cancellationToken);
        return await BuildFinanceDtoAsync(dossier.Id, cancellationToken);
    }

    private async Task<FinanceDossierScolariteDto> BuildFinanceDtoAsync(
        long dossierScolariteId,
        CancellationToken cancellationToken)
    {
        var elements = (await elementsScolariteEtudiants.ListAsync(cancellationToken))
            .Where(x => x.DossierScolariteId == dossierScolariteId)
            .ToList();
        var echeances = await echeancesScolarite.ListAsync(cancellationToken);
        var paiements = (await paiementsScolarite.ListAsync(cancellationToken))
            .Where(x => x.DossierScolariteId == dossierScolariteId)
            .ToList();
        var types = await typesElementsScolarite.ListAsync(cancellationToken);
        var tarifs = await GetTarifsForPaiementsAsync(paiements, cancellationToken);
        var affectations = await affectationsPaiementsEcheances.ListAsync(cancellationToken);
        var elementIds = elements.Select(x => x.Id).ToHashSet();
        var echeanceItems = echeances
            .Where(x => elementIds.Contains(x.ElementScolariteEtudiantId))
            .ToList();
        var echeanceIds = echeanceItems.Select(x => x.Id).ToHashSet();
        var paiementIds = paiements.Select(x => x.Id).ToHashSet();
        var affectationItems = affectations
            .Where(x => paiementIds.Contains(x.PaiementScolariteId) && echeanceIds.Contains(x.EcheanceScolariteId))
            .ToList();

        return new FinanceDossierScolariteDto
        {
            DossierScolariteId = dossierScolariteId,
            TotalPaiements = paiements.Where(x => x.Statut != StatutPaiementScolarite.Annule).Sum(x => x.Montant),
            TotalPaiementsAffectes = paiements.Where(x => x.Statut != StatutPaiementScolarite.Annule).Sum(x => x.MontantAffecte),
            TotalPaiementsNonAffectes = paiements.Where(x => x.Statut != StatutPaiementScolarite.Annule).Sum(x => x.MontantNonAffecte),
            Elements = elements
                .Where(x => x.MontantAttendu > 0)
                .OrderBy(x => x.Libelle)
                .Select(ToElementDto)
                .ToList(),
            Echeances = echeanceItems
                .OrderBy(x => x.DateExigibilite)
                .ThenBy(x => x.Numero)
                .Select(x => ToEcheanceDto(x, elements))
                .ToList(),
            Paiements = paiements
                .OrderByDescending(x => x.DatePaiement)
                .ThenByDescending(x => x.Id)
                .Select(x => ToPaiementDto(x, types, tarifs))
                .ToList(),
            Affectations = affectationItems
                .OrderByDescending(x => x.DateAffectationUtc)
                .Select(x => ToAffectationDto(x, paiements, echeanceItems, elements))
                .ToList()
        };
    }

    private static ElementFinancierDossierDto ToElementDto(ElementScolariteEtudiant entity)
        => new()
        {
            Id = entity.Id,
            TypeElementScolariteId = entity.TypeElementScolariteId,
            Code = entity.Code,
            Libelle = entity.Libelle,
            MontantAttendu = entity.MontantAttendu,
            MontantAffecte = entity.MontantAffecte,
            MontantRestant = entity.MontantRestant,
            Statut = entity.Statut
        };

    private static EcheanceFinanciereDossierDto ToEcheanceDto(
        EcheanceScolarite entity,
        IReadOnlyCollection<ElementScolariteEtudiant> elements)
    {
        var element = elements.FirstOrDefault(x => x.Id == entity.ElementScolariteEtudiantId);

        return new()
        {
            Id = entity.Id,
            ElementScolariteEtudiantId = entity.ElementScolariteEtudiantId,
            ElementLibelle = element?.Libelle ?? string.Empty,
            Numero = entity.Numero,
            Libelle = entity.Libelle,
            DateExigibilite = entity.DateExigibilite,
            MontantAttendu = entity.MontantAttendu,
            MontantAffecte = entity.MontantAffecte,
            MontantRestant = entity.MontantRestant,
            Statut = entity.Statut
        };
    }

    private static PaiementLibreDossierDto ToPaiementDto(
        PaiementScolarite entity,
        IReadOnlyCollection<TypeElementScolarite> types,
        IReadOnlyCollection<TarifScolariteDto> tarifs)
    {
        var type = entity.TypeElementScolariteId is null
            ? null
            : types.FirstOrDefault(x => x.Id == entity.TypeElementScolariteId);
        var tarif = entity.TarifScolariteId is null
            ? null
            : tarifs.FirstOrDefault(x => x.Id == entity.TarifScolariteId);

        return new()
        {
            Id = entity.Id,
            DossierScolariteId = entity.DossierScolariteId,
            TypeElementScolariteId = entity.TypeElementScolariteId ?? 0,
            TypeElementScolariteLibelle = type is null
                ? string.Empty
                : FormatCodeLibelle(type.Code, type.Libelle),
            TarifScolariteId = entity.TarifScolariteId ?? 0,
            TarifMontant = tarif?.Montant ?? 0,
            ModePaiementScolariteId = entity.ModePaiementScolariteId ?? 0,
            DatePaiement = entity.DatePaiement,
            Montant = entity.Montant,
            ModePaiement = entity.ModePaiement,
            ReferencePaiement = entity.ReferencePaiement,
            EncaissePar = entity.EncaissePar,
            Observation = entity.Observation,
            MontantAffecte = entity.MontantAffecte,
            MontantNonAffecte = entity.MontantNonAffecte,
            Statut = entity.Statut
        };
    }

    private async Task<TarifScolariteDto?> ResolveTarifForDossierAsync(
        DossierScolarite dossier,
        long typeElementScolariteId,
        CancellationToken cancellationToken)
    {
        var inscription = await inscriptions.GetByIdAsync(dossier.InscriptionId, cancellationToken)
            ?? throw new InvalidOperationException("L'inscription associée au dossier est introuvable.");

        return await tarifsScolariteService.ResolveTarifScolariteAsync(
            typeElementScolariteId,
            inscription.ParcoursAcademiqueId,
            cancellationToken);
    }

    private async Task<List<TarifScolariteDto>> GetTarifsForPaiementsAsync(
        IReadOnlyCollection<PaiementScolarite> paiements,
        CancellationToken cancellationToken)
    {
        var result = new List<TarifScolariteDto>();
        var tarifIds = paiements
            .Where(x => x.TarifScolariteId is not null)
            .Select(x => x.TarifScolariteId!.Value)
            .Distinct()
            .ToList();

        foreach (var tarifId in tarifIds)
        {
            var tarif = await tarifsScolariteService.GetTarifScolariteAsync(tarifId, cancellationToken);
            if (tarif is not null)
            {
                result.Add(tarif);
            }
        }

        return result;
    }

    private static string FormatCodeLibelle(string code, string libelle)
        => string.IsNullOrWhiteSpace(code) ? libelle : $"{code} - {libelle}";

    private static AffectationPaiementDossierDto ToAffectationDto(
        AffectationPaiementEcheance entity,
        IReadOnlyCollection<PaiementScolarite> paiements,
        IReadOnlyCollection<EcheanceScolarite> echeances,
        IReadOnlyCollection<ElementScolariteEtudiant> elements)
    {
        var paiement = paiements.FirstOrDefault(x => x.Id == entity.PaiementScolariteId);
        var echeance = echeances.FirstOrDefault(x => x.Id == entity.EcheanceScolariteId);
        var element = echeance is null
            ? null
            : elements.FirstOrDefault(x => x.Id == echeance.ElementScolariteEtudiantId);

        return new()
        {
            Id = entity.Id,
            PaiementScolariteId = entity.PaiementScolariteId,
            EcheanceScolariteId = entity.EcheanceScolariteId,
            PaiementLibelle = paiement is null ? string.Empty : $"{paiement.DatePaiement:dd/MM/yyyy} - {paiement.Montant:N0}",
            EcheanceLibelle = echeance is null ? string.Empty : $"{element?.Libelle} / {echeance.Libelle}",
            MontantAffecte = entity.MontantAffecte,
            DateAffectationUtc = entity.DateAffectationUtc,
            AffectePar = entity.AffectePar
        };
    }

    private static StatutPaiementScolarite ResolvePaiementStatut(decimal montant, decimal montantAffecte)
    {
        if (montantAffecte <= 0)
        {
            return StatutPaiementScolarite.NonAffecte;
        }

        return montantAffecte >= montant
            ? StatutPaiementScolarite.Affecte
            : StatutPaiementScolarite.PartiellementAffecte;
    }

    private static string RequireText(string? value, string errorMessage)
    {
        var normalized = NormalizeText(value);
        if (normalized is null)
        {
            throw new InvalidOperationException(errorMessage);
        }

        return normalized;
    }

    private static string? NormalizeText(string? value)
    {
        var normalized = value?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }
}
