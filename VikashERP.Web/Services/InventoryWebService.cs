using System.Net.Http.Json;
using VikashERP.Web.Models;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Services;

public class InventoryWebService : IInventoryWebService
{
    private readonly HttpClient _httpClient;

    public InventoryWebService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("VikashERP.Api");
    }

    public async Task<List<GodownStockDto>> GetGodownStockAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/inventory/stock");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<GodownStockDto>>();
                return result ?? new List<GodownStockDto>();
            }
            return new List<GodownStockDto>();
        }
        catch
        {
            return new List<GodownStockDto>();
        }
    }
}
