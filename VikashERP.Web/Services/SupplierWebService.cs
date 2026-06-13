using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VikashERP.Web.Models;
using VikashERP.Web.Models.Forms;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Services;

public class SupplierWebService : ISupplierWebService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "api/Suppliers";

    public SupplierWebService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("VikashERP.Api");
    }

    public async Task<List<SupplierListDto>> GetSuppliersAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<SupplierListDto>>(BaseUrl) ?? new List<SupplierListDto>();
        }
        catch
        {
            return new List<SupplierListDto>();
        }
    }

    public async Task<SupplierDto?> GetSupplierByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<SupplierDto>($"{BaseUrl}/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<SupplierDto?> CreateSupplierAsync(SupplierFormModel model)
    {
        try
        {
            var dto = new CreateSupplierDto
            {
                Name = model.Name,
                CompanyName = model.CompanyName,
                Phone = model.Phone,
                Gstin = model.Gstin,
                Address = model.Address,
                OpeningBalance = model.OpeningBalance
            };
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SupplierDto>();
            }
        }
        catch {}
        return null;
    }

    public async Task<SupplierDto?> UpdateSupplierAsync(Guid id, SupplierFormModel model)
    {
        try
        {
            var dto = new UpdateSupplierDto
            {
                Name = model.Name,
                CompanyName = model.CompanyName,
                Phone = model.Phone,
                Gstin = model.Gstin,
                Address = model.Address
            };
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SupplierDto>();
            }
        }
        catch {}
        return null;
    }

    public async Task<bool> DeleteSupplierAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
        catch {}
        return false;
    }
}
