using VikashERP.Mobile.Models;
using VikashERP.Mobile.Services.Interfaces;
using VikashERP.Mobile.State;
using System.Net.Http;
using System.Net.Http.Json;

namespace VikashERP.Mobile.Services;

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppStateService _appStateService;

    public AuthService(IHttpClientFactory httpClientFactory, AppStateService appStateService)
    {
        _httpClientFactory = httpClientFactory;
        _appStateService = appStateService;
    }

    public async Task<LoginResponse?> LoginAsync(string email, string password)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("AuthClient");
            var response = await client.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    await _appStateService.SetTokenAsync(result.Token);
                    if (!string.IsNullOrEmpty(result.RefreshToken))
                    {
                        await _appStateService.SetRefreshTokenAsync(result.RefreshToken);
                    }
                    return result;
                }
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> RefreshTokenAsync()
    {
        try
        {
            var accessToken = await _appStateService.GetTokenAsync();
            var refreshToken = await _appStateService.GetRefreshTokenAsync();

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                return false;
            }

            var requestBody = new 
            { 
                AccessToken = accessToken, 
                RefreshToken = refreshToken 
            };

            var client = _httpClientFactory.CreateClient("AuthClient");
            // Also need to pass the old access token in header just in case backend expects it
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.PostAsJsonAsync("api/auth/refresh-token", requestBody);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (result != null && !string.IsNullOrEmpty(result.Token) && !string.IsNullOrEmpty(result.RefreshToken))
                {
                    await _appStateService.SetTokenAsync(result.Token);
                    await _appStateService.SetRefreshTokenAsync(result.RefreshToken);
                    return true;
                }
            }
            
            // If we fail to refresh (e.g. 401 on refresh endpoint, meaning refresh token is expired)
            await LogoutAsync();
            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        await _appStateService.RemoveTokenAsync();
        await _appStateService.RemoveRefreshTokenAsync();
        SecureStorage.Default.Remove("user_pin");
        SecureStorage.Default.Remove("biometric_enabled");
    }

    public async Task<bool> HasPinSetupAsync()
    {
        var pin = await SecureStorage.Default.GetAsync("user_pin");
        return !string.IsNullOrEmpty(pin);
    }

    public async Task SavePinAsync(string pin)
    {
        await SecureStorage.Default.SetAsync("user_pin", pin);
    }

    public async Task<bool> VerifyPinAsync(string pin)
    {
        var savedPin = await SecureStorage.Default.GetAsync("user_pin");
        return pin == savedPin;
    }

    public async Task EnableBiometricAsync(bool enable)
    {
        if (enable)
        {
            await SecureStorage.Default.SetAsync("biometric_enabled", "true");
        }
        else
        {
            SecureStorage.Default.Remove("biometric_enabled");
        }
    }

    public async Task<bool> IsBiometricEnabledAsync()
    {
        var enabled = await SecureStorage.Default.GetAsync("biometric_enabled");
        return enabled == "true";
    }
}
