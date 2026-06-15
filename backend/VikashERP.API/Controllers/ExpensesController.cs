using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikashERP.Application.Features.Expenses.Commands;
using VikashERP.Application.Features.Expenses.DTOs;
using VikashERP.Application.Features.Expenses.Queries;

namespace VikashERP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpensesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExpenseListDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetExpensesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExpenseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetExpenseByIdQuery(id), cancellationToken);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> Create([FromBody] CreateExpenseDto request, CancellationToken cancellationToken)
    {
        var command = new CreateExpenseCommand
        {
            Request = request,
            UserId = GetUserId()
        };

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateExpenseDto request, CancellationToken cancellationToken)
    {
        var command = new UpdateExpenseCommand
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
        var command = new DeleteExpenseCommand
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
