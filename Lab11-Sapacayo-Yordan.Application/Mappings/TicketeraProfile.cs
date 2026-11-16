using AutoMapper;
using Lab11_Sapacayo_Yordan.Application.Dtos;
using Lab11_Sapacayo_Yordan.Application.UseCases.Response.Command;
using Lab11_Sapacayo_Yordan.Application.UseCases.Role.Command;
using Lab11_Sapacayo_Yordan.Application.UseCases.Ticket.Command;
using Lab11_Sapacayo_Yordan.Application.UseCases.Users.Command;
using Lab11_Sapacayo_Yordan.Domain.Entities;

namespace Lab11_Sapacayo_Yordan.Application.Mappings;

public class TicketeraProfile : Profile
{
    public TicketeraProfile()
    {
        CreateMap<CreateUserCommand, User>()
            .ForMember(user => user.UserId, option => option.MapFrom(_ => Guid.NewGuid()))
            .ForMember(user => user.Username, option => option.MapFrom(request => request.Username.Trim()))
            .ForMember(user => user.Email, option => option.MapFrom(request =>
                string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim()))
            .ForMember(user => user.CreatedAt, option => option.Ignore());

        CreateMap<UpdateUserCommand, User>()
            .ForMember(user => user.Username, option => option.MapFrom(request => request.Username.Trim()))
            .ForMember(user => user.Email, option => option.MapFrom(request =>
                string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim()))
            .ForMember(user => user.CreatedAt, option => option.Ignore());

        CreateMap<User, UserDto>();

        CreateMap<CreateRoleCommand, Role>()
            .ForMember(role => role.RoleId, option => option.MapFrom(_ => Guid.NewGuid()))
            .ForMember(role => role.RoleName, option => option.MapFrom(request => request.RoleName.Trim()));

        CreateMap<UpdateRoleCommand, Role>()
            .ForMember(role => role.RoleName, option => option.MapFrom(request => request.RoleName.Trim()));

        CreateMap<CreateTicketCommand, Ticket>()
            .ForMember(ticket => ticket.TicketId, option => option.MapFrom(_ => Guid.NewGuid()))
            .ForMember(ticket => ticket.Title, option => option.MapFrom(request => request.Title.Trim()))
            .ForMember(ticket => ticket.Status, option => option.MapFrom(_ => "abierto"))
            .ForMember(ticket => ticket.CreatedAt, option => option.Ignore())
            .ForMember(ticket => ticket.ClosedAt, option => option.Ignore());

        CreateMap<UpdateTicketCommand, Ticket>()
            .ForMember(ticket => ticket.Title, option => option.MapFrom(request => request.Title.Trim()))
            .ForMember(ticket => ticket.UserId, option => option.Ignore())
            .ForMember(ticket => ticket.CreatedAt, option => option.Ignore())
            .ForMember(ticket => ticket.ClosedAt, option => option.Ignore());

        CreateMap<CreateResponseCommand, Response>()
            .ForMember(response => response.ResponseId, option => option.MapFrom(_ => Guid.NewGuid()))
            .ForMember(response => response.Message, option => option.MapFrom(request => request.Message.Trim()))
            .ForMember(response => response.CreatedAt, option => option.Ignore());
    }
}
