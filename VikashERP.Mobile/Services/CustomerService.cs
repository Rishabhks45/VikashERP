using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;
using VikashERP.Mobile.Services.Interfaces;

namespace VikashERP.Mobile.Services;

public class CustomerService : ICustomerService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CustomerService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<CustomerListDto>> GetAllAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var customers = await client.GetFromJsonAsync<List<CustomerListDto>>("api/customers");
            return customers ?? new List<CustomerListDto>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetAllAsync exception: {ex.Message}");
            return new List<CustomerListDto>();
        }
    }

    public async Task<CustomerListDto?> GetByIdAsync(Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            return await client.GetFromJsonAsync<CustomerListDto>($"api/customers/{id}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetByIdAsync exception: {ex.Message}");
            return null;
        }
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> CreateAsync(CustomerFormModel model)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("api/customers", model);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Error {response.StatusCode}: {error}");
            }
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, $"Exception: {ex.Message}");
        }
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> UpdateAsync(Guid id, CustomerFormModel model)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PutAsJsonAsync($"api/customers/{id}", model);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Error {response.StatusCode}: {error}");
            }
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, $"Exception: {ex.Message}");
        }
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> DeleteAsync(Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.DeleteAsync($"api/customers/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Error {response.StatusCode}: {error}");
            }
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, $"Exception: {ex.Message}");
        }
    }
    public async Task<List<RecentCustomerPaymentDto>> GetRecentPaymentsAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var txs = await client.GetFromJsonAsync<List<RecentCustomerPaymentDto>>("api/customers/payments/recent");
            return txs ?? new List<RecentCustomerPaymentDto>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetRecentPaymentsAsync exception: {ex.Message}");
            return new List<RecentCustomerPaymentDto>();
        }
    }

    public async Task<bool> RecordCustomerPaymentAsync(CustomerPaymentFormModel model)
    {
        if (model.SelectedCustomer == null) return false;

        var dto = new 
        {
            CustomerId = model.SelectedCustomer.Id,
            Amount = model.Amount,
            PaymentMode = model.PaymentMode,
            Remarks = model.Remarks,
            PaymentDate = (model.PaymentDate ?? DateTime.Today).ToUniversalTime()
        };

        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.PostAsJsonAsync("api/customers/payments", dto);
        
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        
        var errorBody = await response.Content.ReadAsStringAsync();
        throw new Exception($"API Error ({response.StatusCode}): {errorBody}");
    }
}
