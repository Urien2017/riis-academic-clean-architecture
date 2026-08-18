using RIIS.Academic.Application.ProcesVerbaux.Dtos;

namespace RIIS.Academic.Application.ProcesVerbaux.Services;

public interface IProcesVerbalExcelExportService
{
    Task<ProcesVerbalExcelExportDto?> ExporterProcesVerbalAsync(
        long procesVerbalId,
        CancellationToken cancellationToken = default);
}
