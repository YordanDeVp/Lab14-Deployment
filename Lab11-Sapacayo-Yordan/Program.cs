using Lab11_Sapacayo_Yordan.Middleware;
using Lab11_Sapacayo_Yordan.Application.UseCases.Users.Queries;
using Lab11_Sapacayo_Yordan.Application.Mappings;
using Lab11_Sapacayo_Yordan.Infrastructure.Configuration;
using Lab11_Sapacayo_Yordan.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Configura ConnectionStrings:DefaultConnection.");

if (Uri.TryCreate(connectionString, UriKind.Absolute, out var databaseUri)
    && (databaseUri.Scheme == "postgres" || databaseUri.Scheme == "postgresql"))
{
    var credentials = databaseUri.GetComponents(UriComponents.UserInfo, UriFormat.UriEscaped).Split(':', 2);
    if (credentials.Length != 2)
    {
        throw new InvalidOperationException("La URL de PostgreSQL debe incluir usuario y contraseña.");
    }

    connectionString = new NpgsqlConnectionStringBuilder
    {
        Host = databaseUri.Host,
        Port = databaseUri.Port < 0 ? 5432 : databaseUri.Port,
        Database = Uri.UnescapeDataString(databaseUri.AbsolutePath.TrimStart('/')),
        Username = Uri.UnescapeDataString(credentials[0]),
        Password = Uri.UnescapeDataString(credentials[1]),
        SslMode = SslMode.Require
    }.ConnectionString;
}

builder.Services.AddInfrastructureServices(connectionString);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetUserQuery).Assembly));
builder.Services.AddAutoMapper(cfg => { }, typeof(TicketeraProfile).Assembly);
builder.Services.AddControllers();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo
{
    Title = "Lab11 - Ticketera",
    Version = "v1",
    Description = "API del Laboratorio 11 con CQRS y MediatR."
}));

var app = builder.Build();

if (builder.Configuration.GetValue<bool>("Database:ApplyMigrationsOnStartup"))
{
    await using var scope = app.Services.CreateAsyncScope();
    await scope.ServiceProvider.GetRequiredService<TicketeraContext>().Database.MigrateAsync();
}

app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Lab11 v1"));

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
app.Run();
