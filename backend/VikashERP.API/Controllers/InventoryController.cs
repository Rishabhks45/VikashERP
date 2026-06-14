using MediatR;
using Microsoft.AspNetCore.Mvc;
using VikashERP.Application.Features.Inventory.DTOs;
using VikashERP.Application.Features.Inventory.Queries;

namespace VikashERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("stock")]
    public async Task<ActionResult<List<GodownStockDto>>> GetGodownStock()
    {
        var result = await _mediator.Send(new GetGodownStockQuery());
        return Ok(result);
    }
}
