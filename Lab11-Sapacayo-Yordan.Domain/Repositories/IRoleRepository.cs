using Lab11_Sapacayo_Yordan.Domain.Entities;

namespace Lab11_Sapacayo_Yordan.Domain.Repositories;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default);
    Task AssignAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
}
