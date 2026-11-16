namespace Lab11_Sapacayo_Yordan.Domain.Entities;

public class UserRole
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public DateTime? AssignedAt { get; set; }
}