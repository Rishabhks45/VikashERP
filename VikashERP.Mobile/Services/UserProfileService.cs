using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using VikashERP.Mobile.Models;
using VikashERP.Mobile.Services.Interfaces;
using VikashERP.Mobile.State;

namespace VikashERP.Mobile.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UserProfileService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<UserAccountDto?> GetProfileAsync(Guid userId)
    {
        var client = _httpClientFactory.CreateClient("ApiClient");
        return await client.GetFromJsonAsync<UserAccountDto>($"api/users/{userId}");
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> UpdateProfileAsync(Guid userId, UpdateUserAccountDto profile)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PutAsJsonAsync($"api/users/{userId}", profile);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"API Error: {response.StatusCode} - {error}");
            }
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, $"Exception: {ex.Message}");
        }
    }
}
