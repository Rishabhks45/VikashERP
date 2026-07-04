using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;
using VikashERP.Mobile.Services.Interfaces;

namespace VikashERP.Mobile.Services;

public class ExpenseService : IExpenseService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ExpenseService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<ExpenseListDto>> GetExpensesAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var expenses = await client.GetFromJsonAsync<List<ExpenseListDto>>("api/Expenses");
            return expenses ?? new List<ExpenseListDto>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetExpensesAsync exception: {ex.Message}");
            return new List<ExpenseListDto>();
        }
    }

    public async Task<bool> RecordExpenseAsync(ExpenseFormModel model)
    {
        var dto = new 
        {
            ExpenseDate = (model.ExpenseDate ?? DateTime.Today).ToUniversalTime(),
            Category = model.Category,
            Amount = model.Amount,
            PaymentMode = model.PaymentMode,
            Remarks = model.Remarks
        };

        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.PostAsJsonAsync("api/Expenses", dto);
        
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        
        var errorBody = await response.Content.ReadAsStringAsync();
        throw new Exception($"API Error ({response.StatusCode}): {errorBody}");
    }

    public async Task<bool> UpdateExpenseAsync(Guid id, ExpenseFormModel model)
    {
        var dto = new 
        {
            ExpenseDate = (model.ExpenseDate ?? DateTime.Today).ToUniversalTime(),
            Category = model.Category,
            Amount = model.Amount,
            PaymentMode = model.PaymentMode,
            Remarks = model.Remarks
        };

        var client = _httpClientFactory.CreateClient("ApiClient");
        var response = await client.PutAsJsonAsync($"api/Expenses/{id}", dto);
        
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        
        var errorBody = await response.Content.ReadAsStringAsync();
        throw new Exception($"API Error ({response.StatusCode}): {errorBody}");
    }
}
