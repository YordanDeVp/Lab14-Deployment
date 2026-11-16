using Lab11_Sapacayo_Yordan.Domain.Repositories;
using Lab11_Sapacayo_Yordan.Infrastructure.Adapters.Repositories;
using Lab11_Sapacayo_Yordan.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lab11_Sapacayo_Yordan.Infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TicketeraContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IResponseRepository, ResponseRepository>();
        services.AddScoped<IUnitOfWorkTicketera, UnitOfWorkTicketera>();

        return services;
    }
}
