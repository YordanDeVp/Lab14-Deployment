using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Lab11_Sapacayo_Yordan.Application.Exceptions;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using MediatR;

namespace Lab11_Sapacayo_Yordan.Application.UseCases.Users.Command;

public record UpdateUserCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    [Required, StringLength(100)]
    public string Username { get; set; } = null!;
    [Required, StringLength(255)]
    public string PasswordHash { get; set; } = null!;
    [EmailAddress, StringLength(150)]
    public string? Email { get; set; }
}

internal sealed record UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUnitOfWorkTicketera _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUnitOfWorkTicketera unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return false;
        }

        var username = request.Username.Trim();
        var email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim();
        var existingUser = await _unitOfWork.Users.GetByUsernameAsync(username, cancellationToken);
        if (existingUser is not null && existingUser.UserId != request.UserId)
        {
            throw new ConflictException("El usuario ya existe.");
        }
        if (email is not null && await _unitOfWork.Users.EmailExistsAsync(email, request.UserId, cancellationToken))
        {
            throw new ConflictException("El correo ya existe.");
        }

        _mapper.Map(request, user);

        await _unitOfWork.Complete(cancellationToken);

        return true;
    }
}
