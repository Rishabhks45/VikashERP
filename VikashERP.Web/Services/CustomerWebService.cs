using System.Net.Http.Json;
using VikashERP.Web.Models;

namespace VikashERP.Web.Services;

public interface ICustomerWebService
{
    Task<List<CustomerListDto>> GetCustomersAsync();
    Task<CustomerListDto?> CreateCustomerAsync(CreateCustomerDto dto);
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
}
