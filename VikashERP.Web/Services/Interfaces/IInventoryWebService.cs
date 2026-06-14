using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Web.Models;

namespace VikashERP.Web.Services.Interfaces;

public interface IInventoryWebService
{
    Task<List<GodownStockDto>> GetGodownStockAsync();
}
