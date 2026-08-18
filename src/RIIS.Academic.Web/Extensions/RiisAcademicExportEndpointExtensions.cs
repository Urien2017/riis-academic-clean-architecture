using Microsoft.AspNetCore.Mvc;
using RIIS.Academic.Application.ProcesVerbaux.Services;
using RIIS.Academic.Application.Releves.Services;

namespace RIIS.Academic.Web.Extensions;

public static class RiisAcademicExportEndpointExtensions
{
    public static IEndpointRouteBuilder MapRiisAcademicExportEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/exports/pv/{procesVerbalId:long}.docx",
            async (
                [FromRoute] long procesVerbalId,
                [FromServices] IProcesVerbalWordExportService exportService,
                CancellationToken cancellationToken) =>
            {
                var export = await exportService.ExporterProcesVerbalAsync(
                    procesVerbalId,
                    cancellationToken);

                return export is null
                    ? Results.NotFound()
                    : Results.File(export.Content, export.ContentType, export.FileName);
            });

        endpoints.MapGet(
            "/exports/pv/{procesVerbalId:long}.xlsx",
            async (
                [FromRoute] long procesVerbalId,
                [FromServices] IProcesVerbalExcelExportService exportService,
                CancellationToken cancellationToken) =>
            {
                var export = await exportService.ExporterProcesVerbalAsync(
                    procesVerbalId,
                    cancellationToken);

                return export is null
                    ? Results.NotFound()
                    : Results.File(export.Content, export.ContentType, export.FileName);
            });

        endpoints.MapGet(
            "/exports/pv/{procesVerbalId:long}.modele.docx",
            async (
                [FromRoute] long procesVerbalId,
                [FromServices] IProcesVerbalTemplateWordExportService exportService,
                CancellationToken cancellationToken) =>
            {
                var export = await exportService.ExporterProcesVerbalAsync(
                    procesVerbalId,
                    cancellationToken);

                return export is null
                    ? Results.NotFound()
                    : Results.File(export.Content, export.ContentType, export.FileName);
            });

        endpoints.MapGet(
            "/exports/releves/{inscriptionId:long}.docx",
            async (
                [FromRoute] long inscriptionId,
                [FromServices] IReleveNoteWordExportService exportService,
                CancellationToken cancellationToken) =>
            {
                var export = await exportService.ExporterReleveAnnuelAsync(
                    inscriptionId,
                    cancellationToken);

                return export is null
                    ? Results.NotFound()
                    : Results.File(export.Content, export.ContentType, export.FileName);
            });

        endpoints.MapGet(
            "/exports/releves/{inscriptionId:long}.modele.docx",
            async (
                [FromRoute] long inscriptionId,
                [FromServices] IReleveNoteTemplateWordExportService exportService,
                CancellationToken cancellationToken) =>
            {
                var export = await exportService.ExporterReleveAnnuelAsync(
                    inscriptionId,
                    cancellationToken);

                return export is null
                    ? Results.NotFound()
                    : Results.File(export.Content, export.ContentType, export.FileName);
            });

        return endpoints;
    }
}
