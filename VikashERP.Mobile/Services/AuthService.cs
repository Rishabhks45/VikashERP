using VikashERP.Mobile.Models;
using VikashERP.Mobile.Services.Interfaces;
using VikashERP.Mobile.State;
using System.Net.Http.Json;

namespace VikashERP.Mobile.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AppStateService _appStateService;

    public AuthService(HttpClient httpClient, AppStateService appStateService)
    {
        _httpClient = httpClient;
        _appStateService = appStateService;
    }

    public async Task<LoginResponse?> LoginAsync(string email, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>(new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    await _appStateService.SetTokenAsync(result.Token);
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

    public async Task LogoutAsync()
    {
        await _appStateService.RemoveTokenAsync();
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
