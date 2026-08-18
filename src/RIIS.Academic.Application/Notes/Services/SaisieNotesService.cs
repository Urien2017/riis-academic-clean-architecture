using RIIS.Academic.Application.Abstractions.Persistence;
using RIIS.Academic.Application.Common.Dtos;
using RIIS.Academic.Application.Notes.Dtos;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Notes.Services;

public class SaisieNotesService(
    IRepository<AnneeAcademique> anneesAcademiques,
    IRepository<ClassePedagogique> classesPedagogiques,
    IRepository<Inscription> inscriptions,
    IRepository<Etudiant> etudiants,
    IRepository<EvaluationAcademique> evaluations,
    IRepository<NoteEvaluation> notesEvaluations,
    IRepository<ElementConstitutif> elementsConstitutifs,
    IRepository<UniteEnseignement> unitesEnseignement,
    IRepository<ResultatSemestre> resultatsSemestres) : ISaisieNotesService
{
    public async Task<List<LookupDto>> GetAnneesAcademiquesLookupAsync(CancellationToken cancellationToken = default)
    {
        var items = await anneesAcademiques.ListAsync(cancellationToken);

        return items
            .OrderByDescending(x => x.AnneeDebut)
            .Select(x => new LookupDto { Id = x.Id, Libelle = x.Libelle })
            .ToList();
    }

    public async Task<List<LookupDto>> GetClassesPedagogiquesLookupAsync(long? anneeAcademiqueId = null, CancellationToken cancellationToken = default)
    {
        var items = await classesPedagogiques.ListAsync(cancellationToken);

        if (anneeAcademiqueId is not null)
        {
            items = items.Where(x => x.AnneeAcademiqueId == anneeAcademiqueId).ToList();
        }

        return items
            .Where(x => x.EstActive)
            .OrderBy(x => x.Code)
            .ThenBy(x => x.Libelle)
            .Select(x => new LookupDto { Id = x.Id, Libelle = FormatCodeLibelle(x.Code, x.Libelle) })
            .ToList();
    }

    public async Task<List<LookupDto>> GetUnitesEnseignementLookupAsync(CancellationToken cancellationToken = default)
    {
        var items = await unitesEnseignement.ListAsync(cancellationToken);

        return items
            .OrderBy(x => x.OrdreAffichage)
            .ThenBy(x => x.Code)
            .Select(x => new LookupDto { Id = x.Id, Libelle = FormatCodeLibelle(x.Code, x.Libelle) })
            .ToList();
    }

    public async Task<List<LookupDto>> GetElementsConstitutifsLookupAsync(long? uniteEnseignementId = null, CancellationToken cancellationToken = default)
    {
        var items = await elementsConstitutifs.ListAsync(cancellationToken);

        if (uniteEnseignementId is not null)
        {
            items = items.Where(x => x.UniteEnseignementId == uniteEnseignementId).ToList();
        }

        return items
            .OrderBy(x => x.OrdreAffichage)
            .ThenBy(x => x.Libelle)
            .Select(x => new LookupDto { Id = x.Id, Libelle = FormatCodeLibelle(x.Code, x.Libelle) })
            .ToList();
    }

    public async Task<List<LookupDto>> GetEvaluationsLookupAsync(
        long? anneeAcademiqueId = null,
        long? uniteEnseignementId = null,
        long? elementConstitutifId = null,
        TypeEvaluation? typeEvaluation = null,
        CancellationToken cancellationToken = default)
    {
        var evaluationItems = await evaluations.ListAsync(cancellationToken);
        var ecItems = await elementsConstitutifs.ListAsync(cancellationToken);

        if (anneeAcademiqueId is not null)
        {
            evaluationItems = evaluationItems.Where(x => x.AnneeAcademiqueId == anneeAcademiqueId).ToList();
        }

        if (uniteEnseignementId is not null)
        {
            var ecIds = ecItems
                .Where(x => x.UniteEnseignementId == uniteEnseignementId)
                .Select(x => x.Id)
                .ToHashSet();

            evaluationItems = evaluationItems.Where(x => ecIds.Contains(x.ElementConstitutifId)).ToList();
        }

        if (elementConstitutifId is not null)
        {
            evaluationItems = evaluationItems.Where(x => x.ElementConstitutifId == elementConstitutifId).ToList();
        }

        if (typeEvaluation is not null)
        {
            evaluationItems = evaluationItems.Where(x => x.Type == typeEvaluation).ToList();
        }

        return evaluationItems
            .OrderBy(x => x.Type)
            .ThenBy(x => x.Numero)
            .ThenBy(x => x.Code)
            .Select(x =>
            {
                var ec = ecItems.FirstOrDefault(ec => ec.Id == x.ElementConstitutifId);
                var ecLibelle = ec is null ? string.Empty : FormatCodeLibelle(ec.Code, ec.Libelle);

                return new LookupDto
                {
                    Id = x.Id,
                    Libelle = $"{GetTypeCode(x.Type)}{x.Numero} - {x.Libelle} | {ecLibelle}"
                };
            })
            .ToList();
    }

    public async Task<SaisieNotesGrilleDto> GetGrilleSaisieAsync(
        long evaluationAcademiqueId,
        long classePedagogiqueId,
        CancellationToken cancellationToken = default)
    {
        var evaluation = await evaluations.GetByIdAsync(evaluationAcademiqueId, cancellationToken)
            ?? throw new InvalidOperationException("L'évaluation sélectionnée est introuvable.");

        var classe = await classesPedagogiques.GetByIdAsync(classePedagogiqueId, cancellationToken)
            ?? throw new InvalidOperationException("La classe sélectionnée est introuvable.");

        if (classe.AnneeAcademiqueId != evaluation.AnneeAcademiqueId)
        {
            throw new InvalidOperationException("La classe sélectionnée n'appartient pas à l'année académique de l'évaluation.");
        }

        var anneeItems = await anneesAcademiques.ListAsync(cancellationToken);
        var inscriptionItems = await inscriptions.ListAsync(cancellationToken);
        var etudiantItems = await etudiants.ListAsync(cancellationToken);
        var noteItems = await notesEvaluations.ListAsync(cancellationToken);
        var ecItems = await elementsConstitutifs.ListAsync(cancellationToken);
        var ueItems = await unitesEnseignement.ListAsync(cancellationToken);
        var resultatSemestreItems = await resultatsSemestres.ListAsync(cancellationToken);

        var annee = anneeItems.FirstOrDefault(x => x.Id == evaluation.AnneeAcademiqueId);
        var ec = ecItems.FirstOrDefault(x => x.Id == evaluation.ElementConstitutifId)
            ?? throw new InvalidOperationException("L'EC de l'évaluation est introuvable.");
        var ue = ueItems.FirstOrDefault(x => x.Id == ec.UniteEnseignementId)
            ?? throw new InvalidOperationException("L'UE de l'évaluation est introuvable.");

        var inscriptionsClasse = inscriptionItems
            .Where(x => x.ClassePedagogiqueId == classePedagogiqueId
                && x.AnneeAcademiqueId == evaluation.AnneeAcademiqueId
                && x.Statut != StatutInscription.Annulee)
            .ToList();

        string? avertissement = null;

        if (evaluation.Type == TypeEvaluation.SessionRattrapage)
        {
            var resultatsPourSemestre = resultatSemestreItems
                .Where(x => x.SemestrePedagogiqueId == ue.SemestrePedagogiqueId)
                .ToList();

            if (resultatsPourSemestre.Count == 0)
            {
                avertissement = "Aucun résultat semestriel n'est encore calculé pour filtrer les étudiants éligibles au rattrapage. Tous les étudiants de la classe sont affichés provisoirement.";
            }
            else
            {
                var inscriptionsEligibles = resultatsPourSemestre
                    .Where(x => x.CreditsAcquis < x.CreditsRequis)
                    .Select(x => x.InscriptionId)
                    .ToHashSet();

                inscriptionsClasse = inscriptionsClasse
                    .Where(x => inscriptionsEligibles.Contains(x.Id))
                    .ToList();
            }
        }

        return new SaisieNotesGrilleDto
        {
            EvaluationAcademiqueId = evaluation.Id,
            ClassePedagogiqueId = classe.Id,
            EvaluationLibelle = $"{evaluation.Code} - {evaluation.Libelle}",
            ClassePedagogiqueLibelle = FormatCodeLibelle(classe.Code, classe.Libelle),
            AnneeAcademiqueLibelle = annee?.Libelle ?? string.Empty,
            ElementConstitutifLibelle = FormatCodeLibelle(ec.Code, ec.Libelle),
            UniteEnseignementLibelle = FormatCodeLibelle(ue.Code, ue.Libelle),
            TypeEvaluation = evaluation.Type,
            Bareme = evaluation.Bareme,
            PonderationPourcentage = evaluation.PonderationPourcentage,
            Avertissement = avertissement,
            Lignes = inscriptionsClasse
                .Select(inscription =>
                {
                    var etudiant = etudiantItems.FirstOrDefault(x => x.Id == inscription.EtudiantId);
                    var note = noteItems.FirstOrDefault(x => x.EvaluationAcademiqueId == evaluation.Id && x.InscriptionId == inscription.Id);
                    var resultatSemestre = resultatSemestreItems.FirstOrDefault(x =>
                        x.InscriptionId == inscription.Id
                        && x.SemestrePedagogiqueId == ue.SemestrePedagogiqueId);

                    return new SaisieNoteLigneDto
                    {
                        NoteEvaluationId = note?.Id ?? 0,
                        InscriptionId = inscription.Id,
                        Matricule = etudiant?.Matricule ?? string.Empty,
                        NomComplet = etudiant is null ? string.Empty : $"{etudiant.Nom} {etudiant.Prenoms}".Trim(),
                        Valeur = note?.Valeur,
                        StatutPresence = note?.StatutPresence ?? StatutPresenceEvaluation.Present,
                        Observation = note?.Observation,
                        CreditsSemestreAcquis = resultatSemestre?.CreditsAcquis,
                        CreditsSemestreRequis = resultatSemestre?.CreditsRequis,
                        EstEligibleSessionRattrapage = evaluation.Type != TypeEvaluation.SessionRattrapage
                            || resultatSemestre is null
                            || resultatSemestre.CreditsAcquis < resultatSemestre.CreditsRequis
                    };
                })
                .OrderBy(x => x.NomComplet)
                .ToList()
        };
    }

    public async Task SaveNotesAsync(SaisieNotesGrilleDto grille, string? saisiePar = null, CancellationToken cancellationToken = default)
    {
        var evaluation = await evaluations.GetByIdAsync(grille.EvaluationAcademiqueId, cancellationToken)
            ?? throw new InvalidOperationException("L'évaluation sélectionnée est introuvable.");

        var classe = await classesPedagogiques.GetByIdAsync(grille.ClassePedagogiqueId, cancellationToken)
            ?? throw new InvalidOperationException("La classe sélectionnée est introuvable.");

        if (classe.AnneeAcademiqueId != evaluation.AnneeAcademiqueId)
        {
            throw new InvalidOperationException("La classe sélectionnée n'appartient pas à l'année académique de l'évaluation.");
        }

        var inscriptionItems = await inscriptions.ListAsync(cancellationToken);
        var noteItems = await notesEvaluations.ListAsync(cancellationToken);
        var now = DateTime.UtcNow;

        foreach (var ligne in grille.Lignes)
        {
            var inscription = inscriptionItems.FirstOrDefault(x => x.Id == ligne.InscriptionId)
                ?? throw new InvalidOperationException("Une inscription de la grille est introuvable.");

            if (inscription.ClassePedagogiqueId != grille.ClassePedagogiqueId || inscription.AnneeAcademiqueId != evaluation.AnneeAcademiqueId)
            {
                throw new InvalidOperationException("Une inscription de la grille n'appartient pas à la classe ou à l'année de l'évaluation.");
            }

            if (ligne.StatutPresence == StatutPresenceEvaluation.Present)
            {
                if (ligne.Valeur is not null && (ligne.Valeur < 0 || ligne.Valeur > evaluation.Bareme))
                {
                    throw new InvalidOperationException($"La note de {ligne.NomComplet} doit être comprise entre 0 et {evaluation.Bareme}.");
                }
            }
            else
            {
                ligne.Valeur = null;
            }

            var note = noteItems.FirstOrDefault(x => x.EvaluationAcademiqueId == evaluation.Id && x.InscriptionId == ligne.InscriptionId);

            if (note is null)
            {
                await notesEvaluations.AddAsync(new NoteEvaluation
                {
                    EvaluationAcademiqueId = evaluation.Id,
                    InscriptionId = ligne.InscriptionId,
                    Valeur = ligne.Valeur,
                    StatutPresence = ligne.StatutPresence,
                    Observation = NormalizeNullable(ligne.Observation),
                    SaisieLeUtc = now,
                    SaisiePar = NormalizeNullable(saisiePar)
                }, cancellationToken);
            }
            else
            {
                note.Valeur = ligne.Valeur;
                note.StatutPresence = ligne.StatutPresence;
                note.Observation = NormalizeNullable(ligne.Observation);
                note.SaisieLeUtc = now;
                note.SaisiePar = NormalizeNullable(saisiePar);
            }
        }

        await notesEvaluations.SaveChangesAsync(cancellationToken);
    }

    private static string GetTypeCode(TypeEvaluation type)
        => type switch
        {
            TypeEvaluation.ControleContinu => "CCON",
            TypeEvaluation.ControleConnaissance => "CC",
            TypeEvaluation.SessionNormale => "SN",
            TypeEvaluation.SessionRattrapage => "SR",
            _ => "EV"
        };

    private static string FormatCodeLibelle(string? code, string libelle)
        => string.IsNullOrWhiteSpace(code) ? libelle : $"{code} - {libelle}";

    private static string? NormalizeNullable(string? value)
    {
        var normalized = value?.Trim();

        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }
}
