using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;
using VikashERP.Mobile.Services.Interfaces;

namespace VikashERP.Mobile.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OrganizationService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<OrganizationResponse?> GetOrganizationAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetFromJsonAsync<OrganizationResponse>("api/organization");
            return response;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetOrganizationAsync exception: {ex.Message}");
            return null;
        }
    }

    public async Task<OrganizationResponse?> UpdateOrganizationAsync(OrganizationFormModel model)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var result = await client.PutAsJsonAsync("api/organization", model);
            
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<OrganizationResponse>();
            }
            
            return null;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"UpdateOrganizationAsync exception: {ex.Message}");
            return null;
        }
    }
}
