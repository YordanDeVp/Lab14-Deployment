namespace Lab11_Sapacayo_Yordan.Domain.Repositories;

public interface IUnitOfWorkTicketera
{
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    ITicketRepository Tickets { get; }
    IResponseRepository Responses { get; }
    IGenericRepository<T> Repository<T>() where T : class;
    Task<int> Complete(CancellationToken cancellationToken = default);
}
