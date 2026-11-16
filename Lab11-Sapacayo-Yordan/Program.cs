using Lab11_Sapacayo_Yordan.Middleware;
using Lab11_Sapacayo_Yordan.Application.UseCases.Users.Queries;
using Lab11_Sapacayo_Yordan.Application.Mappings;
using Lab11_Sapacayo_Yordan.Infrastructure.Configuration;
using Lab11_Sapacayo_Yordan.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Configura ConnectionStrings:DefaultConnection.");

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
