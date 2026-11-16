using Lab11_Sapacayo_Yordan.Domain.Entities;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using Lab11_Sapacayo_Yordan.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Lab11_Sapacayo_Yordan.Infrastructure.Adapters.Repositories;

public class UserRepository(TicketeraContext context) : GenericRepository<User>(context), IUserRepository
{
    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return Context.Users.SingleOrDefaultAsync(user => user.Username == username, cancellationToken);
    }

    public Task<bool> EmailExistsAsync(string email, Guid? exceptId = null, CancellationToken cancellationToken = default)
    {
        return Context.Users.AnyAsync(user => user.Email == email && (!exceptId.HasValue || user.UserId != exceptId.Value), cancellationToken);
    }

    public async Task<IReadOnlyList<string>> GetRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await (from userRole in Context.UserRoles
                      join role in Context.Roles on userRole.RoleId equals role.RoleId
                      where userRole.UserId == userId
                      select role.RoleName).ToListAsync(cancellationToken);
    }

    public async Task<bool> HasActivityAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await Context.Tickets.AnyAsync(ticket => ticket.UserId == userId, cancellationToken)
            || await Context.Responses.AnyAsync(response => response.ResponderId == userId, cancellationToken);
    }
}
