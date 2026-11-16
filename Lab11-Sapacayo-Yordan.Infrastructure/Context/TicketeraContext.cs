using Lab11_Sapacayo_Yordan.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lab11_Sapacayo_Yordan.Infrastructure.Context;

public class TicketeraContext(DbContextOptions<TicketeraContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Response> Responses => Set<Response>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(user => user.UserId);
            entity.Property(user => user.UserId).ValueGeneratedNever().HasColumnName("user_id");
            entity.Property(user => user.Username).HasColumnName("username").HasMaxLength(100).IsRequired();
            entity.HasIndex(user => user.Username).IsUnique();
            entity.Property(user => user.PasswordHash).HasColumnName("password_hash").HasMaxLength(255).IsRequired();
            entity.Property(user => user.Email).HasColumnName("email").HasMaxLength(150);
            entity.HasIndex(user => user.Email).IsUnique();
            entity.Property(user => user.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(role => role.RoleId);
            entity.Property(role => role.RoleId).ValueGeneratedNever().HasColumnName("role_id");
            entity.Property(role => role.RoleName).HasColumnName("role_name").HasMaxLength(50).IsRequired();
            entity.HasIndex(role => role.RoleName).IsUnique();
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("user_roles");
            entity.HasKey(userRole => new { userRole.UserId, userRole.RoleId });
            entity.Property(userRole => userRole.UserId).HasColumnName("user_id");
            entity.Property(userRole => userRole.RoleId).HasColumnName("role_id");
            entity.Property(userRole => userRole.AssignedAt).HasColumnName("assigned_at").HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");
            entity.HasOne<User>().WithMany().HasForeignKey(userRole => userRole.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<Role>().WithMany().HasForeignKey(userRole => userRole.RoleId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("tickets", table => table.HasCheckConstraint("tickets_status_check", "status IN ('abierto', 'en_proceso', 'cerrado')"));
            entity.HasKey(ticket => ticket.TicketId);
            entity.Property(ticket => ticket.TicketId).ValueGeneratedNever().HasColumnName("ticket_id");
            entity.Property(ticket => ticket.UserId).HasColumnName("user_id");
            entity.Property(ticket => ticket.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
            entity.Property(ticket => ticket.Description).HasColumnName("description");
            entity.Property(ticket => ticket.Status).HasColumnName("status").HasMaxLength(20).IsRequired();
            entity.Property(ticket => ticket.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");
            entity.Property(ticket => ticket.ClosedAt).HasColumnName("closed_at").HasColumnType("timestamp without time zone");
            entity.HasOne<User>().WithMany().HasForeignKey(ticket => ticket.UserId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Response>(entity =>
        {
            entity.ToTable("responses");
            entity.HasKey(response => response.ResponseId);
            entity.Property(response => response.ResponseId).ValueGeneratedNever().HasColumnName("response_id");
            entity.Property(response => response.TicketId).HasColumnName("ticket_id");
            entity.Property(response => response.ResponderId).HasColumnName("responder_id");
            entity.Property(response => response.Message).HasColumnName("message").IsRequired();
            entity.Property(response => response.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");
            entity.HasOne<Ticket>().WithMany().HasForeignKey(response => response.TicketId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<User>().WithMany().HasForeignKey(response => response.ResponderId).OnDelete(DeleteBehavior.NoAction);
        });
    }
}
