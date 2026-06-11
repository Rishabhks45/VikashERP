using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VikashERP.Application.Features.Email.Commands;
using VikashERP.Application.Features.Email.DTOs;
using VikashERP.Application.Features.Email.Queries;

namespace VikashERP.API.Controllers;

[Route("api/email-templates")]
[ApiController]
public class EmailTemplatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmailTemplatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var templates = await _mediator.Send(new GetEmailTemplatesQuery(), cancellationToken);
        return Ok(templates);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var template = await _mediator.Send(new GetEmailTemplateByIdQuery(id), cancellationToken);
        return template is null ? NotFound() : Ok(template);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmailTemplateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var template = await _mediator.Send(new CreateEmailTemplateCommand { Request = request }, cancellationToken);
            return Ok(template);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { Message = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage)) });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmailTemplateRequest request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest(new { Message = "Route id does not match request id." });

        try
        {
            var template = await _mediator.Send(new UpdateEmailTemplateCommand { Request = request }, cancellationToken);
            return template is null ? NotFound() : Ok(template);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { Message = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage)) });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteEmailTemplateCommand { Id = id }, cancellationToken);
        return deleted ? Ok(new { Message = "Template deleted." }) : NotFound();
    }

    [HttpPost("preview")]
    public async Task<IActionResult> Preview([FromBody] PreviewEmailTemplateRequest request, CancellationToken cancellationToken)
    {
        var preview = await _mediator.Send(new PreviewEmailTemplateCommand { Request = request }, cancellationToken);
        return Ok(preview);
    }

    [HttpPost("send-test")]
    public async Task<IActionResult> SendTest([FromBody] SendTestEmailRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ToEmail))
            return BadRequest(new { Message = "Recipient email is required." });

        if (string.IsNullOrWhiteSpace(request.TemplateKey))
            return BadRequest(new { Message = "Template key is required." });

        var sent = await _mediator.Send(new SendTestEmailCommand
        {
            ToEmail = request.ToEmail.Trim(),
            TemplateKey = request.TemplateKey.Trim(),
            Tokens = request.Tokens ?? new Dictionary<string, string>()
        }, cancellationToken);

        if (!sent)
            return StatusCode(502, new { Message = "Email could not be sent. Check SendGrid settings and try again." });

        return Ok(new { Message = $"Test email sent to {request.ToEmail.Trim()}." });
    }
}

public class SendTestEmailRequest
{
    public string ToEmail { get; set; } = string.Empty;
    public string TemplateKey { get; set; } = string.Empty;
    public Dictionary<string, string>? Tokens { get; set; }
}
