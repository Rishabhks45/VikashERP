using MediatR;
using VikashERP.Application.Features.Categories.DTOs;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Categories.Queries;

public record GetCategoriesQuery : IRequest<IReadOnlyList<CategoryListDto>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IReadOnlyList<CategoryListDto>>
{
    private readonly ICategoryService _categoryService;

    public GetCategoriesQueryHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public Task<IReadOnlyList<CategoryListDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken) =>
        _categoryService.GetAllAsync(cancellationToken);
}

public record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryDto?>;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    private readonly ICategoryService _categoryService;

    public GetCategoryByIdQueryHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken) =>
        _categoryService.GetByIdAsync(request.Id, cancellationToken);
}
