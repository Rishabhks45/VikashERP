using System.Net.Http.Json;
using VikashERP.Web.Models;

namespace VikashERP.Web.Services;

public interface ISalesWebService
{
    Task<List<InvoiceListDto>> GetInvoicesAsync();
    Task<InvoiceDetailDto?> GetInvoiceByIdAsync(Guid id);
    Task<Guid> CreateInvoiceAsync(CreateInvoiceModel model);
    Task<Guid> UpdateInvoiceAsync(Guid id, CreateInvoiceModel model);
    Task<bool> ApproveInvoiceAsync(Guid id);
    Task<bool> UpdateInvoicePaymentAsync(Guid id, decimal cashAmount, decimal bankAmount);
    Task<byte[]?> GetInvoicePdfAsync(Guid id);
}

public class SalesWebService : ISalesWebService
{
    private readonly HttpClient _httpClient;

    public SalesWebService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("VikashERP.Api");
    }

    public async Task<List<InvoiceListDto>> GetInvoicesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<InvoiceListDto>>("api/sales") ?? new List<InvoiceListDto>();
    }

    public async Task<InvoiceDetailDto?> GetInvoiceByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<InvoiceDetailDto>($"api/sales/{id}");
    }

    public async Task<Guid> CreateInvoiceAsync(CreateInvoiceModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("api/sales", model);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        return Guid.Parse(result!.GetProperty("id").GetString()!);
    }

    public async Task<Guid> UpdateInvoiceAsync(Guid id, CreateInvoiceModel model)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/sales/{id}", model);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        return Guid.Parse(result!.GetProperty("id").GetString()!);
    }

    public async Task<bool> ApproveInvoiceAsync(Guid id)
    {
        var response = await _httpClient.PostAsync($"api/sales/{id}/approve", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateInvoicePaymentAsync(Guid id, decimal cashAmount, decimal bankAmount)
    {
        var dto = new { CashAmount = cashAmount, BankAmount = bankAmount };
        var response = await _httpClient.PutAsJsonAsync($"api/sales/{id}/payment", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<byte[]?> GetInvoicePdfAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/sales/invoices/{id}/pdf");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsByteArrayAsync();
        }
        return null;
    }
}
