using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VikashERP.Web.Models;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Services;

public class CategoryService : ICategoryService
{
    private readonly IHttpClientFactory _clientFactory;

    public CategoryService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<List<CategoryListDto>> GetCategoriesAsync()
    {
        try
        {
            var client = _clientFactory.CreateClient("VikashERP.Api");
            var response = await client.GetAsync("api/categories");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CategoryListDto>>() ?? new List<CategoryListDto>();
            }
        }
        catch { }
        return new List<CategoryListDto>();
    }

    public async Task<CategoryListDto?> GetCategoryByIdAsync(Guid id)
    {
        try
        {
            var client = _clientFactory.CreateClient("VikashERP.Api");
            var response = await client.GetAsync($"api/categories/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoryListDto>();
            }
        }
        catch { }
        return null;
    }

    public async Task<CategoryListDto?> CreateCategoryAsync(CategoryFormModel model)
    {
        try
        {
            var client = _clientFactory.CreateClient("VikashERP.Api");
            var response = await client.PostAsJsonAsync("api/categories", new 
            { 
                Name = model.Name, 
                IsActive = model.IsActive 
            });
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoryListDto>();
            }
        }
        catch { }
        return null;
    }

    public async Task<CategoryListDto?> UpdateCategoryAsync(Guid id, CategoryFormModel model)
    {
        try
        {
            var client = _clientFactory.CreateClient("VikashERP.Api");
            var response = await client.PutAsJsonAsync($"api/categories/{id}", new 
            { 
                Name = model.Name, 
                IsActive = model.IsActive 
            });
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoryListDto>();
            }
        }
        catch { }
        return null;
    }

    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
        try
        {
            var client = _clientFactory.CreateClient("VikashERP.Api");
            var response = await client.DeleteAsync($"api/categories/{id}");
            return response.IsSuccessStatusCode;
        }
        catch { }
        return false;
    }
}
