using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;
using VikashERP.Mobile.Services.Interfaces;

namespace VikashERP.Mobile.Services;

public class SalesService : ISalesService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SalesService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<InvoiceListDto>> GetInvoicesAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var invoices = await client.GetFromJsonAsync<List<InvoiceListDto>>("api/sales");
            return invoices ?? new List<InvoiceListDto>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetInvoicesAsync exception: {ex.Message}");
            return new List<InvoiceListDto>();
        }
    }

    public async Task<InvoiceDetailDto?> GetInvoiceByIdAsync(Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            return await client.GetFromJsonAsync<InvoiceDetailDto>($"api/sales/{id}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetInvoiceByIdAsync exception: {ex.Message}");
            return null;
        }
    }

    public async Task<byte[]?> GetInvoicePdfAsync(Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync($"api/sales/{id}/pdf");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            return null;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetInvoicePdfAsync exception: {ex.Message}");
            return null;
        }
    }

    public async Task<(bool IsSuccess, string ErrorMessage, Guid? InvoiceId)> CreateInvoiceAsync(CreateInvoiceModel invoice)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("api/sales", invoice);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
                Guid? newId = null;
                if (result.TryGetProperty("id", out var idProp) || result.TryGetProperty("Id", out idProp))
                {
                    if (Guid.TryParse(idProp.GetString(), out var parsedId))
                        newId = parsedId;
                }
                return (true, string.Empty, newId);
            }
            else
            {
                var err = await response.Content.ReadAsStringAsync();
                return (false, err ?? "Failed to create invoice", null);
            }
        }
        catch (Exception ex)
        {
            return (false, ex.Message, null);
        }
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> ApproveInvoiceAsync(Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsync($"api/sales/{id}/approve", null);
            if (response.IsSuccessStatusCode)
            {
                return (true, string.Empty);
            }
            var err = await response.Content.ReadAsStringAsync();
            return (false, err ?? "Failed to approve invoice");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}

