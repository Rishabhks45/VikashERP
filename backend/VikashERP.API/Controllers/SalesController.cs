using MediatR;
using Microsoft.AspNetCore.Mvc;
using VikashERP.Application.Features.Sales.Commands;
using VikashERP.Application.Features.Sales.DTOs;
using VikashERP.Application.Features.Sales.Queries;

namespace VikashERP.API.Controllers;

[Route("api/sales")]
[ApiController]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetInvoices(CancellationToken cancellationToken)
    {
        var query = new GetInvoicesQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetInvoiceById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetInvoiceByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateInvoiceCommand(dto);
        var invoiceId = await _mediator.Send(command, cancellationToken);
        return Ok(new { Id = invoiceId });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateInvoice(Guid id, [FromBody] CreateInvoiceDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateInvoiceCommand(id, dto);
        var invoiceId = await _mediator.Send(command, cancellationToken);
        return Ok(new { Id = invoiceId });
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> ApproveInvoice(Guid id, CancellationToken cancellationToken)
    {
        var command = new ApproveInvoiceCommand(id);
        var success = await _mediator.Send(command, cancellationToken);
        if (!success) return BadRequest(new { Message = "Failed to approve invoice." });
        return Ok();
    }
}
