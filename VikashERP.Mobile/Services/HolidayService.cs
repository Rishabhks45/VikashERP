using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;
using VikashERP.Mobile.Services.Interfaces;

namespace VikashERP.Mobile.Services;

public class HolidayService : IHolidayService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HolidayService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<HolidayDto>> GetHolidaysAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var holidays = await client.GetFromJsonAsync<List<HolidayDto>>("api/Holidays");
            return holidays ?? new List<HolidayDto>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetHolidaysAsync exception: {ex.Message}");
            return new List<HolidayDto>();
        }
    }

    public async Task<HolidayDto?> CreateHolidayAsync(CreateHolidayDto dto)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("api/Holidays", dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<HolidayDto>();
            }
            return null;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"CreateHolidayAsync exception: {ex.Message}");
            return null;
        }
    }

    public async Task<HolidayDto?> UpdateHolidayAsync(Guid id, UpdateHolidayDto dto)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PutAsJsonAsync($"api/Holidays/{id}", dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<HolidayDto>();
            }
            return null;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"UpdateHolidayAsync exception: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteHolidayAsync(Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.DeleteAsync($"api/Holidays/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DeleteHolidayAsync exception: {ex.Message}");
            return false;
        }
    }
}
