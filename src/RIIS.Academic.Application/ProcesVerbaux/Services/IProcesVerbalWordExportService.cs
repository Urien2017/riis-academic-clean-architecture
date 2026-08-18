using RIIS.Academic.Application.ProcesVerbaux.Dtos;

namespace RIIS.Academic.Application.ProcesVerbaux.Services;

public interface IProcesVerbalWordExportService
{
    Task<ProcesVerbalWordExportDto?> ExporterProcesVerbalAsync(
        long procesVerbalId,
        CancellationToken cancellationToken = default);
}
