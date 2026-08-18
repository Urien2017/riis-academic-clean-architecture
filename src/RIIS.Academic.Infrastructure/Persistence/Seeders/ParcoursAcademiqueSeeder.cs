using Microsoft.EntityFrameworkCore;
using RIIS.Academic.Domain;

namespace RIIS.Academic.Infrastructure.Persistence.Seeders;

public static class ParcoursAcademiqueSeeder
{
    private static readonly ParcoursPlanItem[] Plan =
    [
        new("PREPA", 1, "INGENIEUR", "IGL"),
        new("PREPA", 2, "INGENIEUR", "IGL"),
        new("PREPA", 1, "MANAGEMENT-ECONOMIE", "MG"),
        new("PREPA", 2, "MANAGEMENT-ECONOMIE", "MG"),

        new("BTS", 1, "TERTIAIRE", "BF"),
        new("BTS", 2, "TERTIAIRE", "BF"),
        new("BTS", 1, "TERTIAIRE", "CGE"),
        new("BTS", 2, "TERTIAIRE", "CGE"),
        new("BTS", 1, "TERTIAIRE", "GRH"),
        new("BTS", 2, "TERTIAIRE", "GRH"),
        new("BTS", 1, "INFORMATIQUE", "GL"),
        new("BTS", 2, "INFORMATIQUE", "GL"),
        new("BTS", 1, "INFORMATIQUE", "RS"),
        new("BTS", 2, "INFORMATIQUE", "RS"),
        new("BTS", 1, "INFORMATIQUE", "IGL"),
        new("BTS", 2, "INFORMATIQUE", "IGL"),
        new("BTS", 1, "GENIE-CIVIL", "BAT"),
        new("BTS", 2, "GENIE-CIVIL", "BAT"),
        new("BTS", 1, "GENIE-CIVIL", "TP"),
        new("BTS", 2, "GENIE-CIVIL", "TP"),
        new("BTS", 1, "GENIE-CIVIL", "GT"),
        new("BTS", 2, "GENIE-CIVIL", "GT"),
        new("BTS", 1, "GENIE-ELECTRIQUE", "ELT"),
        new("BTS", 2, "GENIE-ELECTRIQUE", "ELT"),
        new("BTS", 1, "GESTION", "COFI"),
        new("BTS", 2, "GESTION", "COFI"),

        new("LICENCE", 1, "INFORMATIQUE", "IGL"),
        new("LICENCE", 2, "INFORMATIQUE", "IGL"),
        new("LICENCE", 3, "INFORMATIQUE", "IGL"),
        new("LICENCE", 1, "GESTION", "COFI"),
        new("LICENCE", 2, "GESTION", "COFI"),
        new("LICENCE", 3, "GESTION", "COFI"),

        new("MASTER", 4, "INFORMATIQUE", "IGL"),
        new("MASTER", 5, "INFORMATIQUE", "IGL"),
        new("MASTER", 4, "GESTION", "COFI"),
        new("MASTER", 5, "GESTION", "COFI")
    ];

    public static async Task SeedAsync(
        RiisAcademicDbContext context,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        var annees = await context.AnneesAcademiques
            .OrderBy(x => x.AnneeDebut)
            .ToListAsync(cancellationToken);
        var cycles = await context.CyclesFormation.ToListAsync(cancellationToken);
        var niveaux = await context.NiveauxEtude.ToListAsync(cancellationToken);
        var filieres = await context.Filieres.ToListAsync(cancellationToken);
        var specialites = await context.Specialites.ToListAsync(cancellationToken);
        var parcoursExistants = await context.ParcoursAcademiques.ToListAsync(cancellationToken);

        foreach (var annee in annees)
        {
            foreach (var item in Plan)
            {
                var cycle = cycles.FirstOrDefault(x => string.Equals(x.Code, item.CycleCode, StringComparison.OrdinalIgnoreCase))
                    ?? throw new InvalidOperationException($"Le cycle '{item.CycleCode}' est introuvable.");
                var niveau = niveaux.FirstOrDefault(x => x.Numero == item.NiveauNumero)
                    ?? throw new InvalidOperationException($"Le niveau '{item.NiveauNumero}' est introuvable.");
                var filiere = filieres.FirstOrDefault(x => string.Equals(x.Code, item.FiliereCode, StringComparison.OrdinalIgnoreCase))
                    ?? throw new InvalidOperationException($"La filiere '{item.FiliereCode}' est introuvable.");
                var specialite = specialites.FirstOrDefault(x =>
                    x.FiliereId == filiere.Id
                    && string.Equals(x.Code, item.SpecialiteCode, StringComparison.OrdinalIgnoreCase))
                    ?? throw new InvalidOperationException(
                        $"La specialite '{item.SpecialiteCode}' est introuvable pour la filiere '{item.FiliereCode}'.");

                var code = BuildCode(annee, cycle, niveau, filiere, specialite);
                var libelle = BuildLibelle(annee, cycle, niveau, filiere, specialite);

                var parcours = parcoursExistants.FirstOrDefault(x =>
                    x.AnneeAcademiqueId == annee.Id
                    && x.CycleFormationId == cycle.Id
                    && x.NiveauEtudeId == niveau.Id
                    && x.FiliereId == filiere.Id
                    && x.SpecialiteId == specialite.Id);

                if (parcours is null)
                {
                    parcours = new ParcoursAcademique
                    {
                        AnneeAcademiqueId = annee.Id,
                        CycleFormationId = cycle.Id,
                        NiveauEtudeId = niveau.Id,
                        FiliereId = filiere.Id,
                        SpecialiteId = specialite.Id,
                        Code = code,
                        Libelle = libelle,
                        EstActive = annee.EstActive
                    };

                    context.ParcoursAcademiques.Add(parcours);
                    parcoursExistants.Add(parcours);
                    continue;
                }

                parcours.Code = code;
                parcours.Libelle = libelle;
                parcours.EstActive = annee.EstActive;
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private static string BuildCode(
        AnneeAcademique annee,
        CycleFormation cycle,
        NiveauEtude niveau,
        Filiere filiere,
        Specialite specialite)
        => $"{annee.AnneeDebut % 100:00}-{cycle.Code}-N{niveau.Numero}-{filiere.Code}-{specialite.Code}".ToUpperInvariant();

    private static string BuildLibelle(
        AnneeAcademique annee,
        CycleFormation cycle,
        NiveauEtude niveau,
        Filiere filiere,
        Specialite specialite)
        => $"{annee.Libelle} - {cycle.Libelle} {niveau.Libelle} - {filiere.Libelle} - {specialite.Libelle}";

    private readonly record struct ParcoursPlanItem(
        string CycleCode,
        byte NiveauNumero,
        string FiliereCode,
        string SpecialiteCode);
}
