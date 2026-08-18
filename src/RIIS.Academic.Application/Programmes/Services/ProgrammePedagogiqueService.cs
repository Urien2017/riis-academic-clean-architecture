using RIIS.Academic.Application.Abstractions.Persistence;
using RIIS.Academic.Application.Common.Dtos;
using RIIS.Academic.Application.Programmes.Dtos;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Programmes.Services;

public class ProgrammePedagogiqueService(
    IRepository<MaquettePedagogique> maquettes,
    IRepository<SemestrePedagogique> semestres,
    IRepository<UniteEnseignement> unitesEnseignement,
    IRepository<ElementConstitutif> elementsConstitutifs,
    IRepository<AnneeAcademique> anneesAcademiques,
    IRepository<CycleFormation> cyclesFormation,
    IRepository<ParcoursAcademique> parcoursAcademiques,
    IRepository<ClassePedagogique> classesPedagogiques,
    IRepository<NiveauEtude> niveauxEtude) : IProgrammePedagogiqueService
{
    public async Task<List<MaquettePedagogiqueDto>> GetMaquettesAsync(CancellationToken cancellationToken = default)
    {
        var maquetteItems = await maquettes.ListAsync(cancellationToken);
        var ouvertureItems = await parcoursAcademiques.ListAsync(cancellationToken);
        var semestreItems = await semestres.ListAsync(cancellationToken);

        return maquetteItems
            .Select(x => ToDto(x, ouvertureItems, semestreItems))
            .OrderBy(x => x.ParcoursAcademiqueLibelle)
            .ThenBy(x => x.Libelle)
            .ThenBy(x => x.Version)
            .ToList();
    }

    public async Task<List<MaquettePedagogiqueHierarchyDto>> GetMaquettesHierarchyAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? maquettePedagogiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var maquetteItems = await maquettes.ListAsync(cancellationToken);
        var anneeItems = await anneesAcademiques.ListAsync(cancellationToken);
        var ouvertureItems = await parcoursAcademiques.ListAsync(cancellationToken);
        var classeItems = await classesPedagogiques.ListAsync(cancellationToken);
        var semestreItems = await semestres.ListAsync(cancellationToken);
        var niveauItems = await niveauxEtude.ListAsync(cancellationToken);
        var ueItems = await unitesEnseignement.ListAsync(cancellationToken);
        var ecItems = await elementsConstitutifs.ListAsync(cancellationToken);

        maquetteItems = FilterMaquettes(maquetteItems, anneeItems, ouvertureItems, classeItems, anneeAcademiqueId, cycleFormationId, maquettePedagogiqueId);

        return maquetteItems
            .OrderBy(x => x.Libelle)
            .ThenBy(x => x.Version)
            .Select(maquette =>
            {
                var ouverture = FindRepresentativeParcours(maquette, ouvertureItems);

                return new MaquettePedagogiqueHierarchyDto
                {
                    Id = maquette.Id,
                    ParcoursAcademiqueId = ouverture?.Id ?? 0,
                    ParcoursAcademiqueLibelle = ouverture is null ? string.Empty : FormatCodeLibelle(ouverture.Code, ouverture.Libelle),
                    Code = maquette.Code,
                    Libelle = maquette.Libelle,
                    Version = maquette.Version,
                    Statut = maquette.Statut,
                    DateDebutValidite = maquette.DateDebutValidite,
                    DateFinValidite = maquette.DateFinValidite,
                    SourceDocument = maquette.SourceDocument,
                    Observation = maquette.Observation,
                    Semestres = semestreItems
                        .Where(semestre => semestre.MaquettePedagogiqueId == maquette.Id)
                        .OrderBy(semestre => semestre.OrdreAffichage)
                        .ThenBy(semestre => semestre.Numero)
                        .Select(semestre =>
                        {
                            var niveau = semestre.NiveauEtudeId is null
                                ? null
                                : niveauItems.FirstOrDefault(x => x.Id == semestre.NiveauEtudeId);

                            return new SemestrePedagogiqueHierarchyDto
                            {
                                Id = semestre.Id,
                                Numero = semestre.Numero,
                                Libelle = semestre.Libelle,
                                NiveauEtudeLibelle = niveau?.Libelle,
                                CreditsAttendus = semestre.CreditsAttendus,
                                VolumeHoraireAttendu = semestre.VolumeHoraireAttendu,
                                OrdreAffichage = semestre.OrdreAffichage,
                                UnitesEnseignement = ueItems
                                    .Where(ue => ue.SemestrePedagogiqueId == semestre.Id)
                                    .OrderBy(ue => ue.OrdreAffichage)
                                    .ThenBy(ue => ue.Code)
                                    .Select(ue => new UniteEnseignementHierarchyDto
                                    {
                                        Id = ue.Id,
                                        SemestrePedagogiqueId = ue.SemestrePedagogiqueId,
                                        SemestrePedagogiqueLibelle = $"{semestre.Numero} - {semestre.Libelle}",
                                        Code = ue.Code,
                                        Libelle = ue.Libelle,
                                        Credits = ue.Credits,
                                        VolumeHoraire = ue.VolumeHoraire,
                                        OrdreAffichage = ue.OrdreAffichage,
                                        EstObligatoire = ue.EstObligatoire,
                                        ElementsConstitutifs = ecItems
                                            .Where(ec => ec.UniteEnseignementId == ue.Id)
                                            .OrderBy(ec => ec.OrdreAffichage)
                                            .ThenBy(ec => ec.Libelle)
                                            .Select(ec => new ElementConstitutifHierarchyDto
                                            {
                                                Id = ec.Id,
                                                Code = ec.Code,
                                                Libelle = ec.Libelle,
                                                Type = ec.Type,
                                                Credits = ec.Credits,
                                                Coefficient = ec.Coefficient,
                                                VolumeHoraire = ec.VolumeHoraire,
                                                OrdreAffichage = ec.OrdreAffichage,
                                                EstObligatoire = ec.EstObligatoire,
                                                Observation = ec.Observation
                                            })
                                            .ToList()
                                    })
                                    .ToList()
                            };
                        })
                        .ToList()
                };
            })
            .ToList();
    }

    public async Task<MaquettePedagogiqueDto?> GetMaquetteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await maquettes.GetByIdAsync(id, cancellationToken);
        if (entity is null) return null;

        var ouvertureItems = await parcoursAcademiques.ListAsync(cancellationToken);
        var semestreItems = await semestres.ListAsync(cancellationToken);

        return ToDto(entity, ouvertureItems, semestreItems);
    }

    public MaquettePedagogiqueDto CreateDefaultMaquette(long? parcoursAcademiqueId = null)
        => new()
        {
            ParcoursAcademiqueId = parcoursAcademiqueId ?? 0,
            Version = "V1",
            Statut = StatutMaquettePedagogique.Brouillon
        };

    public async Task SaveMaquetteAsync(MaquettePedagogiqueDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.ParcoursAcademiqueId <= 0)
        {
            throw new InvalidOperationException("Le parcours cycle/filière/spécialité de la maquette est obligatoire.");
        }

        dto.Code = NormalizeCode(dto.Code, "Le code de la maquette est obligatoire.");
        dto.Libelle = RequireText(dto.Libelle, "Le libellé de la maquette est obligatoire.");
        dto.Version = RequireText(dto.Version, "La version de la maquette est obligatoire.").ToUpperInvariant();
        dto.SourceDocument = NormalizeNullable(dto.SourceDocument);
        dto.Observation = NormalizeNullable(dto.Observation);

        if (dto.DateDebutValidite is not null
            && dto.DateFinValidite is not null
            && dto.DateFinValidite < dto.DateDebutValidite)
        {
            throw new InvalidOperationException("La date de fin de validité doit être supérieure ou égale à la date de début.");
        }

        var parcours = await GetParcoursAcademiqueAsync(dto.ParcoursAcademiqueId, cancellationToken);

        var existing = await maquettes.ListAsync(cancellationToken);
        var duplicate = existing.Any(x =>
            x.Id != dto.Id
            && x.CycleFormationId == parcours.CycleFormationId
            && x.NiveauEtudeId == parcours.NiveauEtudeId
            && x.FiliereId == parcours.FiliereId
            && x.SpecialiteId == parcours.SpecialiteId
            && string.Equals(x.Code, dto.Code, StringComparison.OrdinalIgnoreCase)
            && string.Equals(x.Version, dto.Version, StringComparison.OrdinalIgnoreCase));

        if (duplicate)
        {
            throw new InvalidOperationException("Une maquette avec le même code et la même version existe déjà pour ce parcours.");
        }

        if (dto.Id == 0)
        {
            await maquettes.AddAsync(new MaquettePedagogique
            {
                CycleFormationId = parcours.CycleFormationId,
                NiveauEtudeId = parcours.NiveauEtudeId,
                FiliereId = parcours.FiliereId,
                SpecialiteId = parcours.SpecialiteId,
                Code = dto.Code,
                Libelle = dto.Libelle,
                Version = dto.Version,
                Statut = dto.Statut,
                DateDebutValidite = dto.DateDebutValidite,
                DateFinValidite = dto.DateFinValidite,
                SourceDocument = dto.SourceDocument,
                Observation = dto.Observation
            }, cancellationToken);
        }
        else
        {
            var entity = await maquettes.GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null) return;

            entity.CycleFormationId = parcours.CycleFormationId;
            entity.NiveauEtudeId = parcours.NiveauEtudeId;
            entity.FiliereId = parcours.FiliereId;
            entity.SpecialiteId = parcours.SpecialiteId;
            entity.Code = dto.Code;
            entity.Libelle = dto.Libelle;
            entity.Version = dto.Version;
            entity.Statut = dto.Statut;
            entity.DateDebutValidite = dto.DateDebutValidite;
            entity.DateFinValidite = dto.DateFinValidite;
            entity.SourceDocument = dto.SourceDocument;
            entity.Observation = dto.Observation;
        }

        await maquettes.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteMaquetteAsync(long id, CancellationToken cancellationToken = default)
    {
        await maquettes.DeleteByIdAsync(id, cancellationToken);
        await maquettes.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<SemestrePedagogiqueDto>> GetSemestresAsync(
        long? maquettePedagogiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var semestreItems = await semestres.ListAsync(cancellationToken);

        if (maquettePedagogiqueId is not null)
        {
            semestreItems = semestreItems.Where(x => x.MaquettePedagogiqueId == maquettePedagogiqueId).ToList();
        }

        var maquetteItems = await maquettes.ListAsync(cancellationToken);
        var niveauItems = await niveauxEtude.ListAsync(cancellationToken);
        var ueItems = await unitesEnseignement.ListAsync(cancellationToken);

        return semestreItems
            .Select(x => ToDto(x, maquetteItems, niveauItems, ueItems))
            .OrderBy(x => x.MaquettePedagogiqueLibelle)
            .ThenBy(x => x.OrdreAffichage)
            .ThenBy(x => x.Numero)
            .ToList();
    }

    public async Task<SemestrePedagogiqueDto?> GetSemestreAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await semestres.GetByIdAsync(id, cancellationToken);
        if (entity is null) return null;

        var maquetteItems = await maquettes.ListAsync(cancellationToken);
        var niveauItems = await niveauxEtude.ListAsync(cancellationToken);
        var ueItems = await unitesEnseignement.ListAsync(cancellationToken);

        return ToDto(entity, maquetteItems, niveauItems, ueItems);
    }

    public async Task<SemestrePedagogiqueDto> CreateDefaultSemestreAsync(
        long? maquettePedagogiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var semestreItems = await semestres.ListAsync(cancellationToken);
        var filtered = maquettePedagogiqueId is null
            ? semestreItems
            : semestreItems.Where(x => x.MaquettePedagogiqueId == maquettePedagogiqueId).ToList();
        var nextNumero = filtered.Count == 0 ? (byte)1 : (byte)Math.Min(10, filtered.Max(x => x.Numero) + 1);

        return new SemestrePedagogiqueDto
        {
            MaquettePedagogiqueId = maquettePedagogiqueId ?? 0,
            Numero = nextNumero,
            Libelle = $"Semestre {nextNumero}",
            CreditsAttendus = 30m,
            VolumeHoraireAttendu = 450,
            OrdreAffichage = nextNumero
        };
    }

    public async Task SaveSemestreAsync(SemestrePedagogiqueDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.MaquettePedagogiqueId <= 0)
        {
            throw new InvalidOperationException("La maquette du semestre est obligatoire.");
        }

        if (dto.Numero is < 1 or > 10)
        {
            throw new InvalidOperationException("Le numéro du semestre doit être compris entre 1 et 10.");
        }

        if (dto.CreditsAttendus < 0 || dto.VolumeHoraireAttendu < 0)
        {
            throw new InvalidOperationException("Les crédits et volumes horaires doivent être positifs.");
        }

        dto.Libelle = RequireText(dto.Libelle, "Le libellé du semestre est obligatoire.");
        dto.OrdreAffichage = dto.OrdreAffichage == 0 ? dto.Numero : dto.OrdreAffichage;

        await EnsureMaquetteExistsAsync(dto.MaquettePedagogiqueId, cancellationToken);
        await EnsureNiveauExistsAsync(dto.NiveauEtudeId, cancellationToken);

        var existing = await semestres.ListAsync(cancellationToken);
        var duplicate = existing.Any(x =>
            x.Id != dto.Id
            && x.MaquettePedagogiqueId == dto.MaquettePedagogiqueId
            && x.Numero == dto.Numero);

        if (duplicate)
        {
            throw new InvalidOperationException("Ce numéro de semestre existe déjà dans cette maquette.");
        }

        if (dto.Id == 0)
        {
            await semestres.AddAsync(new SemestrePedagogique
            {
                MaquettePedagogiqueId = dto.MaquettePedagogiqueId,
                NiveauEtudeId = dto.NiveauEtudeId,
                Numero = dto.Numero,
                Libelle = dto.Libelle,
                CreditsAttendus = dto.CreditsAttendus,
                VolumeHoraireAttendu = dto.VolumeHoraireAttendu,
                OrdreAffichage = dto.OrdreAffichage
            }, cancellationToken);
        }
        else
        {
            var entity = await semestres.GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null) return;

            entity.MaquettePedagogiqueId = dto.MaquettePedagogiqueId;
            entity.NiveauEtudeId = dto.NiveauEtudeId;
            entity.Numero = dto.Numero;
            entity.Libelle = dto.Libelle;
            entity.CreditsAttendus = dto.CreditsAttendus;
            entity.VolumeHoraireAttendu = dto.VolumeHoraireAttendu;
            entity.OrdreAffichage = dto.OrdreAffichage;
        }

        await semestres.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSemestreAsync(long id, CancellationToken cancellationToken = default)
    {
        await semestres.DeleteByIdAsync(id, cancellationToken);
        await semestres.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<UniteEnseignementDto>> GetUnitesEnseignementAsync(
        long? semestrePedagogiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var ueItems = await unitesEnseignement.ListAsync(cancellationToken);

        if (semestrePedagogiqueId is not null)
        {
            ueItems = ueItems.Where(x => x.SemestrePedagogiqueId == semestrePedagogiqueId).ToList();
        }

        var semestreItems = await semestres.ListAsync(cancellationToken);
        var ecItems = await elementsConstitutifs.ListAsync(cancellationToken);

        return ueItems
            .Select(x => ToDto(x, semestreItems, ecItems))
            .OrderBy(x => x.SemestrePedagogiqueLibelle)
            .ThenBy(x => x.OrdreAffichage)
            .ThenBy(x => x.Code)
            .ToList();
    }

    public async Task<List<UniteEnseignementDto>> GetUnitesEnseignementByHierarchyAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? parcoursAcademiqueId = null,
        long? semestrePedagogiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var ueItems = await unitesEnseignement.ListAsync(cancellationToken);
        var semestreItems = await GetFilteredSemestresAsync(
            anneeAcademiqueId,
            cycleFormationId,
            parcoursAcademiqueId,
            cancellationToken);

        if (semestrePedagogiqueId is not null)
        {
            semestreItems = semestreItems
                .Where(x => x.Id == semestrePedagogiqueId)
                .ToList();
        }

        var semestreIds = semestreItems.Select(x => x.Id).ToHashSet();
        ueItems = ueItems
            .Where(x => semestreIds.Contains(x.SemestrePedagogiqueId))
            .ToList();

        var ecItems = await elementsConstitutifs.ListAsync(cancellationToken);

        return ueItems
            .Select(x => ToDto(x, semestreItems, ecItems))
            .OrderBy(x => x.SemestrePedagogiqueLibelle)
            .ThenBy(x => x.OrdreAffichage)
            .ThenBy(x => x.Code)
            .ToList();
    }

    public async Task<List<UniteEnseignementHierarchyDto>> GetUnitesEnseignementHierarchyAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? parcoursAcademiqueId = null,
        long? semestrePedagogiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var ueItems = await unitesEnseignement.ListAsync(cancellationToken);
        var semestreItems = await GetFilteredSemestresAsync(
            anneeAcademiqueId,
            cycleFormationId,
            parcoursAcademiqueId,
            cancellationToken);

        if (semestrePedagogiqueId is not null)
        {
            semestreItems = semestreItems
                .Where(x => x.Id == semestrePedagogiqueId)
                .ToList();
        }

        var semestreIds = semestreItems.Select(x => x.Id).ToHashSet();
        ueItems = ueItems
            .Where(x => semestreIds.Contains(x.SemestrePedagogiqueId))
            .ToList();

        var ecItems = await elementsConstitutifs.ListAsync(cancellationToken);

        return ueItems
            .OrderBy(x => GetSemestreOrder(x.SemestrePedagogiqueId, semestreItems))
            .ThenBy(x => x.OrdreAffichage)
            .ThenBy(x => x.Code)
            .Select(ue =>
            {
                var semestre = semestreItems.FirstOrDefault(x => x.Id == ue.SemestrePedagogiqueId);

                return new UniteEnseignementHierarchyDto
                {
                    Id = ue.Id,
                    SemestrePedagogiqueId = ue.SemestrePedagogiqueId,
                    SemestrePedagogiqueLibelle = semestre is null ? string.Empty : $"{semestre.Numero} - {semestre.Libelle}",
                    Code = ue.Code,
                    Libelle = ue.Libelle,
                    Credits = ue.Credits,
                    VolumeHoraire = ue.VolumeHoraire,
                    OrdreAffichage = ue.OrdreAffichage,
                    EstObligatoire = ue.EstObligatoire,
                    ElementsConstitutifs = ecItems
                        .Where(ec => ec.UniteEnseignementId == ue.Id)
                        .OrderBy(ec => ec.OrdreAffichage)
                        .ThenBy(ec => ec.Libelle)
                        .Select(ec => new ElementConstitutifHierarchyDto
                        {
                            Id = ec.Id,
                            Code = ec.Code,
                            Libelle = ec.Libelle,
                            Type = ec.Type,
                            Credits = ec.Credits,
                            Coefficient = ec.Coefficient,
                            VolumeHoraire = ec.VolumeHoraire,
                            OrdreAffichage = ec.OrdreAffichage,
                            EstObligatoire = ec.EstObligatoire,
                            Observation = ec.Observation
                        })
                        .ToList()
                };
            })
            .ToList();
    }

    public async Task<UniteEnseignementDto?> GetUniteEnseignementAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await unitesEnseignement.GetByIdAsync(id, cancellationToken);
        if (entity is null) return null;

        var semestreItems = await semestres.ListAsync(cancellationToken);
        var ecItems = await elementsConstitutifs.ListAsync(cancellationToken);

        return ToDto(entity, semestreItems, ecItems);
    }

    public async Task<UniteEnseignementDto> CreateDefaultUniteEnseignementAsync(
        long? semestrePedagogiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var ueItems = await unitesEnseignement.ListAsync(cancellationToken);
        var filtered = semestrePedagogiqueId is null
            ? ueItems
            : ueItems.Where(x => x.SemestrePedagogiqueId == semestrePedagogiqueId).ToList();
        var nextOrder = filtered.Count == 0 ? (short)1 : (short)(filtered.Max(x => x.OrdreAffichage) + 1);

        return new UniteEnseignementDto
        {
            SemestrePedagogiqueId = semestrePedagogiqueId ?? 0,
            Code = $"UE{nextOrder}",
            OrdreAffichage = nextOrder,
            EstObligatoire = true
        };
    }

    public async Task SaveUniteEnseignementAsync(UniteEnseignementDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.SemestrePedagogiqueId <= 0)
        {
            throw new InvalidOperationException("Le semestre de l'UE est obligatoire.");
        }

        if (dto.Credits < 0 || dto.VolumeHoraire < 0)
        {
            throw new InvalidOperationException("Les crédits et volumes horaires doivent être positifs.");
        }

        dto.Code = NormalizeCode(dto.Code, "Le code de l'UE est obligatoire.");
        dto.Libelle = RequireText(dto.Libelle, "Le libellé de l'UE est obligatoire.");

        await EnsureSemestreExistsAsync(dto.SemestrePedagogiqueId, cancellationToken);

        var existing = await unitesEnseignement.ListAsync(cancellationToken);
        var duplicate = existing.Any(x =>
            x.Id != dto.Id
            && x.SemestrePedagogiqueId == dto.SemestrePedagogiqueId
            && string.Equals(x.Code, dto.Code, StringComparison.OrdinalIgnoreCase));

        if (duplicate)
        {
            throw new InvalidOperationException("Une UE avec le même code existe déjà dans ce semestre.");
        }

        if (dto.Id == 0)
        {
            await unitesEnseignement.AddAsync(new UniteEnseignement
            {
                SemestrePedagogiqueId = dto.SemestrePedagogiqueId,
                Code = dto.Code,
                Libelle = dto.Libelle,
                Credits = dto.Credits,
                VolumeHoraire = dto.VolumeHoraire,
                OrdreAffichage = dto.OrdreAffichage,
                EstObligatoire = dto.EstObligatoire
            }, cancellationToken);
        }
        else
        {
            var entity = await unitesEnseignement.GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null) return;

            entity.SemestrePedagogiqueId = dto.SemestrePedagogiqueId;
            entity.Code = dto.Code;
            entity.Libelle = dto.Libelle;
            entity.Credits = dto.Credits;
            entity.VolumeHoraire = dto.VolumeHoraire;
            entity.OrdreAffichage = dto.OrdreAffichage;
            entity.EstObligatoire = dto.EstObligatoire;
        }

        await unitesEnseignement.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUniteEnseignementAsync(long id, CancellationToken cancellationToken = default)
    {
        await unitesEnseignement.DeleteByIdAsync(id, cancellationToken);
        await unitesEnseignement.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<ElementConstitutifDto>> GetElementsConstitutifsAsync(
        long? uniteEnseignementId = null,
        CancellationToken cancellationToken = default)
    {
        var ecItems = await elementsConstitutifs.ListAsync(cancellationToken);

        if (uniteEnseignementId is not null)
        {
            ecItems = ecItems.Where(x => x.UniteEnseignementId == uniteEnseignementId).ToList();
        }

        var ueItems = await unitesEnseignement.ListAsync(cancellationToken);

        return ecItems
            .Select(x => ToDto(x, ueItems))
            .OrderBy(x => x.UniteEnseignementLibelle)
            .ThenBy(x => x.OrdreAffichage)
            .ThenBy(x => x.Libelle)
            .ToList();
    }

    public async Task<ElementConstitutifDto?> GetElementConstitutifAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await elementsConstitutifs.GetByIdAsync(id, cancellationToken);
        if (entity is null) return null;

        var ueItems = await unitesEnseignement.ListAsync(cancellationToken);

        return ToDto(entity, ueItems);
    }

    public async Task<ElementConstitutifDto> CreateDefaultElementConstitutifAsync(
        long? uniteEnseignementId = null,
        CancellationToken cancellationToken = default)
    {
        var ecItems = await elementsConstitutifs.ListAsync(cancellationToken);
        var filtered = uniteEnseignementId is null
            ? ecItems
            : ecItems.Where(x => x.UniteEnseignementId == uniteEnseignementId).ToList();
        var nextOrder = filtered.Count == 0 ? (short)1 : (short)(filtered.Max(x => x.OrdreAffichage) + 1);

        return new ElementConstitutifDto
        {
            UniteEnseignementId = uniteEnseignementId ?? 0,
            Code = $"EC{nextOrder}",
            Type = TypeElementConstitutif.Cours,
            Coefficient = 1m,
            OrdreAffichage = nextOrder,
            EstObligatoire = true
        };
    }

    public async Task SaveElementConstitutifAsync(ElementConstitutifDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.UniteEnseignementId <= 0)
        {
            throw new InvalidOperationException("L'UE de l'EC est obligatoire.");
        }

        if (dto.Credits < 0 || dto.Coefficient < 0 || dto.VolumeHoraire < 0)
        {
            throw new InvalidOperationException("Les crédits, coefficients et volumes horaires doivent être positifs.");
        }

        dto.Code = NormalizeNullable(dto.Code)?.ToUpperInvariant();
        dto.Libelle = RequireText(dto.Libelle, "Le libellé de l'EC est obligatoire.");
        dto.Coefficient = dto.Coefficient == 0 ? 1m : dto.Coefficient;
        dto.Observation = NormalizeNullable(dto.Observation);

        await EnsureUniteEnseignementExistsAsync(dto.UniteEnseignementId, cancellationToken);

        var existing = await elementsConstitutifs.ListAsync(cancellationToken);

        if (dto.Code is not null)
        {
            var duplicateCode = existing.Any(x =>
                x.Id != dto.Id
                && x.UniteEnseignementId == dto.UniteEnseignementId
                && string.Equals(x.Code, dto.Code, StringComparison.OrdinalIgnoreCase));

            if (duplicateCode)
            {
                throw new InvalidOperationException("Un EC avec le même code existe déjà dans cette UE.");
            }
        }

        var duplicateOrder = existing.Any(x =>
            x.Id != dto.Id
            && x.UniteEnseignementId == dto.UniteEnseignementId
            && x.OrdreAffichage == dto.OrdreAffichage);

        if (duplicateOrder)
        {
            throw new InvalidOperationException("Un EC avec le même ordre d'affichage existe déjà dans cette UE.");
        }

        if (dto.Id == 0)
        {
            await elementsConstitutifs.AddAsync(new ElementConstitutif
            {
                UniteEnseignementId = dto.UniteEnseignementId,
                Code = dto.Code,
                Libelle = dto.Libelle,
                Type = dto.Type,
                Credits = dto.Credits,
                Coefficient = dto.Coefficient,
                VolumeHoraire = dto.VolumeHoraire,
                OrdreAffichage = dto.OrdreAffichage,
                EstObligatoire = dto.EstObligatoire,
                Observation = dto.Observation
            }, cancellationToken);
        }
        else
        {
            var entity = await elementsConstitutifs.GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null) return;

            entity.UniteEnseignementId = dto.UniteEnseignementId;
            entity.Code = dto.Code;
            entity.Libelle = dto.Libelle;
            entity.Type = dto.Type;
            entity.Credits = dto.Credits;
            entity.Coefficient = dto.Coefficient;
            entity.VolumeHoraire = dto.VolumeHoraire;
            entity.OrdreAffichage = dto.OrdreAffichage;
            entity.EstObligatoire = dto.EstObligatoire;
            entity.Observation = dto.Observation;
        }

        await elementsConstitutifs.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteElementConstitutifAsync(long id, CancellationToken cancellationToken = default)
    {
        await elementsConstitutifs.DeleteByIdAsync(id, cancellationToken);
        await elementsConstitutifs.SaveChangesAsync(cancellationToken);
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

    public async Task<List<LookupDto>> GetParcoursAcademiquesLookupAsync(
        long? cycleFormationId = null,
        CancellationToken cancellationToken = default)
    {
        var items = await parcoursAcademiques.ListAsync(cancellationToken);

        if (cycleFormationId is not null)
        {
            items = items
                .Where(x => x.CycleFormationId == cycleFormationId)
                .ToList();
        }

        return items
            .Where(x => x.EstActive)
            .OrderBy(x => x.Libelle)
            .Select(x => new LookupDto { Id = x.Id, Libelle = FormatCodeLibelle(x.Code, x.Libelle) })
            .ToList();
    }

    public async Task<List<LookupDto>> GetMaquettesLookupAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        CancellationToken cancellationToken = default)
    {
        var items = await maquettes.ListAsync(cancellationToken);
        var anneeItems = await anneesAcademiques.ListAsync(cancellationToken);
        var ouvertureItems = await parcoursAcademiques.ListAsync(cancellationToken);
        var classeItems = await classesPedagogiques.ListAsync(cancellationToken);

        items = FilterMaquettes(items, anneeItems, ouvertureItems, classeItems, anneeAcademiqueId, cycleFormationId, null);

        return items
            .OrderBy(x => x.Libelle)
            .ThenBy(x => x.Version)
            .Select(x => new LookupDto { Id = x.Id, Libelle = $"{x.Code} - {x.Libelle} ({x.Version})" })
            .ToList();
    }

    private static List<MaquettePedagogique> FilterMaquettes(
        List<MaquettePedagogique> maquetteItems,
        IReadOnlyCollection<AnneeAcademique> anneeItems,
        IReadOnlyCollection<ParcoursAcademique> ouvertureItems,
        IReadOnlyCollection<ClassePedagogique> classeItems,
        long? anneeAcademiqueId,
        long? cycleFormationId,
        long? maquettePedagogiqueId)
    {
        IEnumerable<MaquettePedagogique> query = maquetteItems;

        if (anneeAcademiqueId is not null)
        {
            var annee = anneeItems.FirstOrDefault(x => x.Id == anneeAcademiqueId);
            var maquetteIdsPourAnnee = classeItems
                .Where(x => x.AnneeAcademiqueId == anneeAcademiqueId && x.MaquettePedagogiqueId is not null)
                .Select(x => x.MaquettePedagogiqueId!.Value)
                .ToHashSet();

            query = query.Where(x =>
                maquetteIdsPourAnnee.Contains(x.Id)
                || (annee is not null && IsMaquetteValidForAcademicYear(x, annee)));
        }

        if (cycleFormationId is not null)
        {
            query = query.Where(x => x.CycleFormationId == cycleFormationId);
        }

        if (maquettePedagogiqueId is not null)
        {
            query = query.Where(x => x.Id == maquettePedagogiqueId);
        }

        return query.ToList();
    }

    private static bool IsMaquetteValidForAcademicYear(MaquettePedagogique maquette, AnneeAcademique annee)
    {
        var academicStart = new DateOnly(annee.AnneeDebut, 9, 1);
        var academicEnd = new DateOnly(annee.AnneeFin, 8, 31);

        var maquetteStart = maquette.DateDebutValidite ?? DateOnly.MinValue;
        var maquetteEnd = maquette.DateFinValidite ?? DateOnly.MaxValue;

        return maquetteStart <= academicEnd && maquetteEnd >= academicStart;
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

    public async Task<List<LookupDto>> GetSemestresLookupAsync(
        long? maquettePedagogiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var items = await semestres.ListAsync(cancellationToken);

        if (maquettePedagogiqueId is not null)
        {
            items = items.Where(x => x.MaquettePedagogiqueId == maquettePedagogiqueId).ToList();
        }

        return items
            .OrderBy(x => x.OrdreAffichage)
            .ThenBy(x => x.Numero)
            .Select(x => new LookupDto { Id = x.Id, Libelle = $"{x.Numero} - {x.Libelle}" })
            .ToList();
    }

    public async Task<List<LookupDto>> GetSemestresHierarchyLookupAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? parcoursAcademiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var items = await GetFilteredSemestresAsync(
            anneeAcademiqueId,
            cycleFormationId,
            parcoursAcademiqueId,
            cancellationToken);

        return items
            .OrderBy(x => x.OrdreAffichage)
            .ThenBy(x => x.Numero)
            .Select(x => new LookupDto { Id = x.Id, Libelle = $"{x.Numero} - {x.Libelle}" })
            .ToList();
    }

    public async Task<List<LookupDto>> GetUnitesEnseignementLookupAsync(
        long? semestrePedagogiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var items = await unitesEnseignement.ListAsync(cancellationToken);

        if (semestrePedagogiqueId is not null)
        {
            items = items.Where(x => x.SemestrePedagogiqueId == semestrePedagogiqueId).ToList();
        }

        return items
            .OrderBy(x => x.OrdreAffichage)
            .ThenBy(x => x.Code)
            .Select(x => new LookupDto { Id = x.Id, Libelle = FormatCodeLibelle(x.Code, x.Libelle) })
            .ToList();
    }

    private async Task<ParcoursAcademique> GetParcoursAcademiqueAsync(long id, CancellationToken cancellationToken)
    {
        var parcours = (await parcoursAcademiques.ListAsync(cancellationToken)).FirstOrDefault(x => x.Id == id);
        return parcours ?? throw new InvalidOperationException("Le parcours sélectionné est introuvable.");
    }

    private async Task<List<SemestrePedagogique>> GetFilteredSemestresAsync(
        long? anneeAcademiqueId,
        long? cycleFormationId,
        long? parcoursAcademiqueId,
        CancellationToken cancellationToken)
    {
        var semestreItems = await semestres.ListAsync(cancellationToken);
        var maquetteItems = await maquettes.ListAsync(cancellationToken);
        var anneeItems = await anneesAcademiques.ListAsync(cancellationToken);
        var ouvertureItems = await parcoursAcademiques.ListAsync(cancellationToken);
        var classeItems = await classesPedagogiques.ListAsync(cancellationToken);

        maquetteItems = FilterMaquettes(
            maquetteItems,
            anneeItems,
            ouvertureItems,
            classeItems,
            anneeAcademiqueId,
            cycleFormationId,
            null);

        if (parcoursAcademiqueId is not null)
        {
            var ouverture = ouvertureItems.FirstOrDefault(x => x.Id == parcoursAcademiqueId);
            maquetteItems = maquetteItems
                .Where(x => ouverture is not null && IsMaquetteCompatibleWithParcours(x, ouverture))
                .ToList();
        }

        var maquetteIds = maquetteItems.Select(x => x.Id).ToHashSet();

        return semestreItems
            .Where(x => maquetteIds.Contains(x.MaquettePedagogiqueId))
            .ToList();
    }

    private async Task EnsureMaquetteExistsAsync(long id, CancellationToken cancellationToken)
    {
        var exists = (await maquettes.ListAsync(cancellationToken)).Any(x => x.Id == id);
        if (!exists) throw new InvalidOperationException("La maquette sélectionnée est introuvable.");
    }

    private async Task EnsureNiveauExistsAsync(long? id, CancellationToken cancellationToken)
    {
        if (id is null) return;

        var exists = (await niveauxEtude.ListAsync(cancellationToken)).Any(x => x.Id == id);
        if (!exists) throw new InvalidOperationException("Le niveau sélectionné est introuvable.");
    }

    private async Task EnsureSemestreExistsAsync(long id, CancellationToken cancellationToken)
    {
        var exists = (await semestres.ListAsync(cancellationToken)).Any(x => x.Id == id);
        if (!exists) throw new InvalidOperationException("Le semestre sélectionné est introuvable.");
    }

    private async Task EnsureUniteEnseignementExistsAsync(long id, CancellationToken cancellationToken)
    {
        var exists = (await unitesEnseignement.ListAsync(cancellationToken)).Any(x => x.Id == id);
        if (!exists) throw new InvalidOperationException("L'UE sélectionnée est introuvable.");
    }

    private static short GetSemestreOrder(
        long semestrePedagogiqueId,
        IReadOnlyCollection<SemestrePedagogique> semestreItems)
        => semestreItems.FirstOrDefault(x => x.Id == semestrePedagogiqueId)?.OrdreAffichage ?? short.MaxValue;

    private static MaquettePedagogiqueDto ToDto(
        MaquettePedagogique maquette,
        IReadOnlyCollection<ParcoursAcademique> ouvertures,
        IReadOnlyCollection<SemestrePedagogique> semestreItems)
    {
        var ouverture = FindRepresentativeParcours(maquette, ouvertures);

        return new MaquettePedagogiqueDto
        {
            Id = maquette.Id,
            ParcoursAcademiqueId = ouverture?.Id ?? 0,
            ParcoursAcademiqueLibelle = ouverture is null ? string.Empty : FormatCodeLibelle(ouverture.Code, ouverture.Libelle),
            Code = maquette.Code,
            Libelle = maquette.Libelle,
            Version = maquette.Version,
            Statut = maquette.Statut,
            DateDebutValidite = maquette.DateDebutValidite,
            DateFinValidite = maquette.DateFinValidite,
            SourceDocument = maquette.SourceDocument,
            Observation = maquette.Observation,
            NombreSemestres = semestreItems.Count(x => x.MaquettePedagogiqueId == maquette.Id)
        };
    }

    private static ParcoursAcademique? FindRepresentativeParcours(
        MaquettePedagogique maquette,
        IReadOnlyCollection<ParcoursAcademique> ouvertures)
        => ouvertures
            .Where(ouverture => IsMaquetteCompatibleWithParcours(maquette, ouverture))
            .OrderByDescending(x => x.EstActive)
            .ThenByDescending(x => x.AnneeAcademiqueId)
            .ThenBy(x => x.Code)
            .FirstOrDefault();

    private static bool IsMaquetteCompatibleWithParcours(
        MaquettePedagogique maquette,
        ParcoursAcademique parcours)
        => maquette.CycleFormationId == parcours.CycleFormationId
            && maquette.NiveauEtudeId == parcours.NiveauEtudeId
            && maquette.FiliereId == parcours.FiliereId
            && maquette.SpecialiteId == parcours.SpecialiteId;

    private static SemestrePedagogiqueDto ToDto(
        SemestrePedagogique semestre,
        IReadOnlyCollection<MaquettePedagogique> maquetteItems,
        IReadOnlyCollection<NiveauEtude> niveauItems,
        IReadOnlyCollection<UniteEnseignement> ueItems)
    {
        var maquette = maquetteItems.FirstOrDefault(x => x.Id == semestre.MaquettePedagogiqueId);
        var niveau = semestre.NiveauEtudeId is null ? null : niveauItems.FirstOrDefault(x => x.Id == semestre.NiveauEtudeId);

        return new SemestrePedagogiqueDto
        {
            Id = semestre.Id,
            MaquettePedagogiqueId = semestre.MaquettePedagogiqueId,
            MaquettePedagogiqueLibelle = maquette is null ? string.Empty : $"{maquette.Code} - {maquette.Libelle} ({maquette.Version})",
            NiveauEtudeId = semestre.NiveauEtudeId,
            NiveauEtudeLibelle = niveau?.Libelle,
            Numero = semestre.Numero,
            Libelle = semestre.Libelle,
            CreditsAttendus = semestre.CreditsAttendus,
            VolumeHoraireAttendu = semestre.VolumeHoraireAttendu,
            OrdreAffichage = semestre.OrdreAffichage,
            NombreUnitesEnseignement = ueItems.Count(x => x.SemestrePedagogiqueId == semestre.Id)
        };
    }

    private static UniteEnseignementDto ToDto(
        UniteEnseignement ue,
        IReadOnlyCollection<SemestrePedagogique> semestreItems,
        IReadOnlyCollection<ElementConstitutif> ecItems)
    {
        var semestre = semestreItems.FirstOrDefault(x => x.Id == ue.SemestrePedagogiqueId);

        return new UniteEnseignementDto
        {
            Id = ue.Id,
            SemestrePedagogiqueId = ue.SemestrePedagogiqueId,
            SemestrePedagogiqueLibelle = semestre is null ? string.Empty : $"{semestre.Numero} - {semestre.Libelle}",
            Code = ue.Code,
            Libelle = ue.Libelle,
            Credits = ue.Credits,
            VolumeHoraire = ue.VolumeHoraire,
            OrdreAffichage = ue.OrdreAffichage,
            EstObligatoire = ue.EstObligatoire,
            NombreElementsConstitutifs = ecItems.Count(x => x.UniteEnseignementId == ue.Id)
        };
    }

    private static ElementConstitutifDto ToDto(
        ElementConstitutif ec,
        IReadOnlyCollection<UniteEnseignement> ueItems)
    {
        var ue = ueItems.FirstOrDefault(x => x.Id == ec.UniteEnseignementId);

        return new ElementConstitutifDto
        {
            Id = ec.Id,
            UniteEnseignementId = ec.UniteEnseignementId,
            UniteEnseignementLibelle = ue is null ? string.Empty : FormatCodeLibelle(ue.Code, ue.Libelle),
            Code = ec.Code,
            Libelle = ec.Libelle,
            Type = ec.Type,
            Credits = ec.Credits,
            Coefficient = ec.Coefficient,
            VolumeHoraire = ec.VolumeHoraire,
            OrdreAffichage = ec.OrdreAffichage,
            EstObligatoire = ec.EstObligatoire,
            Observation = ec.Observation
        };
    }

    private static string FormatCodeLibelle(string code, string libelle)
        => string.IsNullOrWhiteSpace(code) ? libelle : $"{code} - {libelle}";

    private static string NormalizeCode(string? value, string errorMessage)
        => RequireText(value, errorMessage).ToUpperInvariant();

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
