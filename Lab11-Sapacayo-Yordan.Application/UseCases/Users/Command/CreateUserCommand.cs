using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Lab11_Sapacayo_Yordan.Application.Exceptions;
using Lab11_Sapacayo_Yordan.Domain.Entities;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using MediatR;

namespace Lab11_Sapacayo_Yordan.Application.UseCases.Users.Command;

public record CreateUserCommand : IRequest<Guid>
{
    [Required, StringLength(100)]
    public string Username { get; set; } = null!;
    [Required, StringLength(255)]
    public string PasswordHash { get; set; } = null!;
    [EmailAddress, StringLength(150)]
    public string? Email { get; set; }
}

internal sealed record CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUnitOfWorkTicketera _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUnitOfWorkTicketera unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var username = request.Username.Trim();
        var email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim();
        if (await _unitOfWork.Users.GetByUsernameAsync(username, cancellationToken) is not null)
        {
            throw new ConflictException("El usuario ya existe.");
        }
        if (email is not null && await _unitOfWork.Users.EmailExistsAsync(email, cancellationToken: cancellationToken))
        {
            throw new ConflictException("El correo ya existe.");
        }

        var entity = _mapper.Map<User>(request);

        _unitOfWork.Repository<User>().AddEntity(entity);
        await _unitOfWork.Complete(cancellationToken);

        return entity.UserId;
    }
}
