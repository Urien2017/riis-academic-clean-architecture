namespace RIIS.Academic.Application.Abstractions.Persistence;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Delete(TEntity entity);
    Task DeleteByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
