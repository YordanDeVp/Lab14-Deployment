using Lab11_Sapacayo_Yordan.Domain.Repositories;
using Lab11_Sapacayo_Yordan.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Lab11_Sapacayo_Yordan.Infrastructure.Adapters.Repositories;

public class GenericRepository<T>(TicketeraContext context) : IGenericRepository<T> where T : class
{
    protected TicketeraContext Context { get; } = context;

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<T>().FindAsync(new object?[] { id }, cancellationToken);
    }

    public void AddEntity(T entity)
    {
        Context.Set<T>().Add(entity);
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }
}
