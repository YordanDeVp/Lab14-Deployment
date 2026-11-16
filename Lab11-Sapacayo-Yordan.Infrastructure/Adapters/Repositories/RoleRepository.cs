using Lab11_Sapacayo_Yordan.Domain.Entities;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using Lab11_Sapacayo_Yordan.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Lab11_Sapacayo_Yordan.Infrastructure.Adapters.Repositories;

public class RoleRepository(TicketeraContext context) : GenericRepository<Role>(context), IRoleRepository
{
    public Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return Context.Roles.AnyAsync(role => role.RoleName == name, cancellationToken);
    }

    public async Task AssignAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default)
    {
        var exists = await Context.UserRoles.AnyAsync(userRole => userRole.UserId == userId && userRole.RoleId == roleId, cancellationToken);
        if (exists)
        {
            return;
        }

        Context.UserRoles.Add(new UserRole
        {
            UserId = userId,
            RoleId = roleId,
            AssignedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
        });
    }
}
