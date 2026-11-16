using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Lab11_Sapacayo_Yordan.Application.Exceptions;
using RoleEntity = Lab11_Sapacayo_Yordan.Domain.Entities.Role;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using MediatR;

namespace Lab11_Sapacayo_Yordan.Application.UseCases.Role.Command;

public record CreateRoleCommand : IRequest<Guid>
{
    [Required, StringLength(50)]
    public string RoleName { get; set; } = null!;
}

internal sealed record CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Guid>
{
    private readonly IUnitOfWorkTicketera _unitOfWork;
    private readonly IMapper _mapper;

    public CreateRoleCommandHandler(IUnitOfWorkTicketera unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var roleName = request.RoleName.Trim();
        if (await _unitOfWork.Roles.NameExistsAsync(roleName, cancellationToken))
        {
            throw new ConflictException("El rol ya existe.");
        }

        var entity = _mapper.Map<RoleEntity>(request);

        _unitOfWork.Repository<RoleEntity>().AddEntity(entity);
        await _unitOfWork.Complete(cancellationToken);

        return entity.RoleId;
    }
}
