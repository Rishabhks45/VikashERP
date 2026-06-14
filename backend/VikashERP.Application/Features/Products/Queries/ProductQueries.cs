using MediatR;
using VikashERP.Application.Features.Products.DTOs;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Products.Queries;

public class GetProductsQuery : IRequest<IReadOnlyList<ProductListDto>>
{
}

public class GetProductsWithVariantsQuery : IRequest<IReadOnlyList<ProductDto>>
{
}

public class GetProductByIdQuery : IRequest<ProductDto?>
{
    public Guid Id { get; set; }
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IReadOnlyList<ProductListDto>>
{
    private readonly IProductService _service;

    public GetProductsQueryHandler(IProductService service)
    {
        _service = service;
    }

    public async Task<IReadOnlyList<ProductListDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetAllAsync(cancellationToken);
    }
}

public class GetProductsWithVariantsQueryHandler : IRequestHandler<GetProductsWithVariantsQuery, IReadOnlyList<ProductDto>>
{
    private readonly IProductService _service;

    public GetProductsWithVariantsQueryHandler(IProductService service)
    {
        _service = service;
    }

    public async Task<IReadOnlyList<ProductDto>> Handle(GetProductsWithVariantsQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetAllWithVariantsAsync(cancellationToken);
    }
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductService _service;

    public GetProductByIdQueryHandler(IProductService service)
    {
        _service = service;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetByIdAsync(request.Id, cancellationToken);
    }
}
