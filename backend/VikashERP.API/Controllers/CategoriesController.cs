using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VikashERP.Application.Features.Categories.Commands;
using VikashERP.Application.Features.Categories.DTOs;
using VikashERP.Application.Features.Categories.Queries;

namespace VikashERP.API.Controllers;

[Route("api/categories")]
[ApiController]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var categories = await _mediator.Send(new GetCategoriesQuery(), cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return category == null ? NotFound() : Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _mediator.Send(new CreateCategoryCommand 
            { 
                Request = dto, 
                UserId = GetUserId() 
            }, cancellationToken);
            
            return Ok(category);


        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _mediator.Send(new UpdateCategoryCommand 
            { 
                Id = id, 
                Request = dto, 
                UserId = GetUserId() 
            }, cancellationToken);
            
            return category == null ? NotFound() : Ok(category);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteCategoryCommand 
        { 
            Id = id, 
            UserId = GetUserId() 
        }, cancellationToken);
        
        return deleted ? Ok(new { Message = "Category deleted." }) : NotFound();
    }

    private Guid? GetUserId()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdString, out var userId))
            return userId;
        return null;
    }
}
