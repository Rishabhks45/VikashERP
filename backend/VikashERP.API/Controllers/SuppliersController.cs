using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikashERP.Application.Features.Suppliers.Commands;
using VikashERP.Application.Features.Suppliers.DTOs;
using VikashERP.Application.Features.Suppliers.Queries;

namespace VikashERP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuppliersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<SupplierListDto>>> GetSuppliers()
    {
        var result = await _mediator.Send(new GetSuppliersQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SupplierDto>> GetSupplierById(Guid id)
    {
        var result = await _mediator.Send(new GetSupplierByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<SupplierDto>> CreateSupplier([FromBody] CreateSupplierDto dto)
    {
        var result = await _mediator.Send(new CreateSupplierCommand(dto));
        if (result == null) return BadRequest("Could not create supplier. Duplicate name or GSTIN?");
        return CreatedAtAction(nameof(GetSupplierById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SupplierDto>> UpdateSupplier(Guid id, [FromBody] UpdateSupplierDto dto)
    {
        var result = await _mediator.Send(new UpdateSupplierCommand(id, dto));
        if (result == null) return NotFound("Supplier not found or duplicate details.");
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSupplier(Guid id)
    {
        var success = await _mediator.Send(new DeleteSupplierCommand(id));
        if (!success) return NotFound();
        return NoContent();
    }
}
