
namespace RIIS.Academic.Application.Abstractions.Services;

public interface IProcesVerbalService
{
    Task<long> GenererProcesVerbalAsync(long classePedagogiqueId, long semestrePedagogiqueId, CancellationToken cancellationToken = default);
}
