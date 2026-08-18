using RIIS.Academic.Application.Abstractions.Persistence;
using RIIS.Academic.Application.Dashboard.Dtos;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Dashboard.Services;

public class DashboardAcademiqueService(
    IRepository<AnneeAcademique> anneesAcademiques,
    IRepository<CycleFormation> cyclesFormation,
    IRepository<ParcoursAcademique> parcoursAcademiques,
    IRepository<Filiere> filieres,
    IRepository<Specialite> specialites,
    IRepository<NiveauEtude> niveauxEtude,
    IRepository<ClassePedagogique> classesPedagogiques,
    IRepository<Inscription> inscriptions,
    IRepository<MaquettePedagogique> maquettesPedagogiques,
    IRepository<SemestrePedagogique> semestresPedagogiques,
    IRepository<UniteEnseignement> unitesEnseignement,
    IRepository<ElementConstitutif> elementsConstitutifs,
    IRepository<MaquetteElementConstitutif> maquetteElementsConstitutifs,
    IRepository<EvaluationAcademique> evaluationsAcademiques,
    IRepository<NoteEvaluation> notesEvaluations,
    IRepository<ResultatElementConstitutif> resultatsElementsConstitutifs,
    IRepository<ResultatSemestre> resultatsSemestres,
    IRepository<ResultatAnnuel> resultatsAnnuels,
    IRepository<ProcesVerbal> procesVerbaux) : IDashboardAcademiqueService
{
    public async Task<DashboardAcademiqueDto> GetDashboardAcademiqueAsync(
        DashboardAcademiqueFilterDto? filter = null,
        CancellationToken cancellationToken = default)
    {
        filter ??= new DashboardAcademiqueFilterDto();

        var data = await LoadDataAsync(cancellationToken);
        var filteredInscriptions = FilterInscriptions(data, filter);
        var filteredClasses = FilterClasses(data, filter, filteredInscriptions);
        var filteredEvaluations = FilterEvaluations(data, filter, filteredInscriptions);
        var filteredNotes = data.Notes
            .Where(x => filteredInscriptions.Any(inscription => inscription.Id == x.InscriptionId))
            .Where(x => filteredEvaluations.Any(evaluation => evaluation.Id == x.EvaluationAcademiqueId))
            .ToList();

        var dashboard = new DashboardAcademiqueDto
        {
            Filtres = filter,
            Kpi = BuildKpi(data, filteredInscriptions, filteredClasses, filteredEvaluations, filteredNotes),
            InscriptionsParPeriode = BuildInscriptionsParPeriode(filteredInscriptions),
            ProgressionNotesParType = BuildProgressionNotesParType(filteredEvaluations, filteredNotes, filteredInscriptions, data),
            EvolutionSaisieNotes = BuildEvolutionSaisieNotes(filteredEvaluations, filteredNotes, filteredInscriptions, data),
            RepartitionResultats = BuildRepartitionResultats(data, filteredInscriptions),
            TauxReussiteParCycle = BuildTauxReussiteParGroupe(data, filteredInscriptions, GroupeDashboard.Cycle),
            TauxReussiteParNiveau = BuildTauxReussiteParGroupe(data, filteredInscriptions, GroupeDashboard.Niveau),
            TauxReussiteParParcours = BuildTauxReussiteParGroupe(data, filteredInscriptions, GroupeDashboard.Parcours),
            ElementsConstitutifsARisque = BuildElementsConstitutifsARisque(data, filteredInscriptions, filter),
            Classes = BuildClassesSynthese(data, filteredClasses, filteredInscriptions)
        };

        return dashboard;
    }

    private async Task<DashboardData> LoadDataAsync(CancellationToken cancellationToken)
        => new(
            await anneesAcademiques.ListAsync(cancellationToken),
            await cyclesFormation.ListAsync(cancellationToken),
            await parcoursAcademiques.ListAsync(cancellationToken),
            await filieres.ListAsync(cancellationToken),
            await specialites.ListAsync(cancellationToken),
            await niveauxEtude.ListAsync(cancellationToken),
            await classesPedagogiques.ListAsync(cancellationToken),
            await inscriptions.ListAsync(cancellationToken),
            await maquettesPedagogiques.ListAsync(cancellationToken),
            await semestresPedagogiques.ListAsync(cancellationToken),
            await unitesEnseignement.ListAsync(cancellationToken),
            await elementsConstitutifs.ListAsync(cancellationToken),
            await maquetteElementsConstitutifs.ListAsync(cancellationToken),
            await evaluationsAcademiques.ListAsync(cancellationToken),
            await notesEvaluations.ListAsync(cancellationToken),
            await resultatsElementsConstitutifs.ListAsync(cancellationToken),
            await resultatsSemestres.ListAsync(cancellationToken),
            await resultatsAnnuels.ListAsync(cancellationToken),
            await procesVerbaux.ListAsync(cancellationToken));

    private static List<Inscription> FilterInscriptions(DashboardData data, DashboardAcademiqueFilterDto filter)
    {
        var query = data.Inscriptions.AsEnumerable();

        if (filter.AnneeAcademiqueId is not null)
        {
            query = query.Where(x => x.AnneeAcademiqueId == filter.AnneeAcademiqueId);
        }

        if (filter.CycleFormationId is not null)
        {
            var ouvertureIds = data.Ouvertures
                .Where(x => x.CycleFormationId == filter.CycleFormationId)
                .Select(x => x.Id)
                .ToHashSet();

            query = query.Where(x => ouvertureIds.Contains(x.ParcoursAcademiqueId));
        }

        if (filter.NiveauEtudeId is not null)
        {
            query = query.Where(x => x.NiveauEtudeId == filter.NiveauEtudeId);
        }

        if (filter.FiliereId is not null)
        {
            var ouvertureIds = data.Ouvertures
                .Where(x => x.FiliereId == filter.FiliereId)
                .Select(x => x.Id)
                .ToHashSet();

            query = query.Where(x => ouvertureIds.Contains(x.ParcoursAcademiqueId));
        }

        if (filter.SpecialiteId is not null)
        {
            var ouvertureIds = data.Ouvertures
                .Where(x => x.SpecialiteId == filter.SpecialiteId)
                .Select(x => x.Id)
                .ToHashSet();

            query = query.Where(x => ouvertureIds.Contains(x.ParcoursAcademiqueId));
        }

        if (filter.ClassePedagogiqueId is not null)
        {
            query = query.Where(x => x.ClassePedagogiqueId == filter.ClassePedagogiqueId);
        }

        if (filter.MaquettePedagogiqueId is not null)
        {
            query = query.Where(x => x.MaquettePedagogiqueId == filter.MaquettePedagogiqueId);
        }

        if (filter.SemestrePedagogiqueId is not null)
        {
            var semestre = data.Semestres.FirstOrDefault(x => x.Id == filter.SemestrePedagogiqueId);
            if (semestre is not null)
            {
                query = query.Where(x => x.MaquettePedagogiqueId == semestre.MaquettePedagogiqueId);
            }
        }

        return query.ToList();
    }

    private static List<ClassePedagogique> FilterClasses(
        DashboardData data,
        DashboardAcademiqueFilterDto filter,
        IReadOnlyCollection<Inscription> filteredInscriptions)
    {
        var inscriptionClasseIds = filteredInscriptions
            .Where(x => x.ClassePedagogiqueId is not null)
            .Select(x => x.ClassePedagogiqueId!.Value)
            .ToHashSet();

        var query = data.Classes.Where(x => inscriptionClasseIds.Contains(x.Id));

        if (filter.ClassePedagogiqueId is not null)
        {
            query = query.Where(x => x.Id == filter.ClassePedagogiqueId);
        }

        return query.ToList();
    }

    private static List<EvaluationAcademique> FilterEvaluations(
        DashboardData data,
        DashboardAcademiqueFilterDto filter,
        IReadOnlyCollection<Inscription> filteredInscriptions)
    {
        var anneeIds = filteredInscriptions.Select(x => x.AnneeAcademiqueId).ToHashSet();
        var maquetteIds = filteredInscriptions
            .Where(x => x.MaquettePedagogiqueId is not null)
            .Select(x => x.MaquettePedagogiqueId!.Value)
            .ToHashSet();
        var ouvertureIds = filteredInscriptions.Select(x => x.ParcoursAcademiqueId).ToHashSet();

        var query = data.Evaluations
            .Where(x => anneeIds.Contains(x.AnneeAcademiqueId))
            .Where(evaluation =>
            {
                var semestre = ResolveSemestre(data, evaluation.MaquetteElementConstitutifId);
                if (semestre is null)
                {
                    return false;
                }

                var maquette = data.Maquettes.FirstOrDefault(x => x.Id == semestre.MaquettePedagogiqueId);

                return maquette is not null
                    && (maquetteIds.Contains(maquette.Id)
                        || ouvertureIds.Any(ouvertureId => IsMaquetteCompatibleWithParcours(data, maquette, ouvertureId)));
            });

        if (filter.SemestrePedagogiqueId is not null)
        {
            query = query.Where(x => ResolveSemestre(data, x.MaquetteElementConstitutifId)?.Id == filter.SemestrePedagogiqueId);
        }

        if (!string.IsNullOrWhiteSpace(filter.CodeSession))
        {
            query = query.Where(x => string.Equals(x.Code, filter.CodeSession, StringComparison.OrdinalIgnoreCase));
        }

        return query.ToList();
    }

    private static DashboardKpiDto BuildKpi(
        DashboardData data,
        IReadOnlyCollection<Inscription> inscriptions,
        IReadOnlyCollection<ClassePedagogique> classes,
        IReadOnlyCollection<EvaluationAcademique> evaluations,
        IReadOnlyCollection<NoteEvaluation> notes)
    {
        var resultats = data.ResultatsAnnuels
            .Where(x => inscriptions.Any(inscription => inscription.Id == x.InscriptionId))
            .ToList();

        var nombreAdmis = resultats.Count(x => IsAdmis(x.DecisionJury, x.StatutValidation));
        var nombreRattrapage = resultats.Count(x => IsRattrapage(x.DecisionJury, x.StatutValidation));
        var nombreEchec = resultats.Count(x => IsEchec(x.DecisionJury, x.StatutValidation));
        var nombreNonCalcules = Math.Max(0, inscriptions.Count - nombreAdmis - nombreRattrapage - nombreEchec);
        var notesAttendues = CountNotesAttendues(data, evaluations, inscriptions);
        var notesSaisies = notes.Count(x => x.Valeur is not null);

        return new DashboardKpiDto
        {
            NombreInscrits = inscriptions.Count,
            NombreClasses = classes.Count,
            NombreEvaluations = evaluations.Count,
            NombreNotesAttendues = notesAttendues,
            NombreNotesSaisies = notesSaisies,
            TauxNotesSaisies = Percent(notesSaisies, notesAttendues),
            NombreAdmis = nombreAdmis,
            NombreRattrapage = nombreRattrapage,
            NombreEchec = nombreEchec,
            NombreNonCalcules = nombreNonCalcules,
            TauxReussite = Percent(nombreAdmis, inscriptions.Count),
            TauxRattrapage = Percent(nombreRattrapage, inscriptions.Count),
            MoyenneGenerale = Average(resultats.Select(x => x.MoyenneAnnuelle)),
            CreditsMoyensCapitalises = resultats.Count == 0 ? 0 : Math.Round(resultats.Average(x => x.CreditsAcquis), 2),
            NombreProcesVerbauxGeneres = data.ProcesVerbaux.Count(pv => classes.Any(classe => classe.Id == pv.ClassePedagogiqueId)),
            NombreRelevesDisponibles = resultats.Select(x => x.InscriptionId).Distinct().Count()
        };
    }

    private static List<DashboardInscriptionEvolutionDto> BuildInscriptionsParPeriode(IReadOnlyCollection<Inscription> inscriptions)
        => inscriptions
            .GroupBy(x => new DateOnly(x.DateInscription.Year, x.DateInscription.Month, 1))
            .OrderBy(x => x.Key)
            .Select(x => new DashboardInscriptionEvolutionDto
            {
                Periode = x.Key.ToString("MM/yyyy"),
                Ordre = x.Key.Year * 100 + x.Key.Month,
                NombreInscriptions = x.Count()
            })
            .ToList();

    private static List<DashboardEvaluationCompletionDto> BuildProgressionNotesParType(
        IReadOnlyCollection<EvaluationAcademique> evaluations,
        IReadOnlyCollection<NoteEvaluation> notes,
        IReadOnlyCollection<Inscription> inscriptions,
        DashboardData data)
        => Enum.GetValues<TypeEvaluation>()
            .Select(type =>
            {
                var evaluationsType = evaluations.Where(x => x.Type == type).ToList();
                var notesType = notes
                    .Where(note => evaluationsType.Any(evaluation => evaluation.Id == note.EvaluationAcademiqueId))
                    .ToList();
                var attendues = CountNotesAttendues(data, evaluationsType, inscriptions);
                var saisies = notesType.Count(x => x.Valeur is not null);

                return new DashboardEvaluationCompletionDto
                {
                    CodeTypeEvaluation = type.ToString(),
                    LibelleTypeEvaluation = FormatTypeEvaluation(type),
                    NombreEvaluations = evaluationsType.Count,
                    NombreNotesAttendues = attendues,
                    NombreNotesSaisies = saisies,
                    TauxCompletion = Percent(saisies, attendues)
                };
            })
            .ToList();

    private static List<DashboardEvaluationCompletionParSemestreDto> BuildEvolutionSaisieNotes(
        IReadOnlyCollection<EvaluationAcademique> evaluations,
        IReadOnlyCollection<NoteEvaluation> notes,
        IReadOnlyCollection<Inscription> inscriptions,
        DashboardData data)
    {
        var semestreIds = evaluations
            .Select(x => ResolveSemestre(data, x.MaquetteElementConstitutifId))
            .Where(x => x is not null)
            .Select(x => x!)
            .DistinctBy(x => x.Id)
            .OrderBy(x => x.NumeroSemestre)
            .ToList();

        return semestreIds
            .Select(semestre => new DashboardEvaluationCompletionParSemestreDto
            {
                SemestrePedagogiqueId = semestre.Id,
                SemestreNumero = semestre.NumeroSemestre,
                Periode = $"S{semestre.NumeroSemestre}",
                TauxCcon = CompletionForSemestreAndType(data, evaluations, notes, inscriptions, semestre.Id, TypeEvaluation.ControleContinu),
                TauxCc = CompletionForSemestreAndType(data, evaluations, notes, inscriptions, semestre.Id, TypeEvaluation.ControleConnaissance),
                TauxSn = CompletionForSemestreAndType(data, evaluations, notes, inscriptions, semestre.Id, TypeEvaluation.SessionNormale),
                TauxSr = CompletionForSemestreAndType(data, evaluations, notes, inscriptions, semestre.Id, TypeEvaluation.SessionRattrapage)
            })
            .ToList();
    }

    private static List<DashboardResultatRepartitionDto> BuildRepartitionResultats(
        DashboardData data,
        IReadOnlyCollection<Inscription> inscriptions)
    {
        var resultats = data.ResultatsAnnuels
            .Where(x => inscriptions.Any(inscription => inscription.Id == x.InscriptionId))
            .ToList();

        var nombreAdmis = resultats.Count(x => IsAdmis(x.DecisionJury, x.StatutValidation));
        var nombreRattrapage = resultats.Count(x => IsRattrapage(x.DecisionJury, x.StatutValidation));
        var nombreEchec = resultats.Count(x => IsEchec(x.DecisionJury, x.StatutValidation));
        var nombreNonCalcules = Math.Max(0, inscriptions.Count - nombreAdmis - nombreRattrapage - nombreEchec);

        return
        [
            new() { Code = "ADMIS", Libelle = "Admis", Nombre = nombreAdmis, Taux = Percent(nombreAdmis, inscriptions.Count), Couleur = "#28a745" },
            new() { Code = "RATTRAPAGE", Libelle = "Rattrapage", Nombre = nombreRattrapage, Taux = Percent(nombreRattrapage, inscriptions.Count), Couleur = "#ffc107" },
            new() { Code = "ECHEC", Libelle = "Échec", Nombre = nombreEchec, Taux = Percent(nombreEchec, inscriptions.Count), Couleur = "#dc3545" },
            new() { Code = "NON_CALCULE", Libelle = "Non calculé", Nombre = nombreNonCalcules, Taux = Percent(nombreNonCalcules, inscriptions.Count), Couleur = "#6c757d" }
        ];
    }

    private static List<DashboardTauxReussiteParGroupeDto> BuildTauxReussiteParGroupe(
        DashboardData data,
        IReadOnlyCollection<Inscription> inscriptions,
        GroupeDashboard groupe)
        => inscriptions
            .GroupBy(x => BuildGroupeKey(data, x, groupe))
            .Where(x => !string.IsNullOrWhiteSpace(x.Key.Libelle))
            .Select(group =>
            {
                var inscriptionIds = group.Select(x => x.Id).ToHashSet();
                var resultats = data.ResultatsAnnuels.Where(x => inscriptionIds.Contains(x.InscriptionId)).ToList();
                var nombreAdmis = resultats.Count(x => IsAdmis(x.DecisionJury, x.StatutValidation));
                var nombreRattrapage = resultats.Count(x => IsRattrapage(x.DecisionJury, x.StatutValidation));
                var nombreEchec = resultats.Count(x => IsEchec(x.DecisionJury, x.StatutValidation));

                return new DashboardTauxReussiteParGroupeDto
                {
                    TypeGroupe = groupe.ToString(),
                    GroupeId = group.Key.Id,
                    Code = group.Key.Code,
                    Libelle = group.Key.Libelle,
                    NombreInscrits = group.Count(),
                    NombreAdmis = nombreAdmis,
                    NombreRattrapage = nombreRattrapage,
                    NombreEchec = nombreEchec,
                    TauxReussite = Percent(nombreAdmis, group.Count())
                };
            })
            .OrderByDescending(x => x.TauxReussite)
            .ThenBy(x => x.Libelle)
            .ToList();

    private static List<DashboardEcRisqueDto> BuildElementsConstitutifsARisque(
        DashboardData data,
        IReadOnlyCollection<Inscription> inscriptions,
        DashboardAcademiqueFilterDto filter)
    {
        var inscriptionIds = inscriptions.Select(x => x.Id).ToHashSet();
        var resultats = data.ResultatsElementsConstitutifs
            .Where(x => inscriptionIds.Contains(x.InscriptionId))
            .Where(x => filter.SemestrePedagogiqueId is null
                || ResolveSemestre(data, x.MaquetteElementConstitutifId)?.Id == filter.SemestrePedagogiqueId)
            .GroupBy(x => x.MaquetteElementConstitutifId)
            .Select(group =>
            {
                var maquetteEc = data.MaquetteElementsConstitutifs.FirstOrDefault(x => x.Id == group.Key);
                var ec = maquetteEc is null ? null : data.ElementsConstitutifs.FirstOrDefault(x => x.Id == maquetteEc.ElementConstitutifId);
                var ue = ec is null ? null : data.UnitesEnseignement.FirstOrDefault(x => x.Id == ec.UniteEnseignementId);
                var notes = group.Select(x => x.MoyenneRetenue).Where(x => x is not null).Select(x => x!.Value).ToList();
                var nombreEchecs = group.Count(x => x.MoyenneRetenue is < 10m || x.StatutValidation == StatutValidationAcademique.NonValide);

                return new DashboardEcRisqueDto
                {
                    ElementConstitutifId = group.Key,
                    ElementConstitutifCode = ec?.Code ?? string.Empty,
                    ElementConstitutifLibelle = ec?.Libelle ?? string.Empty,
                    UniteEnseignementId = ue?.Id,
                    UniteEnseignementLibelle = ue?.Libelle ?? string.Empty,
                    ClassePedagogiqueLibelle = string.Empty,
                    MoyenneClasse = notes.Count == 0 ? 0 : Math.Round(notes.Average(), 2),
                    NombreNotes = group.Count(),
                    NombreEchecs = nombreEchecs,
                    TauxEchec = Percent(nombreEchecs, group.Count())
                };
            })
            .Where(x => x.NombreNotes > 0)
            .OrderByDescending(x => x.TauxEchec)
            .ThenBy(x => x.ElementConstitutifLibelle)
            .Take(10)
            .ToList();

        return resultats;
    }

    private static List<DashboardClasseSyntheseDto> BuildClassesSynthese(
        DashboardData data,
        IReadOnlyCollection<ClassePedagogique> classes,
        IReadOnlyCollection<Inscription> inscriptions)
        => classes
            .OrderBy(x => x.Code)
            .Select(classe =>
            {
                var classeInscriptions = inscriptions.Where(x => x.ClassePedagogiqueId == classe.Id).ToList();
                var inscriptionIds = classeInscriptions.Select(x => x.Id).ToHashSet();
                var resultats = data.ResultatsAnnuels.Where(x => inscriptionIds.Contains(x.InscriptionId)).ToList();
                var ouverture = data.Ouvertures.FirstOrDefault(x => x.Id == classe.ParcoursAcademiqueId);
                var cycle = ouverture is null ? null : data.Cycles.FirstOrDefault(x => x.Id == ouverture.CycleFormationId);
                var filiere = ouverture is null ? null : data.Filieres.FirstOrDefault(x => x.Id == ouverture.FiliereId);
                var specialite = ouverture is null ? null : data.Specialites.FirstOrDefault(x => x.Id == ouverture.SpecialiteId);
                var niveau = ouverture is null ? null : data.Niveaux.FirstOrDefault(x => x.Id == ouverture.NiveauEtudeId);
                var annee = data.Annees.FirstOrDefault(x => x.Id == classe.AnneeAcademiqueId);
                var evaluations = ResolveEvaluationsForClasse(data, classe, classeInscriptions);
                var notes = data.Notes
                    .Where(x => inscriptionIds.Contains(x.InscriptionId))
                    .Where(x => evaluations.Any(evaluation => evaluation.Id == x.EvaluationAcademiqueId))
                    .ToList();
                var attendues = CountNotesAttendues(data, evaluations, classeInscriptions);
                var saisies = notes.Count(x => x.Valeur is not null);

                return new DashboardClasseSyntheseDto
                {
                    ClassePedagogiqueId = classe.Id,
                    ClassePedagogiqueCode = classe.Code,
                    ClassePedagogiqueLibelle = classe.Libelle,
                    AnneeAcademiqueId = classe.AnneeAcademiqueId,
                    AnneeAcademiqueLibelle = annee?.Libelle ?? string.Empty,
                    CycleFormationId = ouverture?.CycleFormationId,
                    CycleFormationCode = cycle?.Code ?? string.Empty,
                    CycleFormationLibelle = cycle?.Libelle ?? string.Empty,
                    NiveauEtudeId = ouverture?.NiveauEtudeId ?? 0,
                    NiveauEtudeLibelle = niveau?.Libelle ?? string.Empty,
                    FiliereId = ouverture?.FiliereId,
                    FiliereLibelle = filiere?.Libelle ?? string.Empty,
                    SpecialiteId = ouverture?.SpecialiteId,
                    SpecialiteLibelle = specialite?.Libelle,
                    ParcoursLibelle = ouverture?.Libelle ?? string.Empty,
                    NombreInscrits = classeInscriptions.Count,
                    NombreNotesAttendues = attendues,
                    NombreNotesSaisies = saisies,
                    TauxNotesSaisies = Percent(saisies, attendues),
                    MoyenneClasse = Average(resultats.Select(x => x.MoyenneAnnuelle)),
                    CreditsMoyensCapitalises = resultats.Count == 0 ? 0 : Math.Round(resultats.Average(x => x.CreditsAcquis), 2),
                    NombreAdmis = resultats.Count(x => IsAdmis(x.DecisionJury, x.StatutValidation)),
                    NombreRattrapage = resultats.Count(x => IsRattrapage(x.DecisionJury, x.StatutValidation)),
                    NombreEchec = resultats.Count(x => IsEchec(x.DecisionJury, x.StatutValidation)),
                    NombreNonCalcules = Math.Max(0, classeInscriptions.Count - resultats.Count),
                    NombreProcesVerbauxGeneres = data.ProcesVerbaux.Count(x => x.ClassePedagogiqueId == classe.Id),
                    NombreRelevesDisponibles = resultats.Select(x => x.InscriptionId).Distinct().Count()
                };
            })
            .ToList();

    private static List<EvaluationAcademique> ResolveEvaluationsForClasse(
        DashboardData data,
        ClassePedagogique classe,
        IReadOnlyCollection<Inscription> classeInscriptions)
    {
        var maquetteIds = classeInscriptions
            .Where(x => x.MaquettePedagogiqueId is not null)
            .Select(x => x.MaquettePedagogiqueId!.Value)
            .ToHashSet();

        return data.Evaluations
            .Where(x => x.AnneeAcademiqueId == classe.AnneeAcademiqueId)
            .Where(evaluation =>
            {
                var semestre = ResolveSemestre(data, evaluation.MaquetteElementConstitutifId);
                if (semestre is null)
                {
                    return false;
                }

                var maquette = data.Maquettes.FirstOrDefault(x => x.Id == semestre.MaquettePedagogiqueId);

                return maquette is not null
                    && (maquetteIds.Contains(maquette.Id)
                        || IsMaquetteCompatibleWithParcours(data, maquette, classe.ParcoursAcademiqueId));
            })
            .ToList();
    }

    private static int CountNotesAttendues(
        DashboardData data,
        IReadOnlyCollection<EvaluationAcademique> evaluations,
        IReadOnlyCollection<Inscription> inscriptions)
        => evaluations.Sum(evaluation =>
        {
            var semestre = ResolveSemestre(data, evaluation.MaquetteElementConstitutifId);
            if (semestre is null)
            {
                return 0;
            }

            var maquette = data.Maquettes.FirstOrDefault(x => x.Id == semestre.MaquettePedagogiqueId);
            if (maquette is null)
            {
                return 0;
            }

            return inscriptions.Count(inscription =>
                inscription.AnneeAcademiqueId == evaluation.AnneeAcademiqueId
                && (inscription.MaquettePedagogiqueId == maquette.Id
                    || IsMaquetteCompatibleWithParcours(data, maquette, inscription.ParcoursAcademiqueId)));
        });

    private static decimal CompletionForSemestreAndType(
        DashboardData data,
        IReadOnlyCollection<EvaluationAcademique> evaluations,
        IReadOnlyCollection<NoteEvaluation> notes,
        IReadOnlyCollection<Inscription> inscriptions,
        long semestrePedagogiqueId,
        TypeEvaluation type)
    {
        var evaluationsFiltrees = evaluations
            .Where(x => x.Type == type)
            .Where(x => ResolveSemestre(data, x.MaquetteElementConstitutifId)?.Id == semestrePedagogiqueId)
            .ToList();
        var evaluationIds = evaluationsFiltrees.Select(x => x.Id).ToHashSet();
        var saisies = notes.Count(x => evaluationIds.Contains(x.EvaluationAcademiqueId) && x.Valeur is not null);
        var attendues = CountNotesAttendues(data, evaluationsFiltrees, inscriptions);

        return Percent(saisies, attendues);
    }

    private static SemestrePedagogique? ResolveSemestre(DashboardData data, long maquetteElementConstitutifId)
    {
        var maquetteEc = data.MaquetteElementsConstitutifs.FirstOrDefault(x => x.Id == maquetteElementConstitutifId);

        return maquetteEc is null ? null : data.Semestres.FirstOrDefault(x => x.Id == maquetteEc.SemestrePedagogiqueId);
    }

    private static DashboardGroupeKey BuildGroupeKey(DashboardData data, Inscription inscription, GroupeDashboard groupe)
    {
        var ouverture = data.Ouvertures.FirstOrDefault(x => x.Id == inscription.ParcoursAcademiqueId);

        return groupe switch
        {
            GroupeDashboard.Cycle => BuildCycleKey(data, ouverture),
            GroupeDashboard.Niveau => BuildNiveauKey(data, inscription.NiveauEtudeId),
            GroupeDashboard.Parcours => new DashboardGroupeKey(
                ouverture?.Id,
                ouverture?.Code ?? string.Empty,
                ouverture?.Libelle ?? string.Empty),
            _ => new DashboardGroupeKey(null, string.Empty, string.Empty)
        };
    }

    private static DashboardGroupeKey BuildCycleKey(DashboardData data, ParcoursAcademique? ouverture)
    {
        var cycle = ouverture is null ? null : data.Cycles.FirstOrDefault(x => x.Id == ouverture.CycleFormationId);

        return new DashboardGroupeKey(cycle?.Id, cycle?.Code ?? string.Empty, cycle?.Libelle ?? string.Empty);
    }

    private static DashboardGroupeKey BuildNiveauKey(DashboardData data, long niveauEtudeId)
    {
        var niveau = data.Niveaux.FirstOrDefault(x => x.Id == niveauEtudeId);

        return new DashboardGroupeKey(niveau?.Id, niveau?.Numero.ToString() ?? string.Empty, niveau?.Libelle ?? string.Empty);
    }

    private static bool IsAdmis(DecisionAcademique decision, StatutValidationAcademique statut)
        => decision == DecisionAcademique.Valide || statut == StatutValidationAcademique.Valide;

    private static bool IsRattrapage(DecisionAcademique decision, StatutValidationAcademique statut)
        => decision == DecisionAcademique.AutoriseRattrapage || statut == StatutValidationAcademique.Rattrapage;

    private static bool IsEchec(DecisionAcademique decision, StatutValidationAcademique statut)
        => decision == DecisionAcademique.Ajoune || statut == StatutValidationAcademique.NonValide;

    private static bool IsMaquetteCompatibleWithParcours(
        DashboardData data,
        MaquettePedagogique maquette,
        long parcoursAcademiqueId)
    {
        var parcours = data.Ouvertures.FirstOrDefault(x => x.Id == parcoursAcademiqueId);

        return parcours is not null
            && maquette.ParcoursAcademiqueId == parcours.Id;
    }

    private static decimal Average(IEnumerable<decimal?> values)
    {
        var items = values
            .Where(x => x is not null)
            .Select(x => x!.Value)
            .ToList();

        return items.Count == 0 ? 0 : Math.Round(items.Average(), 2);
    }

    private static decimal Percent(int value, int total)
        => total <= 0 ? 0 : Math.Round(value * 100m / total, 2);

    private static string FormatTypeEvaluation(TypeEvaluation type)
        => type switch
        {
            TypeEvaluation.ControleContinu => "CCON",
            TypeEvaluation.ControleConnaissance => "CC",
            TypeEvaluation.SessionNormale => "SN",
            TypeEvaluation.SessionRattrapage => "SR",
            _ => type.ToString()
        };

    private enum GroupeDashboard
    {
        Cycle,
        Niveau,
        Parcours
    }

    private record DashboardGroupeKey(long? Id, string Code, string Libelle);

    private record DashboardData(
        List<AnneeAcademique> Annees,
        List<CycleFormation> Cycles,
        List<ParcoursAcademique> Ouvertures,
        List<Filiere> Filieres,
        List<Specialite> Specialites,
        List<NiveauEtude> Niveaux,
        List<ClassePedagogique> Classes,
        List<Inscription> Inscriptions,
        List<MaquettePedagogique> Maquettes,
        List<SemestrePedagogique> Semestres,
        List<UniteEnseignement> UnitesEnseignement,
        List<ElementConstitutif> ElementsConstitutifs,
        List<MaquetteElementConstitutif> MaquetteElementsConstitutifs,
        List<EvaluationAcademique> Evaluations,
        List<NoteEvaluation> Notes,
        List<ResultatElementConstitutif> ResultatsElementsConstitutifs,
        List<ResultatSemestre> ResultatsSemestres,
        List<ResultatAnnuel> ResultatsAnnuels,
        List<ProcesVerbal> ProcesVerbaux);
}
