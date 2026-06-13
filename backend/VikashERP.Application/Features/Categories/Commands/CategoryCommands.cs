using MediatR;
using VikashERP.Application.Features.Categories.DTOs;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Categories.Commands;

public class CreateCategoryCommand : IRequest<CategoryDto>
{
    public CreateCategoryDto Request { get; set; } = null!;
    public Guid? UserId { get; set; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly ICategoryService _categoryService;

    public CreateCategoryCommandHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken) =>
        _categoryService.CreateAsync(request.Request, request.UserId, cancellationToken);
}

public class UpdateCategoryCommand : IRequest<CategoryDto?>
{
    public Guid Id { get; set; }
    public UpdateCategoryDto Request { get; set; } = null!;
    public Guid? UserId { get; set; }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto?>
{
    private readonly ICategoryService _categoryService;

    public UpdateCategoryCommandHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public Task<CategoryDto?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken) =>
        _categoryService.UpdateAsync(request.Id, request.Request, request.UserId, cancellationToken);
}

public class DeleteCategoryCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
}

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoryService _categoryService;

    public DeleteCategoryCommandHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken) =>
        _categoryService.DeleteAsync(request.Id, request.UserId, cancellationToken);
}
