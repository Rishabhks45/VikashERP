using System.Net.Http.Json;
using VikashERP.Web.Models;

namespace VikashERP.Web.Services;

public interface ICustomerWebService
{
    Task<List<CustomerListDto>> GetCustomersAsync();
    Task<CustomerListDto?> CreateCustomerAsync(CreateCustomerDto dto);
    Task<bool> RecordCustomerPaymentAsync(CreateCustomerPaymentDto request);
    Task<List<RecentCustomerPaymentDto>> GetRecentPaymentsAsync();
    Task<List<CustomerLedgerEntryDto>> GetCustomerLedgerAsync(Guid id, DateTime? fromDate = null, DateTime? toDate = null);
}

public class CustomerWebService : ICustomerWebService
{
    private readonly HttpClient _httpClient;

    public CustomerWebService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("VikashERP.Api");
    }

    public async Task<List<CustomerListDto>> GetCustomersAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<CustomerListDto>>("api/customers") ?? new List<CustomerListDto>();
    }

    public async Task<CustomerListDto?> CreateCustomerAsync(CreateCustomerDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/customers", dto);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<CreateCustomerResponse>();
            return result?.Customer;
        }
        var error = await response.Content.ReadAsStringAsync();
        throw new Exception($"Failed to create customer: {error}");
    }

    public async Task<bool> RecordCustomerPaymentAsync(CreateCustomerPaymentDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/customers/payments", request);
        if (response.IsSuccessStatusCode)
            return true;

        try
        {
            var errObj = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            if (errObj != null && !string.IsNullOrEmpty(errObj.Message))
            {
                throw new Exception(errObj.Message);
            }
        }
        catch (Exception ex) when (ex is not Exception) // Only catch parsing exceptions
        {
            // Fallback
        }

        var errorText = await response.Content.ReadAsStringAsync();
        throw new Exception(string.IsNullOrWhiteSpace(errorText) ? $"HTTP {(int)response.StatusCode}" : errorText);
    }

    public async Task<List<RecentCustomerPaymentDto>> GetRecentPaymentsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<RecentCustomerPaymentDto>>("api/customers/payments/recent")
                ?? new List<RecentCustomerPaymentDto>();
        }
        catch
        {
            return new List<RecentCustomerPaymentDto>();
        }
    }

    public async Task<List<CustomerLedgerEntryDto>> GetCustomerLedgerAsync(Guid id, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var url = $"api/customers/{id}/ledger";
        var queryParams = new List<string>();
        if (fromDate.HasValue) queryParams.Add($"fromDate={fromDate.Value:yyyy-MM-dd}");
        if (toDate.HasValue) queryParams.Add($"toDate={toDate.Value:yyyy-MM-dd}");
        if (queryParams.Count > 0)
        {
            url += "?" + string.Join("&", queryParams);
        }
        return await _httpClient.GetFromJsonAsync<List<CustomerLedgerEntryDto>>(url) ?? new List<CustomerLedgerEntryDto>();
    }
}
