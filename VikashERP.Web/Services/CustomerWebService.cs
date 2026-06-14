using System.Net.Http.Json;
using VikashERP.Web.Models;

namespace VikashERP.Web.Services;

public interface ICustomerWebService
{
    Task<List<CustomerListDto>> GetCustomersAsync();
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
}
