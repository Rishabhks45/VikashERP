using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;
using VikashERP.Mobile.Services.Interfaces;

namespace VikashERP.Mobile.Services;

public class InventoryService : IInventoryService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public InventoryService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<GodownStockDto>> GetGodownStockAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var stockList = await client.GetFromJsonAsync<List<GodownStockDto>>("api/inventory/stock");
            return stockList ?? new List<GodownStockDto>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetGodownStockAsync exception: {ex.Message}");
            return new List<GodownStockDto>();
        }
    }
}
