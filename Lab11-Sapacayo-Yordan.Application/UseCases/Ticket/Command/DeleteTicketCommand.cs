using Lab11_Sapacayo_Yordan.Domain.Repositories;
using MediatR;

namespace Lab11_Sapacayo_Yordan.Application.UseCases.Ticket.Command;

public record DeleteTicketCommand : IRequest<bool>
{
    public Guid TicketId { get; set; }
}

internal sealed record DeleteTicketCommandHandler : IRequestHandler<DeleteTicketCommand, bool>
{
    private readonly IUnitOfWorkTicketera _unitOfWork;

    public DeleteTicketCommandHandler(IUnitOfWorkTicketera unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId, cancellationToken);
        if (ticket is null)
        {
            return false;
        }

        await _unitOfWork.Tickets.DeleteAsync(ticket, cancellationToken);
        await _unitOfWork.Complete(cancellationToken);

        return true;
    }
}
