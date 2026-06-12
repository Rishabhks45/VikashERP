using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikashERP.Application.Features.Organization.Commands;
using VikashERP.Application.Features.Organization.DTOs;
using VikashERP.Application.Features.Organization.Queries;

namespace VikashERP.API.Controllers;

[Route("api/organization")]
[ApiController]
public class OrganizationController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrganizationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Public website branding & contact (login page, footer, SEO).</summary>
    [HttpGet("public")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublic(CancellationToken cancellationToken)
    {
        var organization = await _mediator.Send(new GetPublicOrganizationQuery(), cancellationToken);
        return Ok(organization);
    }

    /// <summary>Full organization settings for admin configuration.</summary>
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var organization = await _mediator.Send(new GetOrganizationQuery(), cancellationToken);
        return Ok(organization);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateOrganizationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var organization = await _mediator.Send(new UpdateOrganizationCommand { Request = request }, cancellationToken);
            return organization is null ? NotFound() : Ok(organization);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { Message = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage)) });
        }
    }
}
