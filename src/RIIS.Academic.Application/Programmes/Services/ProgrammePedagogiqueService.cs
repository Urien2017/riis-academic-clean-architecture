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
    IRepository<MaquetteElementConstitutif> maquetteElementsConstitutifs,
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
        var ueItems = await unitesEnseignement.ListAsync(cancellationToken);
        var ecItems = await elementsConstitutifs.ListAsync(cancellationToken);
        var maquetteEcItems = await maquetteElementsConstitutifs.ListAsync(cancellationToken);

        maquetteItems = FilterMaquettes(maquetteItems, anneeItems, ouvertureItems, classeItems, anneeAcademiqueId, cycleFormationId, maquettePedagogiqueId);

        return maquetteItems
            .OrderBy(x => x.Libelle)
            .ThenBy(x => x.Version)
            .Select(maquette =>
            {
                var ouverture = ouvertureItems.FirstOrDefault(x => x.Id == maquette.ParcoursAcademiqueId);

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
                        .ThenBy(semestre => semestre.NumeroSemestre)
                        .Select(semestre =>
                        {
                            var maquetteEcsForSemestre = maquetteEcItems
                                .Where(mec => mec.SemestrePedagogiqueId == semestre.Id)
                                .ToList();

                            var ueGroups = maquetteEcsForSemestre
                                .GroupBy(mec => mec.ElementConstitutifId)
                                .Select(g => g.First())
                                .Select(mec =>
                                {
                                    var ec = ecItems.FirstOrDefault(e => e.Id == mec.ElementConstitutifId);
                                    var ue = ec is null ? null : ueItems.FirstOrDefault(u => u.Id == ec.UniteEnseignementId);
                                    return new { Ue = ue, Ec = ec, Mec = mec };
                                })
                                .Where(x => x.Ue is not null)
                                .GroupBy(x => x.Ue!.Id)
                                .ToList();

                            return new SemestrePedagogiqueHierarchyDto
                            {
                                Id = semestre.Id,
                                NumeroSemestre = semestre.NumeroSemestre,
                                Libelle = semestre.Libelle,
                                CreditsUE = semestre.CreditsUE,
                                VolumeHoraireUE = semestre.VolumeHoraireUE,
                                OrdreAffichage = semestre.OrdreAffichage,
                                UnitesEnseignement = ueGroups
                                    .Select(group =>
                                    {
                                        var first = group.First();
                                        var ue = first.Ue!;

                                        var ueMaquetteEcs = maquetteEcItems
                                            .Where(mec => mec.SemestrePedagogiqueId == semestre.Id)
                                            .Where(mec =>
                                            {
                                                var mecEc = ecItems.FirstOrDefault(e => e.Id == mec.ElementConstitutifId);
                                                return mecEc is not null && mecEc.UniteEnseignementId == ue.Id;
                                            })
                                            .ToList();

                                        return new UniteEnseignementHierarchyDto
                                        {
                                            Id = ue.Id,
                                            SemestrePedagogiqueId = semestre.Id,
                                            SemestrePedagogiqueLibelle = $"{semestre.NumeroSemestre} - {semestre.Libelle}",
                                            Code = ue.Code,
                                            Libelle = ue.Libelle,
                                            Credits = ueMaquetteEcs.Sum(m => m.Credits),
                                            VolumeHoraire = (short)ueMaquetteEcs.Sum(m => m.VolumeHoraire),
                                            OrdreAffichage = ueMaquetteEcs.Min(m => m.OrdreAffichage),
                                            EstObligatoire = ueMaquetteEcs.All(m => m.EstObligatoire),
                                            ElementsConstitutifs = ueMaquetteEcs
                                                .OrderBy(m => m.OrdreAffichage)
                                                .Select(m =>
                                                {
                                                    var mecEc = ecItems.FirstOrDefault(e => e.Id == m.ElementConstitutifId);
                                                    return new ElementConstitutifHierarchyDto
                                                    {
                                                        Id = m.ElementConstitutifId,
                                                        MaquetteElementConstitutifId = m.Id,
                                                        Code = mecEc?.Code,
                                                        Libelle = mecEc?.Libelle ?? string.Empty,
                                                        Type = mecEc?.Type ?? TypeElementConstitutif.CoursMagistraux,
                                                        Credits = m.Credits,
                                                        Coefficient = m.Coefficient,
                                                        VolumeHoraire = m.VolumeHoraire,
                                                        OrdreAffichage = m.OrdreAffichage,
                                                        EstObligatoire = m.EstObligatoire,
                                                        Observation = m.Observation
                                                    };
                                                })
                                                .ToList()
                                        };
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
            throw new InvalidOperationException("Le parcours de la maquette est obligatoire.");
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

        var existing = await maquettes.ListAsync(cancellationToken);
        var duplicate = existing.Any(x =>
            x.Id != dto.Id
            && x.ParcoursAcademiqueId == dto.ParcoursAcademiqueId
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
                ParcoursAcademiqueId = dto.ParcoursAcademiqueId,
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

            entity.ParcoursAcademiqueId = dto.ParcoursAcademiqueId;
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

        return semestreItems
            .Select(x => ToSemestreDto(x, maquetteItems))
            .OrderBy(x => x.MaquettePedagogiqueLibelle)
            .ThenBy(x => x.OrdreAffichage)
            .ThenBy(x => x.NumeroSemestre)
            .ToList();
    }

    public async Task<SemestrePedagogiqueDto?> GetSemestreAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await semestres.GetByIdAsync(id, cancellationToken);
        if (entity is null) return null;

        var maquetteItems = await maquettes.ListAsync(cancellationToken);

        return ToSemestreDto(entity, maquetteItems);
    }

    public async Task<SemestrePedagogiqueDto> CreateDefaultSemestreAsync(
        long? maquettePedagogiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var semestreItems = await semestres.ListAsync(cancellationToken);
        var filtered = maquettePedagogiqueId is null
            ? semestreItems
            : semestreItems.Where(x => x.MaquettePedagogiqueId == maquettePedagogiqueId).ToList();
        var nextNumero = filtered.Count == 0 ? (byte)1 : (byte)Math.Min(10, filtered.Max(x => x.NumeroSemestre) + 1);

        return new SemestrePedagogiqueDto
        {
            MaquettePedagogiqueId = maquettePedagogiqueId ?? 0,
            NumeroSemestre = nextNumero,
            Libelle = $"Semestre {nextNumero}",
            OrdreAffichage = nextNumero
        };
    }

    public async Task SaveSemestreAsync(SemestrePedagogiqueDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.MaquettePedagogiqueId <= 0)
        {
            throw new InvalidOperationException("La maquette du semestre est obligatoire.");
        }

        if (dto.NumeroSemestre is < 1 or > 10)
        {
            throw new InvalidOperationException("Le numéro du semestre doit être compris entre 1 et 10.");
        }

        dto.Libelle = RequireText(dto.Libelle, "Le libellé du semestre est obligatoire.");
        dto.OrdreAffichage = dto.OrdreAffichage == 0 ? dto.NumeroSemestre : dto.OrdreAffichage;

        await EnsureMaquetteExistsAsync(dto.MaquettePedagogiqueId, cancellationToken);

        var existing = await semestres.ListAsync(cancellationToken);
        var duplicate = existing.Any(x =>
            x.Id != dto.Id
            && x.MaquettePedagogiqueId == dto.MaquettePedagogiqueId
            && x.NumeroSemestre == dto.NumeroSemestre);

        if (duplicate)
        {
            throw new InvalidOperationException("Ce numéro de semestre existe déjà dans cette maquette.");
        }

        if (dto.Id == 0)
        {
            await semestres.AddAsync(new SemestrePedagogique
            {
                MaquettePedagogiqueId = dto.MaquettePedagogiqueId,
                NumeroSemestre = dto.NumeroSemestre,
                Libelle = dto.Libelle,
                OrdreAffichage = dto.OrdreAffichage
            }, cancellationToken);
        }
        else
        {
            var entity = await semestres.GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null) return;

            entity.MaquettePedagogiqueId = dto.MaquettePedagogiqueId;
            entity.NumeroSemestre = dto.NumeroSemestre;
            entity.Libelle = dto.Libelle;
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
        var ecItems = await elementsConstitutifs.ListAsync(cancellationToken);

        if (semestrePedagogiqueId is not null)
        {
            var maquetteEcItems = await maquetteElementsConstitutifs.ListAsync(cancellationToken);
            var ecIds = maquetteEcItems
                .Where(x => x.SemestrePedagogiqueId == semestrePedagogiqueId)
                .Select(x => x.ElementConstitutifId)
                .ToHashSet();
            var ueIds = ecItems
                .Where(x => ecIds.Contains(x.Id))
                .Select(x => x.UniteEnseignementId)
                .ToHashSet();
            ueItems = ueItems.Where(x => ueIds.Contains(x.Id)).ToList();
        }

        return ueItems
            .Select(x => ToUeDto(x, ecItems))
            .OrderBy(x => x.Code)
            .ToList();
    }

    public async Task<List<UniteEnseignementDto>> GetUnitesEnseignementByHierarchyAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? parcoursAcademiqueId = null,
        long? semestrePedagogiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var semestreItems = await GetFilteredSemestresAsync(
            anneeAcademiqueId, cycleFormationId, parcoursAcademiqueId, cancellationToken);

        if (semestrePedagogiqueId is not null)
        {
            semestreItems = semestreItems.Where(x => x.Id == semestrePedagogiqueId).ToList();
        }

        var semestreIds = semestreItems.Select(x => x.Id).ToHashSet();
        var maquetteEcItems = await maquetteElementsConstitutifs.ListAsync(cancellationToken);
        var ecIds = maquetteEcItems
            .Where(x => semestreIds.Contains(x.SemestrePedagogiqueId))
            .Select(x => x.ElementConstitutifId)
            .ToHashSet();
        var ecItems = await elementsConstitutifs.ListAsync(cancellationToken);
        var ueIds = ecItems
            .Where(x => ecIds.Contains(x.Id))
            .Select(x => x.UniteEnseignementId)
            .ToHashSet();
        var ueItems = await unitesEnseignement.ListAsync(cancellationToken);
        ueItems = ueItems.Where(x => ueIds.Contains(x.Id)).ToList();

        return ueItems
            .Select(x => ToUeDto(x, ecItems))
            .OrderBy(x => x.Code)
            .ToList();
    }

    public async Task<List<UniteEnseignementHierarchyDto>> GetUnitesEnseignementHierarchyAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? parcoursAcademiqueId = null,
        long? semestrePedagogiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var semestreItems = await GetFilteredSemestresAsync(
            anneeAcademiqueId, cycleFormationId, parcoursAcademiqueId, cancellationToken);

        if (semestrePedagogiqueId is not null)
        {
            semestreItems = semestreItems.Where(x => x.Id == semestrePedagogiqueId).ToList();
        }

        var semestreIds = semestreItems.Select(x => x.Id).ToHashSet();
        var maquetteEcItems = await maquetteElementsConstitutifs.ListAsync(cancellationToken);
        var ecItems = await elementsConstitutifs.ListAsync(cancellationToken);
        var ueItems = await unitesEnseignement.ListAsync(cancellationToken);

        var maquetteEcsFiltered = maquetteEcItems
            .Where(x => semestreIds.Contains(x.SemestrePedagogiqueId))
            .ToList();

        return maquetteEcsFiltered
            .GroupBy(mec => mec.SemestrePedagogiqueId)
            .SelectMany(semGroup =>
            {
                var semestre = semestreItems.FirstOrDefault(x => x.Id == semGroup.Key);
                if (semestre is null) return Enumerable.Empty<UniteEnseignementHierarchyDto>();

                return semGroup
                    .GroupBy(mec =>
                    {
                        var ec = ecItems.FirstOrDefault(e => e.Id == mec.ElementConstitutifId);
                        return ec?.UniteEnseignementId ?? 0;
                    })
                    .Where(ueGroup => ueGroup.Key > 0)
                    .Select(ueGroup =>
                    {
                        var ue = ueItems.FirstOrDefault(x => x.Id == ueGroup.Key);
                        if (ue is null) return null!;

                        return new UniteEnseignementHierarchyDto
                        {
                            Id = ue.Id,
                            SemestrePedagogiqueId = semestre.Id,
                            SemestrePedagogiqueLibelle = $"{semestre.NumeroSemestre} - {semestre.Libelle}",
                            Code = ue.Code,
                            Libelle = ue.Libelle,
                            Credits = ueGroup.Sum(m => m.Credits),
                            VolumeHoraire = (short)ueGroup.Sum(m => m.VolumeHoraire),
                            OrdreAffichage = ueGroup.Min(m => m.OrdreAffichage),
                            EstObligatoire = ueGroup.All(m => m.EstObligatoire),
                            ElementsConstitutifs = ueGroup
                                .OrderBy(m => m.OrdreAffichage)
                                .Select(m =>
                                {
                                    var mecEc = ecItems.FirstOrDefault(e => e.Id == m.ElementConstitutifId);
                                    return new ElementConstitutifHierarchyDto
                                    {
                                        Id = m.ElementConstitutifId,
                                        MaquetteElementConstitutifId = m.Id,
                                        Code = mecEc?.Code,
                                        Libelle = mecEc?.Libelle ?? string.Empty,
                                        Type = mecEc?.Type ?? TypeElementConstitutif.CoursMagistraux,
                                        Credits = m.Credits,
                                        Coefficient = m.Coefficient,
                                        VolumeHoraire = m.VolumeHoraire,
                                        OrdreAffichage = m.OrdreAffichage,
                                        EstObligatoire = m.EstObligatoire,
                                        Observation = m.Observation
                                    };
                                })
                                .ToList()
                        };
                    })
                    .Where(x => x is not null);
            })
            .ToList();
    }

    public async Task<UniteEnseignementDto?> GetUniteEnseignementAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await unitesEnseignement.GetByIdAsync(id, cancellationToken);
        if (entity is null) return null;

        var ecItems = await elementsConstitutifs.ListAsync(cancellationToken);

        return ToUeDto(entity, ecItems);
    }

    public Task<UniteEnseignementDto> CreateDefaultUniteEnseignementAsync(
        long? semestrePedagogiqueId = null,
        CancellationToken cancellationToken = default)
        => Task.FromResult(new UniteEnseignementDto
        {
            Code = "UE",
        });

    public async Task SaveUniteEnseignementAsync(UniteEnseignementDto dto, CancellationToken cancellationToken = default)
    {
        dto.Code = NormalizeCode(dto.Code, "Le code de l'UE est obligatoire.");
        dto.Libelle = RequireText(dto.Libelle, "Le libellé de l'UE est obligatoire.");

        var existing = await unitesEnseignement.ListAsync(cancellationToken);
        var duplicate = existing.Any(x =>
            x.Id != dto.Id
            && string.Equals(x.Code, dto.Code, StringComparison.OrdinalIgnoreCase));

        if (duplicate)
        {
            throw new InvalidOperationException("Une UE avec le même code existe déjà.");
        }

        if (dto.Id == 0)
        {
            await unitesEnseignement.AddAsync(new UniteEnseignement
            {
                Code = dto.Code,
                Libelle = dto.Libelle
            }, cancellationToken);
        }
        else
        {
            var entity = await unitesEnseignement.GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null) return;

            entity.Code = dto.Code;
            entity.Libelle = dto.Libelle;
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
            .Select(x => ToEcDto(x, ueItems))
            .OrderBy(x => x.UniteEnseignementLibelle)
            .ThenBy(x => x.Libelle)
            .ToList();
    }

    public async Task<ElementConstitutifDto?> GetElementConstitutifAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await elementsConstitutifs.GetByIdAsync(id, cancellationToken);
        if (entity is null) return null;

        var ueItems = await unitesEnseignement.ListAsync(cancellationToken);

        return ToEcDto(entity, ueItems);
    }

    public Task<ElementConstitutifDto> CreateDefaultElementConstitutifAsync(
        long? uniteEnseignementId = null,
        CancellationToken cancellationToken = default)
        => Task.FromResult(new ElementConstitutifDto
        {
            UniteEnseignementId = uniteEnseignementId ?? 0,
            Type = TypeElementConstitutif.CoursMagistraux
        });

    public async Task SaveElementConstitutifAsync(ElementConstitutifDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.UniteEnseignementId <= 0)
        {
            throw new InvalidOperationException("L'UE de l'EC est obligatoire.");
        }

        dto.Code = NormalizeNullable(dto.Code)?.ToUpperInvariant();
        dto.Libelle = RequireText(dto.Libelle, "Le libellé de l'EC est obligatoire.");

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

        if (dto.Id == 0)
        {
            await elementsConstitutifs.AddAsync(new ElementConstitutif
            {
                UniteEnseignementId = dto.UniteEnseignementId,
                Code = dto.Code,
                Libelle = dto.Libelle,
                Type = dto.Type
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
            .ThenBy(x => x.NumeroSemestre)
            .Select(x => new LookupDto { Id = x.Id, Libelle = $"{x.NumeroSemestre} - {x.Libelle}" })
            .ToList();
    }

    public async Task<List<LookupDto>> GetSemestresHierarchyLookupAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? parcoursAcademiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var items = await GetFilteredSemestresAsync(
            anneeAcademiqueId, cycleFormationId, parcoursAcademiqueId, cancellationToken);

        return items
            .OrderBy(x => x.OrdreAffichage)
            .ThenBy(x => x.NumeroSemestre)
            .Select(x => new LookupDto { Id = x.Id, Libelle = $"{x.NumeroSemestre} - {x.Libelle}" })
            .ToList();
    }

    public async Task<List<LookupDto>> GetUnitesEnseignementLookupAsync(
        long? semestrePedagogiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var items = await unitesEnseignement.ListAsync(cancellationToken);

        if (semestrePedagogiqueId is not null)
        {
            var maquetteEcItems = await maquetteElementsConstitutifs.ListAsync(cancellationToken);
            var ecItems = await elementsConstitutifs.ListAsync(cancellationToken);
            var ecIds = maquetteEcItems
                .Where(x => x.SemestrePedagogiqueId == semestrePedagogiqueId)
                .Select(x => x.ElementConstitutifId)
                .ToHashSet();
            var ueIds = ecItems
                .Where(x => ecIds.Contains(x.Id))
                .Select(x => x.UniteEnseignementId)
                .ToHashSet();
            items = items.Where(x => ueIds.Contains(x.Id)).ToList();
        }

        return items
            .OrderBy(x => x.Code)
            .Select(x => new LookupDto { Id = x.Id, Libelle = FormatCodeLibelle(x.Code, x.Libelle) })
            .ToList();
    }

    // --- Private helpers ---

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
            maquetteItems, anneeItems, ouvertureItems, classeItems,
            anneeAcademiqueId, cycleFormationId, null);

        if (parcoursAcademiqueId is not null)
        {
            var ouverture = ouvertureItems.FirstOrDefault(x => x.Id == parcoursAcademiqueId);
            maquetteItems = maquetteItems
                .Where(x => ouverture is not null && x.ParcoursAcademiqueId == ouverture.Id)
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

    private async Task EnsureUniteEnseignementExistsAsync(long id, CancellationToken cancellationToken)
    {
        var exists = (await unitesEnseignement.ListAsync(cancellationToken)).Any(x => x.Id == id);
        if (!exists) throw new InvalidOperationException("L'UE sélectionnée est introuvable.");
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
            var parcoursIdsPourAnnee = ouvertureItems
                .Where(x => x.AnneeAcademiqueId == anneeAcademiqueId)
                .Select(x => x.Id)
                .ToHashSet();

            var maquetteIdsPourAnnee = maquetteItems
                .Where(x => parcoursIdsPourAnnee.Contains(x.ParcoursAcademiqueId))
                .Select(x => x.Id)
                .ToHashSet();

            query = query.Where(x =>
                maquetteIdsPourAnnee.Contains(x.Id)
                || (annee is not null && IsMaquetteValidForAcademicYear(x, annee)));
        }

        if (cycleFormationId is not null)
        {
            var ouvertureIds = ouvertureItems
                .Where(x => x.CycleFormationId == cycleFormationId)
                .Select(x => x.Id)
                .ToHashSet();

            query = query.Where(x => ouvertureIds.Contains(x.ParcoursAcademiqueId));
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

    private static MaquettePedagogiqueDto ToDto(
        MaquettePedagogique maquette,
        IReadOnlyCollection<ParcoursAcademique> ouvertures,
        IReadOnlyCollection<SemestrePedagogique> semestreItems)
    {
        var ouverture = ouvertures.FirstOrDefault(x => x.Id == maquette.ParcoursAcademiqueId);

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

    private static SemestrePedagogiqueDto ToSemestreDto(
        SemestrePedagogique semestre,
        IReadOnlyCollection<MaquettePedagogique> maquetteItems)
    {
        var maquette = maquetteItems.FirstOrDefault(x => x.Id == semestre.MaquettePedagogiqueId);

        return new SemestrePedagogiqueDto
        {
            Id = semestre.Id,
            MaquettePedagogiqueId = semestre.MaquettePedagogiqueId,
            MaquettePedagogiqueLibelle = maquette is null ? string.Empty : $"{maquette.Code} - {maquette.Libelle} ({maquette.Version})",
            NumeroSemestre = semestre.NumeroSemestre,
            Libelle = semestre.Libelle,
            CreditsUE = semestre.CreditsUE,
            VolumeHoraireUE = semestre.VolumeHoraireUE,
            OrdreAffichage = semestre.OrdreAffichage
        };
    }

    private static UniteEnseignementDto ToUeDto(
        UniteEnseignement ue,
        IReadOnlyCollection<ElementConstitutif> ecItems)
        => new()
        {
            Id = ue.Id,
            Code = ue.Code,
            Libelle = ue.Libelle,
            NombreElementsConstitutifs = ecItems.Count(x => x.UniteEnseignementId == ue.Id)
        };

    private static ElementConstitutifDto ToEcDto(
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
            Type = ec.Type
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
