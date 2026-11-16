using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Lab11_Sapacayo_Yordan.Application.Exceptions;
using TicketEntity = Lab11_Sapacayo_Yordan.Domain.Entities.Ticket;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using MediatR;

namespace Lab11_Sapacayo_Yordan.Application.UseCases.Ticket.Command;

public record CreateTicketCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    [Required, StringLength(255)]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
}

internal sealed record CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Guid>
{
    private readonly IUnitOfWorkTicketera _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTicketCommandHandler(IUnitOfWorkTicketera unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken) is null)
        {
            throw new NotFoundException("Usuario no encontrado.");
        }

        var entity = _mapper.Map<TicketEntity>(request);

        _unitOfWork.Repository<TicketEntity>().AddEntity(entity);
        await _unitOfWork.Complete(cancellationToken);

        return entity.TicketId;
    }
}
