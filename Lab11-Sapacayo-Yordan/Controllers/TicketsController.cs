using Lab11_Sapacayo_Yordan.Application.UseCases.Ticket.Command;
using Lab11_Sapacayo_Yordan.Application.UseCases.Ticket.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lab11_Sapacayo_Yordan.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await sender.Send(new GetTicketsQuery(), cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var ticket = await sender.Send(new GetTicketQuery { TicketId = id }, cancellationToken);
        return ticket is null ? NotFound() : Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTicketCommand command, CancellationToken cancellationToken)
    {
        var ticketId = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = ticketId }, new { ticketId });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateTicketCommand command, CancellationToken cancellationToken)
    {
        command.TicketId = id;
        return await sender.Send(command, cancellationToken) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        return await sender.Send(new DeleteTicketCommand { TicketId = id }, cancellationToken) ? NoContent() : NotFound();
    }
}
