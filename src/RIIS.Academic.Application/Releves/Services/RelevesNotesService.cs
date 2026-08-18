using RIIS.Academic.Application.Abstractions.Persistence;
using RIIS.Academic.Application.Abstractions.Services;
using RIIS.Academic.Application.Releves.Dtos;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Releves.Services;

public class RelevesNotesService(
    IRepository<Inscription> inscriptions,
    IRepository<Etudiant> etudiants,
    IRepository<AnneeAcademique> anneesAcademiques,
    IRepository<ParcoursAcademique> parcoursAcademiques,
    IRepository<CycleFormation> cyclesFormation,
    IRepository<Filiere> filieres,
    IRepository<Specialite> specialites,
    IRepository<NiveauEtude> niveauxEtude,
    IRepository<MaquettePedagogique> maquettesPedagogiques,
    IRepository<SemestrePedagogique> semestresPedagogiques,
    IRepository<UniteEnseignement> unitesEnseignement,
    IRepository<ElementConstitutif> elementsConstitutifs,
    IRepository<EvaluationAcademique> evaluationsAcademiques,
    IRepository<NoteEvaluation> notesEvaluations,
    ICalculNotesService calculNotesService) : IRelevesNotesService
{
    public async Task<List<ReleveNoteEtudiantDisponibleDto>> GetEtudiantsDisponiblesAsync(
        long? anneeAcademiqueId = null,
        long? cycleFormationId = null,
        long? niveauEtudeId = null,
        long? classePedagogiqueId = null,
        CancellationToken cancellationToken = default)
    {
        var inscriptionItems = await inscriptions.ListAsync(cancellationToken);
        var etudiantItems = await etudiants.ListAsync(cancellationToken);
        var anneeItems = await anneesAcademiques.ListAsync(cancellationToken);
        var ouvertureItems = await parcoursAcademiques.ListAsync(cancellationToken);
        var niveauItems = await niveauxEtude.ListAsync(cancellationToken);

        if (anneeAcademiqueId is not null)
        {
            inscriptionItems = inscriptionItems
                .Where(x => x.AnneeAcademiqueId == anneeAcademiqueId)
                .ToList();
        }

        if (cycleFormationId is not null)
        {
            var ouvertureIds = ouvertureItems
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

        return inscriptionItems
            .Select(x =>
            {
                var etudiant = etudiantItems.FirstOrDefault(e => e.Id == x.EtudiantId);
                var annee = anneeItems.FirstOrDefault(a => a.Id == x.AnneeAcademiqueId);
                var ouverture = ouvertureItems.FirstOrDefault(o => o.Id == x.ParcoursAcademiqueId);
                var niveau = niveauItems.FirstOrDefault(n => n.Id == x.NiveauEtudeId);

                return new ReleveNoteEtudiantDisponibleDto
                {
                    InscriptionId = x.Id,
                    Matricule = etudiant?.Matricule ?? string.Empty,
                    EtudiantNomComplet = etudiant is null ? string.Empty : $"{etudiant.Nom} {etudiant.Prenoms}",
                    ParcoursLibelle = ouverture is null ? string.Empty : FormatCodeLibelle(ouverture.Code, ouverture.Libelle),
                    NiveauLibelle = niveau?.Libelle ?? string.Empty,
                    AnneeAcademiqueLibelle = annee?.Libelle ?? string.Empty
                };
            })
            .OrderBy(x => x.EtudiantNomComplet)
            .ToList();
    }

    public async Task<ReleveNoteAnnuelDto?> GenererReleveAnnuelAsync(
        long inscriptionId,
        CancellationToken cancellationToken = default)
    {
        var inscriptionItems = await inscriptions.ListAsync(cancellationToken);
        var inscription = inscriptionItems.FirstOrDefault(x => x.Id == inscriptionId);

        if (inscription is null)
        {
            return null;
        }

        var etudiantItems = await etudiants.ListAsync(cancellationToken);
        var anneeItems = await anneesAcademiques.ListAsync(cancellationToken);
        var ouvertureItems = await parcoursAcademiques.ListAsync(cancellationToken);
        var cycleItems = await cyclesFormation.ListAsync(cancellationToken);
        var filiereItems = await filieres.ListAsync(cancellationToken);
        var specialiteItems = await specialites.ListAsync(cancellationToken);
        var niveauItems = await niveauxEtude.ListAsync(cancellationToken);
        var maquetteItems = await maquettesPedagogiques.ListAsync(cancellationToken);
        var semestreItems = await semestresPedagogiques.ListAsync(cancellationToken);
        var ueItems = await unitesEnseignement.ListAsync(cancellationToken);
        var ecItems = await elementsConstitutifs.ListAsync(cancellationToken);
        var evaluationItems = await evaluationsAcademiques.ListAsync(cancellationToken);
        var noteItems = await notesEvaluations.ListAsync(cancellationToken);

        var etudiant = etudiantItems.FirstOrDefault(x => x.Id == inscription.EtudiantId)
            ?? throw new InvalidOperationException("L'étudiant de l'inscription est introuvable.");
        var annee = anneeItems.FirstOrDefault(x => x.Id == inscription.AnneeAcademiqueId)
            ?? throw new InvalidOperationException("L'année académique de l'inscription est introuvable.");
        var ouverture = ouvertureItems.FirstOrDefault(x => x.Id == inscription.ParcoursAcademiqueId)
            ?? throw new InvalidOperationException("Le parcours de l'inscription est introuvable.");
        var cycle = cycleItems.FirstOrDefault(x => x.Id == ouverture.CycleFormationId);
        var filiere = filiereItems.FirstOrDefault(x => x.Id == ouverture.FiliereId);
        var specialite = specialiteItems.FirstOrDefault(x => x.Id == ouverture.SpecialiteId);
        var niveau = niveauItems.FirstOrDefault(x => x.Id == inscription.NiveauEtudeId);

        var maquette = ResolveMaquette(inscription, ouverture, maquetteItems);
        if (maquette is null)
        {
            throw new InvalidOperationException("Aucune maquette pédagogique n'est rattachée à cette inscription ou à son parcours.");
        }

        var numerosSemestres = GetNumerosSemestresPourNiveau(niveau?.Numero ?? 1);

        var releve = new ReleveNoteAnnuelDto
        {
            InscriptionId = inscription.Id,
            CycleFormationCode = cycle?.Code ?? string.Empty,
            CycleFormationLibelle = cycle?.Libelle ?? string.Empty,
            EtudiantNomComplet = $"{etudiant.Nom} {etudiant.Prenoms}",
            Matricule = etudiant.Matricule ?? string.Empty,
            DateNaissance = etudiant.DateNaissance,
            LieuNaissance = etudiant.LieuNaissance,
            FiliereLibelle = filiere?.Libelle ?? string.Empty,
            SpecialiteLibelle = specialite?.Libelle,
            NiveauLibelle = niveau?.Libelle ?? string.Empty,
            AnneeAcademiqueLibelle = annee.Libelle
        };

        foreach (var numeroSemestre in numerosSemestres)
        {
            var semestre = semestreItems
                .Where(x => x.MaquettePedagogiqueId == maquette.Id && x.Numero == numeroSemestre)
                .OrderBy(x => x.Id)
                .FirstOrDefault();

            if (semestre is null)
            {
                continue;
            }

            releve.Semestres.Add(BuildSemestreDto(
                inscription,
                annee,
                semestre,
                ueItems,
                ecItems,
                evaluationItems,
                noteItems));
        }

        releve.CreditsCapitalises = releve.Semestres.Sum(x => x.CreditsCapitalises);
        releve.CreditsRequis = releve.Semestres.Sum(x => x.CreditsAttendus);
        releve.MoyenneAnnuelle = CalculerMoyenneAnnuelle(releve.Semestres);
        releve.Mention = GetMention(releve.MoyenneAnnuelle);
        releve.Decision = releve.MoyenneAnnuelle >= 10m && releve.CreditsCapitalises >= releve.CreditsRequis
            ? "ADMIS"
            : "ECHEC";

        releve.Resume = releve.Semestres
            .Select(x => new ReleveNoteResumeDto
            {
                Libelle = $"SEMESTRE {x.Numero}",
                Moyenne = x.Moyenne,
                CreditsCapitalises = x.CreditsCapitalises
            })
            .ToList();

        releve.Resume.Add(new ReleveNoteResumeDto
        {
            Libelle = "ANNUELLE",
            Moyenne = releve.MoyenneAnnuelle,
            CreditsCapitalises = releve.CreditsCapitalises
        });

        return releve;
    }

    private ReleveNoteSemestreDto BuildSemestreDto(
        Inscription inscription,
        AnneeAcademique annee,
        SemestrePedagogique semestre,
        IReadOnlyCollection<UniteEnseignement> ueItems,
        IReadOnlyCollection<ElementConstitutif> ecItems,
        IReadOnlyCollection<EvaluationAcademique> evaluationItems,
        IReadOnlyCollection<NoteEvaluation> noteItems)
    {
        var semestreDto = new ReleveNoteSemestreDto
        {
            Numero = semestre.Numero,
            Libelle = $"SEMESTRE {semestre.Numero}",
            CreditsAttendus = semestre.CreditsAttendus
        };

        var elementsConstitutifsAvecUe =
            (from ec in ecItems
             join ue in ueItems on ec.UniteEnseignementId equals ue.Id
             where ue.SemestrePedagogiqueId == semestre.Id
             orderby ue.OrdreAffichage, ue.Code, ec.OrdreAffichage, ec.Libelle
             select new
             {
                 ElementConstitutif = ec,
                 UniteEnseignement = ue
             })
            .ToList();

        foreach (var item in elementsConstitutifsAvecUe)
        {
            var ec = item.ElementConstitutif;
            var ue = item.UniteEnseignement;

            var moyenneCcon = CalculerMoyenneType(inscription.Id, annee.Id, ec.Id, TypeEvaluation.ControleContinu, evaluationItems, noteItems);
            var moyenneCc = CalculerMoyenneType(inscription.Id, annee.Id, ec.Id, TypeEvaluation.ControleConnaissance, evaluationItems, noteItems);
            var moyenneSn = CalculerMoyenneType(inscription.Id, annee.Id, ec.Id, TypeEvaluation.SessionNormale, evaluationItems, noteItems);
            var moyenneSr = CalculerMoyenneType(inscription.Id, annee.Id, ec.Id, TypeEvaluation.SessionRattrapage, evaluationItems, noteItems);
            var moyenneSessionRetenue = moyenneSr ?? moyenneSn;
            var moyenneFinale = calculNotesService.CalculerMoyenneElementConstitutif(moyenneCcon, moyenneCc, moyenneSessionRetenue);
            var credits = calculNotesService.CalculerCreditsAcquis(ec, moyenneFinale);

            semestreDto.Lignes.Add(new ReleveNoteLigneDto
            {
                UniteEnseignementLibelle = ue.Libelle,
                ElementConstitutifCode = ec.Code,
                ElementConstitutifLibelle = ec.Libelle,
                Session = moyenneSr is not null ? "SR" : $"SN{semestre.Numero}",
                NoteSur20 = moyenneFinale,
                Decision = moyenneFinale >= 10m ? "V" : "NV",
                CreditsCapitalises = credits,
                Grade = GetGrade(moyenneFinale)
            });
        }

        var notes = semestreDto.Lignes
            .Where(x => x.NoteSur20.HasValue)
            .Select(x => x.NoteSur20!.Value)
            .ToList();

        semestreDto.TotalNotes = Truncate2(notes.Sum());
        semestreDto.Moyenne = notes.Count == 0 ? null : Truncate2(notes.Average());
        semestreDto.CreditsCapitalises = semestreDto.Lignes.Sum(x => x.CreditsCapitalises);
        semestreDto.Grade = GetGrade(semestreDto.Moyenne);

        return semestreDto;
    }

    private static MaquettePedagogique? ResolveMaquette(
        Inscription inscription,
        ParcoursAcademique ouverture,
        IReadOnlyCollection<MaquettePedagogique> maquettes)
    {
        if (inscription.MaquettePedagogiqueId is not null)
        {
            var inscriptionMaquette = maquettes.FirstOrDefault(x => x.Id == inscription.MaquettePedagogiqueId);
            if (inscriptionMaquette is not null)
            {
                return inscriptionMaquette;
            }
        }

        return maquettes
            .Where(x => x.ParcoursAcademiqueId == ouverture.Id)
            .OrderByDescending(x => x.Statut == StatutMaquettePedagogique.Active)
            .ThenByDescending(x => x.Version)
            .FirstOrDefault();
    }

    private static decimal? CalculerMoyenneType(
        long inscriptionId,
        long anneeAcademiqueId,
        long elementConstitutifId,
        TypeEvaluation type,
        IReadOnlyCollection<EvaluationAcademique> evaluations,
        IReadOnlyCollection<NoteEvaluation> notes)
    {
        var evaluationIds = evaluations
            .Where(x => x.AnneeAcademiqueId == anneeAcademiqueId
                && x.ElementConstitutifId == elementConstitutifId
                && x.Type == type)
            .Select(x => x.Id)
            .ToHashSet();

        var valeurs = notes
            .Where(x => evaluationIds.Contains(x.EvaluationAcademiqueId)
                && x.InscriptionId == inscriptionId
                && x.StatutPresence == StatutPresenceEvaluation.Present
                && x.Valeur.HasValue)
            .Select(x => x.Valeur!.Value)
            .ToList();

        return valeurs.Count == 0 ? null : Truncate2(valeurs.Average());
    }

    private static byte[] GetNumerosSemestresPourNiveau(byte niveauNumero)
        => niveauNumero switch
        {
            1 => [1, 2],
            2 => [3, 4],
            3 => [5, 6],
            _ => [(byte)((niveauNumero * 2) - 1), (byte)(niveauNumero * 2)]
        };

    private static decimal? CalculerMoyenneAnnuelle(IReadOnlyCollection<ReleveNoteSemestreDto> semestres)
    {
        var moyennes = semestres
            .Where(x => x.Moyenne.HasValue)
            .Select(x => x.Moyenne!.Value)
            .ToList();

        return moyennes.Count == 0 ? null : Truncate2(moyennes.Average());
    }

    private static decimal Truncate2(decimal value)
        => Math.Truncate(value * 100m) / 100m;

    private static string GetGrade(decimal? note)
    {
        if (note is null)
        {
            return string.Empty;
        }

        return note.Value switch
        {
            >= 18m => "A+",
            >= 16m => "A",
            >= 14m => "B+",
            >= 12m => "B",
            >= 10m => "C",
            >= 8m => "D",
            _ => "F"
        };
    }

    private static string GetMention(decimal? moyenne)
    {
        if (moyenne is null)
        {
            return "NON RENSEIGNÉ";
        }

        return moyenne.Value switch
        {
            >= 16m => "TRÈS BIEN",
            >= 14m => "BIEN",
            >= 12m => "ASSEZ BIEN",
            >= 10m => "PASSABLE",
            _ => "INSUFFISANT"
        };
    }

    private static string FormatCodeLibelle(string code, string libelle)
        => string.IsNullOrWhiteSpace(code) ? libelle : $"{code} - {libelle}";
}
