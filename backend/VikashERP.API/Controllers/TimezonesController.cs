using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikashERP.Application.Features.Timezones.DTOs;
using VikashERP.Application.Features.Timezones.Queries;

namespace VikashERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class TimezonesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TimezonesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<TimezoneDto>>> GetActiveTimezones(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetActiveTimezonesQuery(), cancellationToken);
        return Ok(result);
    }
}
