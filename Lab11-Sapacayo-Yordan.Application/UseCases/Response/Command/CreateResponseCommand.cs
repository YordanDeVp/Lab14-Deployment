using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Lab11_Sapacayo_Yordan.Application.Exceptions;
using ResponseEntity = Lab11_Sapacayo_Yordan.Domain.Entities.Response;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using MediatR;

namespace Lab11_Sapacayo_Yordan.Application.UseCases.Response.Command;

public record CreateResponseCommand : IRequest<Guid>
{
    public Guid TicketId { get; set; }
    public Guid ResponderId { get; set; }
    [Required]
    public string Message { get; set; } = null!;
}

internal sealed record CreateResponseCommandHandler : IRequestHandler<CreateResponseCommand, Guid>
{
    private readonly IUnitOfWorkTicketera _unitOfWork;
    private readonly IMapper _mapper;

    public CreateResponseCommandHandler(IUnitOfWorkTicketera unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateResponseCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId, cancellationToken)
            ?? throw new NotFoundException("Ticket no encontrado.");
        if (ticket.Status == "cerrado")
        {
            throw new ConflictException("El ticket está cerrado.");
        }
        if (await _unitOfWork.Users.GetByIdAsync(request.ResponderId, cancellationToken) is null)
        {
            throw new NotFoundException("Usuario no encontrado.");
        }

        var entity = _mapper.Map<ResponseEntity>(request);

        _unitOfWork.Repository<ResponseEntity>().AddEntity(entity);
        await _unitOfWork.Complete(cancellationToken);

        return entity.ResponseId;
    }
}
