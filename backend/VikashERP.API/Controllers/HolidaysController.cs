using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikashERP.Application.Features.Holidays.Commands;
using VikashERP.Application.Features.Holidays.DTOs;
using VikashERP.Application.Features.Holidays.Queries;

namespace VikashERP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HolidaysController : ControllerBase
{
    private readonly IMediator _mediator;

    public HolidaysController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HolidayDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetHolidaysQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<HolidayDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetHolidayByIdQuery(id), cancellationToken);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<HolidayDto>> Create([FromBody] CreateHolidayDto request, CancellationToken cancellationToken)
    {
        var command = new CreateHolidayCommand
        {
            Request = request,
            UserId = GetUserId()
        };

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateHolidayDto request, CancellationToken cancellationToken)
    {
        var command = new UpdateHolidayCommand
        {
            Id = id,
            Request = request,
            UserId = GetUserId()
        };

        var success = await _mediator.Send(command, cancellationToken);
        if (!success) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteHolidayCommand
        {
            Id = id,
            UserId = GetUserId()
        };

        var success = await _mediator.Send(command, cancellationToken);
        if (!success) return NotFound();

        return NoContent();
    }

    private Guid? GetUserId()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdString, out var userId))
            return userId;
        return null;
    }
}
