using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using MediatR;

namespace Lab11_Sapacayo_Yordan.Application.UseCases.Ticket.Command;

public record UpdateTicketCommand : IRequest<bool>
{
    public Guid TicketId { get; set; }
    [Required, StringLength(255)]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    [Required, RegularExpression("^(abierto|en_proceso|cerrado)$")]
    public string Status { get; set; } = null!;
}

internal sealed record UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand, bool>
{
    private readonly IUnitOfWorkTicketera _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTicketCommandHandler(IUnitOfWorkTicketera unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId, cancellationToken);
        if (ticket is null)
        {
            return false;
        }

        var statusChanged = ticket.Status != request.Status;
        _mapper.Map(request, ticket);
        if (statusChanged)
        {
            ticket.ClosedAt = request.Status == "cerrado"
                ? DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
                : null;
        }

        await _unitOfWork.Complete(cancellationToken);
        return true;
    }
}
