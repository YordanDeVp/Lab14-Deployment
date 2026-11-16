using Lab11_Sapacayo_Yordan.Domain.Entities;

namespace Lab11_Sapacayo_Yordan.Domain.Repositories;

public interface ITicketRepository : IGenericRepository<Ticket>
{
    Task<IReadOnlyList<Ticket>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
