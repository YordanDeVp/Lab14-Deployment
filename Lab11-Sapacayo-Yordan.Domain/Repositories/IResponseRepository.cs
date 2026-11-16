using Lab11_Sapacayo_Yordan.Domain.Entities;

namespace Lab11_Sapacayo_Yordan.Domain.Repositories;

public interface IResponseRepository : IGenericRepository<Response>
{
    Task<IReadOnlyList<Response>> GetByTicketAsync(Guid ticketId, CancellationToken cancellationToken = default);
}
