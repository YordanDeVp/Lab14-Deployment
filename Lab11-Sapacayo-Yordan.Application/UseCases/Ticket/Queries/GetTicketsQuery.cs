using TicketEntity = Lab11_Sapacayo_Yordan.Domain.Entities.Ticket;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using MediatR;

namespace Lab11_Sapacayo_Yordan.Application.UseCases.Ticket.Queries;

public record GetTicketsQuery : IRequest<IReadOnlyList<TicketEntity>>;

internal sealed record GetTicketsQueryHandler : IRequestHandler<GetTicketsQuery, IReadOnlyList<TicketEntity>>
{
    private readonly IUnitOfWorkTicketera _unitOfWork;

    public GetTicketsQueryHandler(IUnitOfWorkTicketera unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<IReadOnlyList<TicketEntity>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        return _unitOfWork.Tickets.GetAllAsync(cancellationToken);
    }
}
