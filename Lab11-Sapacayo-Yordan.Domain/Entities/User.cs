namespace Lab11_Sapacayo_Yordan.Domain.Entities;

public class User
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string? Email { get; set; }
    public DateTime? CreatedAt { get; set; }
}