
namespace RIIS.Academic.Application.Abstractions.Services;

public interface IInscriptionService
{
    Task<long> CreerInscriptionAsync(CancellationToken cancellationToken = default);
    Task ValiderInscriptionAsync(long inscriptionId, CancellationToken cancellationToken = default);
}
