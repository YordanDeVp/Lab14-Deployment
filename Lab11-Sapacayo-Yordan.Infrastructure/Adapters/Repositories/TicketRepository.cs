using Lab11_Sapacayo_Yordan.Domain.Entities;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using Lab11_Sapacayo_Yordan.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Lab11_Sapacayo_Yordan.Infrastructure.Adapters.Repositories;

public class TicketRepository(TicketeraContext context) : GenericRepository<Ticket>(context), ITicketRepository
{
    public async Task<IReadOnlyList<Ticket>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await Context.Tickets.AsNoTracking()
            .Where(ticket => ticket.UserId == userId)
            .OrderByDescending(ticket => ticket.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
