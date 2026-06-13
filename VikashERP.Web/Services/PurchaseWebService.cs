using System.Net.Http.Json;
using VikashERP.Web.Models;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Services;

public class PurchaseWebService : IPurchaseWebService
{
    private readonly HttpClient _httpClient;

    public PurchaseWebService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("VikashERP.Api");
    }

    public async Task<List<PurchaseEntryDto>> GetPurchaseEntriesAsync()
    {
        try
        {
            var entries = await _httpClient.GetFromJsonAsync<List<PurchaseEntryDto>>("api/Purchases");
            return entries ?? new List<PurchaseEntryDto>();
        }
        catch
        {
            return new List<PurchaseEntryDto>();
        }
    }

    public async Task<PurchaseEntryDto?> GetPurchaseEntryByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<PurchaseEntryDto>($"api/Purchases/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<Guid> CreatePurchaseEntryAsync(CreatePurchaseEntryDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Purchases", dto);
        response.EnsureSuccessStatusCode();
        var id = await response.Content.ReadFromJsonAsync<Guid>();
        return id;
    }

    public async Task<bool> ApprovePurchaseEntryAsync(Guid id)
    {
        var response = await _httpClient.PostAsync($"api/Purchases/{id}/approve", null);
        return response.IsSuccessStatusCode;
    }
}

