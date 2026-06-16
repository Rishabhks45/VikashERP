using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VikashERP.Web.Models;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Services;

public class HolidayWebService : IHolidayWebService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "api/Holidays";

    public HolidayWebService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("VikashERP.Api");
    }

    public async Task<List<HolidayDto>> GetHolidaysAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<HolidayDto>>(BaseUrl) ?? new List<HolidayDto>();
        }
        catch
        {
            return new List<HolidayDto>();
        }
    }

    public async Task<HolidayDto?> GetHolidayByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<HolidayDto>($"{BaseUrl}/{id}");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<HolidayDto?> CreateHolidayAsync(CreateHolidayDto request)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseUrl, request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<HolidayDto>();
        }
        return null;
    }

    public async Task<bool> UpdateHolidayAsync(Guid id, UpdateHolidayDto request)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteHolidayAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        return response.IsSuccessStatusCode;
    }
}
