using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;

namespace VikashERP.Mobile.Services.Interfaces;

public interface IInventoryService
{
    Task<List<GodownStockDto>> GetGodownStockAsync();
    Task<List<ProductDto>> GetProductsAsync();
}
