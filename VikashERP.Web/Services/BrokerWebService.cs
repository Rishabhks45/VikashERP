using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VikashERP.Web.Models.Brokers;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Services;

public class BrokerWebService : IBrokerWebService
{
    private readonly HttpClient _httpClient;

    public BrokerWebService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("VikashERP.Api");
    }

    public async Task<List<BrokerListDto>> GetBrokersAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<BrokerListDto>>("api/Brokers") ?? new List<BrokerListDto>();
    }

    public async Task<BrokerDto?> GetBrokerByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<BrokerDto>($"api/Brokers/{id}");
    }

    public async Task<BrokerDto?> CreateBrokerAsync(BrokerFormModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Brokers", model);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BrokerDto>();
    }

    public async Task<BrokerDto?> UpdateBrokerAsync(Guid id, BrokerFormModel model)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Brokers/{id}", model);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BrokerDto>();
    }

    public async Task<bool> DeleteBrokerAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/Brokers/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<BrokerLedgerEntryDto>> GetBrokerLedgerAsync(Guid brokerId)
    {
        return await _httpClient.GetFromJsonAsync<List<BrokerLedgerEntryDto>>($"api/Brokers/{brokerId}/ledger") ?? new List<BrokerLedgerEntryDto>();
    }

    public async Task<bool> RecordBrokerTransactionAsync(CreateBrokerTransactionDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Brokers/transactions", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<RecentBrokerTransactionDto>> GetRecentBrokerTransactionsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<RecentBrokerTransactionDto>>("api/Brokers/transactions/recent") ?? new List<RecentBrokerTransactionDto>();
    }
}

