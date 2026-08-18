using Microsoft.EntityFrameworkCore;

namespace RIIS.Academic.Infrastructure.Persistence;

public static class RiisAcademicDatabaseResetter
{
    public static async Task ResetNonReferentialDataAsync(
        RiisAcademicDbContext context,
        bool confirmReset,
        bool includeTarifsScolarite = true,
        CancellationToken cancellationToken = default)
    {
        if (!confirmReset)
        {
            throw new InvalidOperationException(
                "Reset refuse: passez confirmReset=true explicitement pour purger les donnees metier.");
        }

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        await ExecuteAsync(context, "UPDATE EvaluationsAcademiques SET EvaluationRemplaceeId = NULL", cancellationToken);

        await ExecuteAsync(context, "DELETE FROM AffectationsPaiementsEcheances", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM NotificationsScolarite", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM PaiementsScolarite", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM EcheancesScolarite", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM DocumentsElementsScolarite", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM ValidationsElementsScolarite", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM ElementsScolariteEtudiants", cancellationToken);

        await ExecuteAsync(context, "DELETE FROM DossiersAdmission", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM ValidationsInscriptions", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM DossiersScolarite", cancellationToken);

        await ExecuteAsync(context, "DELETE FROM ProcesVerbauxLignes", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM ProcesVerbaux", cancellationToken);

        await ExecuteAsync(context, "DELETE FROM ResultatsAnnuels", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM ResultatsSemestres", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM ResultatsUnitesEnseignement", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM ResultatsElementsConstitutifs", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM NotesEvaluations", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM EvaluationsAcademiques", cancellationToken);

        await ExecuteAsync(context, "DELETE FROM Inscriptions", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM ContactsUrgence", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM Etudiants", cancellationToken);

        await ExecuteAsync(context, "DELETE FROM ClassesPedagogiques", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM ElementsConstitutifs", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM UnitesEnseignement", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM SemestresPedagogiques", cancellationToken);
        await ExecuteAsync(context, "DELETE FROM MaquettesPedagogiques", cancellationToken);

        if (includeTarifsScolarite)
        {
            await ExecuteAsync(context, "DELETE FROM TarifsScolarite", cancellationToken);
        }

        await ExecuteAsync(context, "DELETE FROM ParcoursAcademiques", cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }

    private static Task ExecuteAsync(
        RiisAcademicDbContext context,
        string sql,
        CancellationToken cancellationToken)
        => context.Database.ExecuteSqlRawAsync(sql, cancellationToken);
}
