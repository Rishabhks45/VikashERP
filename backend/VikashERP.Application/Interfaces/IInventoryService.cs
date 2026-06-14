using VikashERP.Application.Features.Inventory.DTOs;

namespace VikashERP.Application.Interfaces;

public interface IInventoryService
{
    Task<List<GodownStockDto>> GetGodownStockAsync(CancellationToken cancellationToken);
}
