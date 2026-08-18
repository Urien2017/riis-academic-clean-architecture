using Microsoft.EntityFrameworkCore;
using RIIS.Academic.Application.Abstractions.Persistence;

namespace RIIS.Academic.Infrastructure.Persistence.Repositories;

public class EfRepository<TEntity>(RiisAcademicDbContext context) : IRepository<TEntity>
    where TEntity : class
{
    public Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default)
        => context.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.Set<TEntity>().FindAsync([id], cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await context.Set<TEntity>().AddAsync(entity, cancellationToken);

    public void Delete(TEntity entity)
        => context.Set<TEntity>().Remove(entity);

    public async Task DeleteByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is null) return;

        Delete(entity);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
}
