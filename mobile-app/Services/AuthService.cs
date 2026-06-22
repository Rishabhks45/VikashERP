using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using MobileApp.Models;

namespace MobileApp.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    
    public bool IsLoggedIn { get; private set; }
    public string? AccessToken { get; private set; }
    public string? UserRole { get; private set; }
    public string? UserName { get; private set; }

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> InitializeAsync()
    {
        try
        {
            var token = await SecureStorage.Default.GetAsync("access_token");
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            AccessToken = token;
            UserName = Preferences.Default.Get<string?>("user_name", null);
            UserRole = Preferences.Default.Get<string?>("user_role", null);
            IsLoggedIn = true;

            // Set default auth header
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return true;
        }
        catch
        {
            // SecureStorage might fail on some platforms if not configured correctly (like emulator without lock screen set)
            return false;
        }
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var request = new UserLoginRequest
            {
                Email = email,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var authResult = await response.Content.ReadFromJsonAsync<UserLoginResponse>();
            if (authResult == null || string.IsNullOrEmpty(authResult.Token))
            {
                return false;
            }

            // Save credentials
            await SecureStorage.Default.SetAsync("access_token", authResult.Token);
            await SecureStorage.Default.SetAsync("refresh_token", authResult.RefreshToken);
            Preferences.Default.Set("user_name", authResult.UserName);
            Preferences.Default.Set("user_role", authResult.Role);

            AccessToken = authResult.Token;
            UserName = authResult.UserName;
            UserRole = authResult.Role;
            IsLoggedIn = true;

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResult.Token);
            return true;
        }
        catch (Exception)
        {
            // Log or handle exceptions
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            SecureStorage.Default.Remove("access_token");
            SecureStorage.Default.Remove("refresh_token");
            Preferences.Default.Remove("user_name");
            Preferences.Default.Remove("user_role");
        }
        catch { }

        AccessToken = null;
        UserName = null;
        UserRole = null;
        IsLoggedIn = false;
        _httpClient.DefaultRequestHeaders.Authorization = null;

        await Task.CompletedTask;
    }
}
