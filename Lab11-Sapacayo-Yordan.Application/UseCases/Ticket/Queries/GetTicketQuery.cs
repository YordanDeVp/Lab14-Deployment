using TicketEntity = Lab11_Sapacayo_Yordan.Domain.Entities.Ticket;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using MediatR;

namespace Lab11_Sapacayo_Yordan.Application.UseCases.Ticket.Queries;

public record GetTicketQuery : IRequest<TicketEntity?>
{
    public Guid TicketId { get; set; }
}

internal sealed record GetTicketQueryHandler : IRequestHandler<GetTicketQuery, TicketEntity?>
{
    private readonly IUnitOfWorkTicketera _unitOfWork;

    public GetTicketQueryHandler(IUnitOfWorkTicketera unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<TicketEntity?> Handle(GetTicketQuery request, CancellationToken cancellationToken)
    {
        return _unitOfWork.Tickets.GetByIdAsync(request.TicketId, cancellationToken);
    }
}
