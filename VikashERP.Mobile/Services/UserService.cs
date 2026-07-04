using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;
using VikashERP.Mobile.Services.Interfaces;

namespace VikashERP.Mobile.Services;

public class UserService : IUserService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UserService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<UserAccountDto>> GetAllAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var users = await client.GetFromJsonAsync<List<UserAccountDto>>("api/users");
            return users ?? new List<UserAccountDto>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetAllAsync exception: {ex.Message}");
            return new List<UserAccountDto>();
        }
    }

    public async Task<UserAccountDto?> GetByIdAsync(Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            return await client.GetFromJsonAsync<UserAccountDto>($"api/users/{id}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetByIdAsync exception: {ex.Message}");
            return null;
        }
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> CreateAsync(CreateUserAccountDto dto)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("api/users", dto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Error {response.StatusCode}: {error}");
            }
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, $"Exception: {ex.Message}");
        }
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> UpdateAsync(Guid id, UpdateUserAccountDto dto)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PutAsJsonAsync($"api/users/{id}", dto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Error {response.StatusCode}: {error}");
            }
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, $"Exception: {ex.Message}");
        }
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> DeleteAsync(Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.DeleteAsync($"api/users/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Error {response.StatusCode}: {error}");
            }
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, $"Exception: {ex.Message}");
        }
    }
}
