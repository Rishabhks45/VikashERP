using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VikashERP.Web.Models;

namespace VikashERP.Web.Services;

public class SalaryConfigurationService : ISalaryConfigurationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SalaryConfigurationService> _logger;

    public SalaryConfigurationService(IHttpClientFactory httpClientFactory, ILogger<SalaryConfigurationService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("VikashERP.Api");
        _logger = logger;
    }

    public async Task<List<SalaryConfigDto>> GetConfigurationsAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<SalaryConfigDto>>("api/salary-configuration");
            return response ?? new List<SalaryConfigDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching salary configurations");
            return new List<SalaryConfigDto>();
        }
    }

    public async Task<List<RoleUserDto>> GetUsersByRoleAsync(string roleName)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<RoleUserDto>>($"api/salary-configuration/users-by-role/{Uri.EscapeDataString(roleName)}");
            return response ?? new List<RoleUserDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching users by role");
            return new List<RoleUserDto>();
        }
    }

    public async Task<bool> CreateConfigurationAsync(CreateSalaryConfigDto request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/salary-configuration", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating salary configuration");
            return false;
        }
    }

    public async Task<bool> UpdateConfigurationAsync(Guid id, UpdateSalaryConfigDto request)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/salary-configuration/{id}", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating salary configuration");
            return false;
        }
    }

    public async Task<bool> DeleteConfigurationAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/salary-configuration/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting salary configuration");
            return false;
        }
    }
}
