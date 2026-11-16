using Lab11_Sapacayo_Yordan.Domain.Entities;

namespace Lab11_Sapacayo_Yordan.Domain.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, Guid? exceptId = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetRolesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> HasActivityAsync(Guid userId, CancellationToken cancellationToken = default);
}
