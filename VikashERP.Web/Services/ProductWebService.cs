using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VikashERP.Web.Models;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Services;

public class ProductWebService : IProductWebService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "api/Products";

    public ProductWebService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("VikashERP.Api");
    }

    public async Task<List<ProductListDto>> GetProductsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ProductListDto>>(BaseUrl) ?? new List<ProductListDto>();
    }

    public async Task<List<ProductDto>> GetProductsWithVariantsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ProductDto>>($"{BaseUrl}/with-variants") ?? new List<ProductDto>();
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ProductDto>($"{BaseUrl}/{id}");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<ProductDto?> CreateProductAsync(CreateProductDto request)
    {
        var response = await _httpClient.PostAsJsonAsync(BaseUrl, request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ProductDto>();
        }
        return null;
    }

    public async Task<ProductDto?> UpdateProductAsync(Guid id, UpdateProductDto request)
    {
        var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ProductDto>();
        }
        return null;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        return response.IsSuccessStatusCode;
    }
}
