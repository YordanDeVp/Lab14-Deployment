using Lab11_Sapacayo_Yordan.Application.Exceptions;
using ResponseEntity = Lab11_Sapacayo_Yordan.Domain.Entities.Response;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using MediatR;

namespace Lab11_Sapacayo_Yordan.Application.UseCases.Response.Queries;

public record GetResponsesQuery : IRequest<IReadOnlyList<ResponseEntity>>
{
    public Guid TicketId { get; set; }
}

internal sealed record GetResponsesQueryHandler : IRequestHandler<GetResponsesQuery, IReadOnlyList<ResponseEntity>>
{
    private readonly IUnitOfWorkTicketera _unitOfWork;

    public GetResponsesQueryHandler(IUnitOfWorkTicketera unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ResponseEntity>> Handle(GetResponsesQuery request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Tickets.GetByIdAsync(request.TicketId, cancellationToken) is null)
        {
            throw new NotFoundException("Ticket no encontrado.");
        }

        return await _unitOfWork.Responses.GetByTicketAsync(request.TicketId, cancellationToken);
    }
}
