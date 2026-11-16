using Lab11_Sapacayo_Yordan.Application.UseCases.UseRole.Queries;
using Lab11_Sapacayo_Yordan.Application.UseCases.Users.Command;
using Lab11_Sapacayo_Yordan.Application.UseCases.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lab11_Sapacayo_Yordan.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userId = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = userId }, new { userId });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await sender.Send(new GetUsersQuery(), cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var user = await sender.Send(new GetUserQuery { UserId = id }, cancellationToken);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateUserCommand command, CancellationToken cancellationToken)
    {
        command.UserId = id;
        return await sender.Send(command, cancellationToken) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        return await sender.Send(new DeleteUserCommand { UserId = id }, cancellationToken) ? NoContent() : NotFound();
    }

    [HttpGet("{id:guid}/roles")]
    public async Task<IActionResult> GetRoles(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await sender.Send(new GetUserRolesQuery { UserId = id }, cancellationToken));
    }
}
