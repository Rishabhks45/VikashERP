using System.Net.Http.Json;
using System.Net.Http.Headers;
using VikashERP.Mobile.Models;
using VikashERP.Mobile.Services.Interfaces;
using VikashERP.Mobile.State;

namespace VikashERP.Mobile.Services;

public class UserProfileService : IUserProfileService
{
    private readonly HttpClient _httpClient;
    private readonly AppStateService _appStateService;

    public UserProfileService(HttpClient httpClient, AppStateService appStateService)
    {
        _httpClient = httpClient;
        _appStateService = appStateService;
    }

    private async Task SetAuthorizationHeader()
    {
        var token = await _appStateService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<UserAccountDto?> GetProfileAsync(Guid userId)
    {
        await SetAuthorizationHeader();
        return await _httpClient.GetFromJsonAsync<UserAccountDto>($"api/users/{userId}");
    }

    public async Task<bool> UpdateProfileAsync(Guid userId, UpdateUserAccountDto profile)
    {
        try
        {
            await SetAuthorizationHeader();
            var response = await _httpClient.PutAsJsonAsync($"api/users/{userId}", profile);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
