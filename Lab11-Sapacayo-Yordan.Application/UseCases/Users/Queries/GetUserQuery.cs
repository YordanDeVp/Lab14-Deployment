using AutoMapper;
using Lab11_Sapacayo_Yordan.Application.Dtos;
using Lab11_Sapacayo_Yordan.Domain.Repositories;
using MediatR;

namespace Lab11_Sapacayo_Yordan.Application.UseCases.Users.Queries;

public record GetUserQuery : IRequest<UserDto?>
{
    public Guid UserId { get; set; }
}

internal sealed record GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto?>
{
    private readonly IUnitOfWorkTicketera _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IUnitOfWorkTicketera unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        return user is null
            ? null
            : _mapper.Map<UserDto>(user);
    }
}
