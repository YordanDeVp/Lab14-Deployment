using Lab11_Sapacayo_Yordan.Application.Exceptions;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using MediatR;

namespace Lab11_Sapacayo_Yordan.Application.UseCases.Users.Command;

public record DeleteUserCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
}

internal sealed record DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUnitOfWorkTicketera _unitOfWork;

    public DeleteUserCommandHandler(IUnitOfWorkTicketera unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return false;
        }

        if (await _unitOfWork.Users.HasActivityAsync(request.UserId, cancellationToken))
        {
            throw new ConflictException("No se puede eliminar un usuario con tickets o respuestas.");
        }

        await _unitOfWork.Users.DeleteAsync(user, cancellationToken);
        await _unitOfWork.Complete(cancellationToken);

        return true;
    }
}
