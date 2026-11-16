using Lab11_Sapacayo_Yordan.Application.Exceptions;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using MediatR;

namespace Lab11_Sapacayo_Yordan.Application.UseCases.UseRole.Queries;

public record GetUserRolesQuery : IRequest<IReadOnlyList<string>>
{
    public Guid UserId { get; set; }
}

internal sealed record GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, IReadOnlyList<string>>
{
    private readonly IUnitOfWorkTicketera _unitOfWork;

    public GetUserRolesQueryHandler(IUnitOfWorkTicketera unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<string>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken) is null)
        {
            throw new NotFoundException("Usuario no encontrado.");
        }

        return await _unitOfWork.Users.GetRolesAsync(request.UserId, cancellationToken);
    }
}
