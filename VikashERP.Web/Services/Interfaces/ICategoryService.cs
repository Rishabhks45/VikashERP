using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Web.Models;

namespace VikashERP.Web.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryListDto>> GetCategoriesAsync();
    Task<CategoryListDto?> GetCategoryByIdAsync(Guid id);
    Task<CategoryListDto?> CreateCategoryAsync(CategoryFormModel model);
    Task<CategoryListDto?> UpdateCategoryAsync(Guid id, CategoryFormModel model);
    Task<bool> DeleteCategoryAsync(Guid id);
}
