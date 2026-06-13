using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using VikashERP.Web.Models;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Services;

public class CustomerShopService : ICustomerShopService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly IHttpClientFactory _httpClientFactory;

    public CustomerShopService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<CustomerShopResult> LoadMyShopAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("VikashERP.Api");
            var response = await client.GetAsync("api/customers/me", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return CustomerShopResult.NotLinkedResult("No customer profile is linked to your login. Contact admin to map your account.");

            if (!response.IsSuccessStatusCode)
                return CustomerShopResult.Fail(await ReadErrorAsync(response, cancellationToken) ?? "Could not load shop profile.");

            var profile = await response.Content.ReadFromJsonAsync<CustomerShopProfileModel>(JsonOptions, cancellationToken);
            if (profile is null)
                return CustomerShopResult.Fail("Customer profile response was empty.");

            return CustomerShopResult.Ok(profile);
        }
        catch (Exception ex)
        {
            return CustomerShopResult.Fail($"Failed to load shop profile: {ex.Message}");
        }
    }

    public async Task<CustomerShopResult> SaveMyShopAsync(CustomerShopSaveModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("VikashERP.Api");
            var response = await client.PutAsJsonAsync("api/customers/me", model, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return CustomerShopResult.NotLinkedResult("No customer profile is linked to your login. Contact admin to map your account.");

            if (!response.IsSuccessStatusCode)
                return CustomerShopResult.Fail(await ReadErrorAsync(response, cancellationToken) ?? "Failed to save shop profile.");

            var profile = await response.Content.ReadFromJsonAsync<CustomerShopProfileModel>(JsonOptions, cancellationToken);
            if (profile is null)
                return CustomerShopResult.Fail("Save succeeded but response was empty.");

            return CustomerShopResult.Ok(profile);
        }
        catch (Exception ex)
        {
            return CustomerShopResult.Fail($"Failed to save shop profile: {ex.Message}");
        }
    }

    private static async Task<string?> ReadErrorAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        try
        {
            var err = await response.Content.ReadFromJsonAsync<ApiErrorResponse>(JsonOptions, cancellationToken);
            return err?.Message;
        }
        catch
        {
            return null;
        }
    }

    private sealed class ApiErrorResponse
    {
        public string Message { get; set; } = string.Empty;
    }
}
