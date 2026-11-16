using Lab11_Sapacayo_Yordan.Domain.Repositories;
using Lab11_Sapacayo_Yordan.Infrastructure.Context;

namespace Lab11_Sapacayo_Yordan.Infrastructure.Adapters.Repositories;

public class UnitOfWorkTicketera : IUnitOfWorkTicketera
{
    private readonly TicketeraContext _context;

    public IUserRepository Users { get; }
    public IRoleRepository Roles { get; }
    public ITicketRepository Tickets { get; }
    public IResponseRepository Responses { get; }

    public UnitOfWorkTicketera(TicketeraContext context, IUserRepository users,
        IRoleRepository roles, ITicketRepository tickets, IResponseRepository responses)
    {
        _context = context;
        Users = users;
        Roles = roles;
        Tickets = tickets;
        Responses = responses;
    }

    public IGenericRepository<T> Repository<T>() where T : class
    {
        return new GenericRepository<T>(_context);
    }

    public Task<int> Complete(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
