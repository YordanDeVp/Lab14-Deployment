using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Lab11_Sapacayo_Yordan.Application.Exceptions;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using MediatR;

namespace Lab11_Sapacayo_Yordan.Application.UseCases.Role.Command;

public record UpdateRoleCommand : IRequest<bool>
{
    public Guid RoleId { get; set; }
    [Required, StringLength(50)]
    public string RoleName { get; set; } = null!;
}

internal sealed record UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, bool>
{
    private readonly IUnitOfWorkTicketera _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateRoleCommandHandler(IUnitOfWorkTicketera unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId, cancellationToken);
        if (role is null)
        {
            return false;
        }

        var roleName = request.RoleName.Trim();
        if (role.RoleName != roleName && await _unitOfWork.Roles.NameExistsAsync(roleName, cancellationToken))
        {
            throw new ConflictException("El rol ya existe.");
        }

        _mapper.Map(request, role);
        await _unitOfWork.Complete(cancellationToken);

        return true;
    }
}
