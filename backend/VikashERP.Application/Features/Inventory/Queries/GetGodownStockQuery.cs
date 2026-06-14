using MediatR;
using VikashERP.Application.Features.Inventory.DTOs;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Inventory.Queries;

public record GetGodownStockQuery : IRequest<List<GodownStockDto>>;

public class GetGodownStockQueryHandler : IRequestHandler<GetGodownStockQuery, List<GodownStockDto>>
{
    private readonly IInventoryService _inventoryService;

    public GetGodownStockQueryHandler(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public async Task<List<GodownStockDto>> Handle(GetGodownStockQuery request, CancellationToken cancellationToken)
    {
        return await _inventoryService.GetGodownStockAsync(cancellationToken);
    }
}
