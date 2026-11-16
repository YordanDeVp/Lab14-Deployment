namespace Lab11_Sapacayo_Yordan.Domain.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void AddEntity(T entity);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
