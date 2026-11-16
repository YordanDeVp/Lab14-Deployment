using RoleEntity = Lab11_Sapacayo_Yordan.Domain.Entities.Role;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using MediatR;

namespace Lab11_Sapacayo_Yordan.Application.UseCases.Role.Queries;

public record GetRolesQuery : IRequest<IReadOnlyList<RoleEntity>>;

internal sealed record GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IReadOnlyList<RoleEntity>>
{
    private readonly IUnitOfWorkTicketera _unitOfWork;

    public GetRolesQueryHandler(IUnitOfWorkTicketera unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<IReadOnlyList<RoleEntity>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return _unitOfWork.Roles.GetAllAsync(cancellationToken);
    }
}
