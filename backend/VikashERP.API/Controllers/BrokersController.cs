using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VikashERP.Application.Features.Brokers.Commands;
using VikashERP.Application.Features.Brokers.DTOs;
using VikashERP.Application.Features.Brokers.Queries;

namespace VikashERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrokersController : ControllerBase
{
    private readonly IMediator _mediator;

    public BrokersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetBrokers()
    {
        var brokers = await _mediator.Send(new GetBrokersQuery());
        return Ok(brokers);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBroker(Guid id)
    {
        var broker = await _mediator.Send(new GetBrokerByIdQuery(id));
        if (broker == null)
            return NotFound();

        return Ok(broker);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBroker([FromBody] CreateBrokerDto brokerDto)
    {
        try
        {
            var result = await _mediator.Send(new CreateBrokerCommand(brokerDto));
            return CreatedAtAction(nameof(GetBroker), new { id = result!.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBroker(Guid id, [FromBody] UpdateBrokerDto brokerDto)
    {
        try
        {
            var result = await _mediator.Send(new UpdateBrokerCommand(id, brokerDto));
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBroker(Guid id)
    {
        var result = await _mediator.Send(new DeleteBrokerCommand(id));
        if (!result)
            return NotFound();

        return NoContent();
    }
}
