using Lab11_Sapacayo_Yordan.Application.UseCases.Response.Command;
using Lab11_Sapacayo_Yordan.Application.UseCases.Response.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lab11_Sapacayo_Yordan.Controllers;

[ApiController]
[Route("api/responses")]
public class ResponsesController(ISender sender) : ControllerBase
{
    [HttpGet("ticket/{ticketId:guid}")]
    public async Task<IActionResult> GetByTicket(Guid ticketId, CancellationToken cancellationToken)
    {
        return Ok(await sender.Send(new GetResponsesQuery { TicketId = ticketId }, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateResponseCommand command, CancellationToken cancellationToken)
    {
        var responseId = await sender.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, new { responseId });
    }
}
