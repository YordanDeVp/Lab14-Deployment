using Lab11_Sapacayo_Yordan.Application.Exceptions;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using MediatR;

namespace Lab11_Sapacayo_Yordan.Application.UseCases.UseRole.Command;

public record AssignRoleCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}

internal sealed record AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, Unit>
{
    private readonly IUnitOfWorkTicketera _unitOfWork;

    public AssignRoleCommandHandler(IUnitOfWorkTicketera unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken) is null)
        {
            throw new NotFoundException("Usuario no encontrado.");
        }
        if (await _unitOfWork.Roles.GetByIdAsync(request.RoleId, cancellationToken) is null)
        {
            throw new NotFoundException("Rol no encontrado.");
        }

        await _unitOfWork.Roles.AssignAsync(request.UserId, request.RoleId, cancellationToken);
        await _unitOfWork.Complete(cancellationToken);
        return Unit.Value;
    }
}
