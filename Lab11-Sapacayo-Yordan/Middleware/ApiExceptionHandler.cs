using Lab11_Sapacayo_Yordan.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Lab11_Sapacayo_Yordan.Middleware;

public sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var (status, title) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            ConflictException => (StatusCodes.Status409Conflict, exception.Message),
            DbUpdateConcurrencyException => (StatusCodes.Status409Conflict, "El registro fue modificado por otra operación."),
            DbUpdateException { InnerException: PostgresException { SqlState: "23505" } } =>
                (StatusCodes.Status409Conflict, "Ya existe un registro con esos datos."),
            DbUpdateException { InnerException: PostgresException { SqlState: "23503" } } =>
                (StatusCodes.Status409Conflict, "La operación afecta a un registro relacionado."),
            DbUpdateException { InnerException: PostgresException { SqlState: "23502" or "23514" or "22001" } } =>
                (StatusCodes.Status400BadRequest, "Los datos no cumplen las restricciones de la base de datos."),
            NpgsqlException or DbUpdateException { InnerException: NpgsqlException } =>
                (StatusCodes.Status503ServiceUnavailable, "No se pudo acceder a PostgreSQL. Revisa la conexión."),
            _ => (StatusCodes.Status500InternalServerError, "Ocurrió un error interno.")
        };

        if (status >= 500)
        {
            logger.LogError(exception, "Error al procesar la solicitud");
        }

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = status,
            Title = title
        }, options: null, contentType: "application/problem+json", cancellationToken: cancellationToken);
        return true;
    }
}
