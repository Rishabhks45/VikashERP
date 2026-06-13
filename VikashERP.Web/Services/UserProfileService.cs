using System;
using System.Net.Http.Json;

using VikashERP.Web.Models;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IHttpClientFactory _clientFactory;

    public UserProfileService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public event Action<UserProfileDto>? OnProfileUpdated;

    public async Task<UserAccountDto?> GetProfileAsync(Guid userId)
    {
        try
        {
            var client = _clientFactory.CreateClient("VikashERP.Api");
            return await client.GetFromJsonAsync<UserAccountDto>($"api/users/{userId}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdateProfileAsync(Guid userId, UpdateUserAccountDto profile)
    {
        try
        {
            var client = _clientFactory.CreateClient("VikashERP.Api");
            var response = await client.PutAsJsonAsync($"api/users/{userId}", profile);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public void NotifyProfileUpdated(UserProfileDto profile)
    {
        OnProfileUpdated?.Invoke(profile);
    }
}
