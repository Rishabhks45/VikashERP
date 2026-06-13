using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikashERP.Application.Features.Purchases.Commands;
using VikashERP.Application.Features.Purchases.DTOs;
using VikashERP.Application.Features.Purchases.Queries;

namespace VikashERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Added Authorize if authentication is configured
public class PurchasesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PurchasesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPurchaseEntries()
    {
        var entries = await _mediator.Send(new GetPurchaseEntriesQuery());
        return Ok(entries);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPurchaseEntryById(Guid id)
    {
        var entry = await _mediator.Send(new GetPurchaseEntryByIdQuery(id));
        if (entry == null) return NotFound();
        return Ok(entry);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePurchaseEntry([FromBody] CreatePurchaseEntryDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _mediator.Send(new CreatePurchaseEntryCommand(dto));
        return CreatedAtAction(nameof(GetPurchaseEntryById), new { id = result }, result);
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> ApprovePurchaseEntry(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new ApprovePurchaseEntryCommand(id));
            if (!result) return BadRequest("Failed to approve purchase entry.");
            return Ok(new { Message = "Purchase Entry Approved Successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
