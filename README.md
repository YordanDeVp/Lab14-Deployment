# Laboratorio 11 Ticketera con CQRS

API en .NET 10 que utiliza la base de datos del Laboratorio 10 y MediatR para separar comandos y consultas. La API funciona sin login, JWT ni autorización.

## Estructura

- **Domain:** entidades y contratos de repositorio.
- **Application:** solicitudes `public record : IRequest<T>` y handlers `internal sealed record : IRequestHandler<TRequest, TResponse>` en el mismo archivo dentro de `UseCases`, separados en `Command` y `Queries`. AutoMapper convierte las solicitudes en entidades y los usuarios en DTO.
- **Infrastructure:** `TicketeraContext`, configuración de PostgreSQL, repositorios EF Core y `UnitOfWorkTicketera`. `Scaffolding` conserva el modelo generado como referencia; el contexto activo es `TicketeraContext`.
- **API:** controladores que envían las solicitudes con MediatR, manejo de errores y Swagger UI.

## Configurar PostgreSQL

Se utiliza el esquema existente de `TicketeraBD`, con las tablas `users`, `roles`, `user_roles`, `tickets` y `responses`. La aplicación no crea ni modifica tablas al iniciar.

Guarda la cadena completa de conexión en User Secrets, reemplazando `TU_PASSWORD` por tu contraseña local:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=TicketeraBD;Username=postgres;Password=TU_PASSWORD;SSL Mode=Prefer" --project .\Lab11-Sapacayo-Yordan
```

## Ejecutar y probar con Swagger

```powershell
dotnet build .\Lab11-Sapacayo-Yordan.sln
dotnet run --project .\Lab11-Sapacayo-Yordan --launch-profile http
```

Swagger está disponible en <http://localhost:5257/swagger>. Los perfiles `http` y `https` tienen `launchBrowser: true` y `launchUrl: swagger` para que Rider o Visual Studio abran Swagger al ejecutar. La ruta `/` también redirige a Swagger.

Desde la terminal, abre esa dirección en el navegador. Usa **Try it out** para enviar las solicitudes. Crea primero un usuario y utiliza su identificador al crear un ticket o una respuesta. Los comandos de usuario conservan el campo `PasswordHash` del esquema existente; las consultas de usuarios omiten ese campo.

Las rutas principales son `/api/users`, `/api/roles`, `/api/tickets` y `/api/responses`. Las respuestas de error usan `400` para datos inválidos, `404` para registros inexistentes y `409` para conflictos de negocio.

## Observaciones y conclusiones

- Cada operación tiene su propia solicitud y handler, lo que permite aplicar responsabilidad única dentro de Application.
- Los comandos mapean las solicitudes con `IMapper`, agregan entidades con `Repository<T>().AddEntity(...)` y guardan los cambios con `IUnitOfWorkTicketera.Complete(...)`, siguiendo la estructura del ejemplo del profesor. Las consultas obtienen información sin modificarla.
- Los controladores se comunican con MediatR y la infraestructura concentra el acceso a PostgreSQL, manteniendo separadas las responsabilidades de cada capa.
