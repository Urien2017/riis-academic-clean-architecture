using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RIIS.Academic.Application.Abstractions.Persistence;
using RIIS.Academic.Application.Abstractions.Services;
using RIIS.Academic.Application.ClassesPedagogiques.Services;
using RIIS.Academic.Application.Dashboard.Services;
using RIIS.Academic.Application.Etudiants.Services;
using RIIS.Academic.Application.Evaluations.Services;
using RIIS.Academic.Application.Inscriptions.Services;
using RIIS.Academic.Application.Notes.Services;
using RIIS.Academic.Application.ProcesVerbaux.Services;
using RIIS.Academic.Application.Programmes.Services;
using RIIS.Academic.Application.Releves.Services;
using RIIS.Academic.Application.Referentiels.Services;
using RIIS.Academic.Application.Scolarite.Services;
using RIIS.Academic.Infrastructure.Documents;
using RIIS.Academic.Infrastructure.Persistence;
using RIIS.Academic.Infrastructure.Persistence.Repositories;

namespace RIIS.Academic.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRiisAcademicInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = GetRiisConnectionString(configuration);

        services.AddDbContext<RiisAcademicDbContext>(
            options => options.UseSqlServer(connectionString),
            contextLifetime: ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Transient);

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IReferentielsService, ReferentielsService>();
        services.AddScoped<IEtudiantsService, EtudiantsService>();
        services.AddScoped<IClassesPedagogiquesService, ClassesPedagogiquesService>();
        services.AddScoped<IInscriptionsService, InscriptionsService>();
        services.AddScoped<IProgrammePedagogiqueService, ProgrammePedagogiqueService>();
        services.AddScoped<IEvaluationsService, EvaluationsService>();
        services.AddScoped<ICalculNotesService, CalculNotesService>();
        services.AddScoped<ISaisieNotesService, SaisieNotesService>();
        services.AddScoped<IDashboardAcademiqueService, DashboardAcademiqueService>();
        services.AddScoped<IProcesVerbauxService, ProcesVerbauxService>();
        services.AddScoped<IRelevesNotesService, RelevesNotesService>();
        services.AddScoped<ITypesElementsScolariteService, TypesElementsScolariteService>();
        services.AddScoped<IModesPaiementScolariteService, ModesPaiementScolariteService>();
        services.AddScoped<ITarifsScolariteService, TarifsScolariteService>();
        services.AddScoped<IDossiersScolariteService, DossiersScolariteService>();
        services.AddScoped<IFinancesScolariteService, FinancesScolariteService>();

        services.AddScoped<IProcesVerbalExcelExportService, ProcesVerbalExcelExportService>();
        services.AddScoped<IProcesVerbalTemplateWordExportService, ProcesVerbalTemplateWordExportService>();
        services.AddScoped<IProcesVerbalWordExportService, ProcesVerbalWordExportService>();
        services.AddScoped<IReleveNoteTemplateWordExportService, ReleveNoteTemplateWordExportService>();
        services.AddScoped<IReleveNoteWordExportService, ReleveNoteWordExportService>();

        return services;
    }

    private static string GetRiisConnectionString(IConfiguration configuration)
    {
        var env = configuration["envval"];

        var connectionName = string.Equals(env, "dev", StringComparison.OrdinalIgnoreCase)
            ? "RiisSqlServer"
            : "OtherConnection";

        return configuration.GetConnectionString(connectionName)
            ?? throw new InvalidOperationException($"Connection string '{connectionName}' not found.");
    }
}
