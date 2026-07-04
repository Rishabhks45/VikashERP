using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;
using VikashERP.Mobile.Services.Interfaces;

namespace VikashERP.Mobile.Services;

public class BrokerService : IBrokerService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BrokerService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<BrokerListDto>> GetBrokersAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var brokers = await client.GetFromJsonAsync<List<BrokerListDto>>("api/Brokers");
            return brokers ?? new List<BrokerListDto>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetBrokersAsync exception: {ex.Message}");
            return new List<BrokerListDto>();
        }
    }

    public async Task<List<RecentBrokerTransactionDto>> GetRecentTransactionsAsync(int count = 50)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var txs = await client.GetFromJsonAsync<List<RecentBrokerTransactionDto>>("api/Brokers/transactions/recent");
            return txs ?? new List<RecentBrokerTransactionDto>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetRecentTransactionsAsync exception: {ex.Message}");
            return new List<RecentBrokerTransactionDto>();
        }
    }

    public async Task<BrokerTransactionResponseDto?> RecordTransactionAsync(BrokerTransactionFormModel model)
    {
        try
        {
            if (model.SelectedBroker == null) return null;

            var dto = new 
            {
                BrokerId = model.SelectedBroker.Id,
                Amount = model.Amount,
                TransactionType = model.TransactionType,
                PaymentMode = model.TransactionType == "Payment" ? model.PaymentMode : "N/A",
                Remarks = model.Remarks,
                TransactionDate = model.TransactionDate ?? DateTime.Today
            };

            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("api/Brokers/transactions", dto);
            
            if (response.IsSuccessStatusCode)
            {
                return new BrokerTransactionResponseDto { BrokerId = model.SelectedBroker.Id };
            }
            
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"API Error ({response.StatusCode}): {errorBody}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"RecordTransactionAsync exception: {ex.Message}");
            throw;
        }
    }
}
