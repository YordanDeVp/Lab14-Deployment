using Lab11_Sapacayo_Yordan.Application.UseCases.Role.Command;
using Lab11_Sapacayo_Yordan.Application.UseCases.Role.Queries;
using Lab11_Sapacayo_Yordan.Application.UseCases.UseRole.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lab11_Sapacayo_Yordan.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await sender.Send(new GetRolesQuery(), cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var roleId = await sender.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, new { roleId });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        command.RoleId = id;
        return await sender.Send(command, cancellationToken) ? NoContent() : NotFound();
    }

    [HttpPost("{roleId:guid}/users/{userId:guid}")]
    public async Task<IActionResult> Assign(Guid roleId, Guid userId, CancellationToken cancellationToken)
    {
        await sender.Send(new AssignRoleCommand { UserId = userId, RoleId = roleId }, cancellationToken);
        return NoContent();
    }
}
