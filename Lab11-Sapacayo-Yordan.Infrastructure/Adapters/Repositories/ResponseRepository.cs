using Lab11_Sapacayo_Yordan.Domain.Entities;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using Lab11_Sapacayo_Yordan.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Lab11_Sapacayo_Yordan.Infrastructure.Adapters.Repositories;

public class ResponseRepository(TicketeraContext context) : GenericRepository<Response>(context), IResponseRepository
{
    public async Task<IReadOnlyList<Response>> GetByTicketAsync(Guid ticketId, CancellationToken cancellationToken = default)
    {
        return await Context.Responses.AsNoTracking()
            .Where(response => response.TicketId == ticketId)
            .OrderBy(response => response.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
