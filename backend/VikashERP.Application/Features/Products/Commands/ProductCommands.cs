using MediatR;
using VikashERP.Application.Features.Products.DTOs;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Products.Commands;

public class CreateProductCommand : IRequest<ProductDto>
{
    public CreateProductDto Request { get; set; } = null!;
    public Guid? UserId { get; set; }
}

public class UpdateProductCommand : IRequest<ProductDto?>
{
    public Guid Id { get; set; }
    public UpdateProductDto Request { get; set; } = null!;
    public Guid? UserId { get; set; }
}

public class DeleteProductCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductService _service;

    public CreateProductCommandHandler(IProductService service)
    {
        _service = service;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        return await _service.CreateAsync(request.Request, request.UserId, cancellationToken);
    }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto?>
{
    private readonly IProductService _service;

    public UpdateProductCommandHandler(IProductService service)
    {
        _service = service;
    }

    public async Task<ProductDto?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        return await _service.UpdateAsync(request.Id, request.Request, request.UserId, cancellationToken);
    }
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductService _service;

    public DeleteProductCommandHandler(IProductService service)
    {
        _service = service;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        return await _service.DeleteAsync(request.Id, request.UserId, cancellationToken);
    }
}
