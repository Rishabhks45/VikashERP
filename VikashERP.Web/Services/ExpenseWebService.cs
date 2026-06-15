using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VikashERP.Web.Models;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Services;

public class ExpenseWebService : IExpenseWebService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "api/Expenses";

    public ExpenseWebService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("VikashERP.Api");
    }

    public async Task<List<ExpenseListDto>> GetExpensesAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<ExpenseListDto>>(BaseUrl) ?? new List<ExpenseListDto>();
        }
        catch
        {
            return new List<ExpenseListDto>();
        }
    }

    public async Task<ExpenseDto?> GetExpenseByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ExpenseDto>($"{BaseUrl}/{id}");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<ExpenseDto?> CreateExpenseAsync(CreateExpenseDto request)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseUrl, request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ExpenseDto>();
        }
        return null;
    }

    public async Task<bool> UpdateExpenseAsync(Guid id, UpdateExpenseDto request)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteExpenseAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        return response.IsSuccessStatusCode;
    }
}
