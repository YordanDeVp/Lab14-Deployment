namespace Lab11_Sapacayo_Yordan.Application.Dtos;

public record UserDto(Guid UserId, string Username, string? Email, DateTime? CreatedAt);