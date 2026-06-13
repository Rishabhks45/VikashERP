using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Web.Models;

namespace VikashERP.Web.Services.Interfaces;

public interface IProductWebService
{
    Task<List<ProductListDto>> GetProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(Guid id);
    Task<ProductDto?> CreateProductAsync(CreateProductDto request);
    Task<ProductDto?> UpdateProductAsync(Guid id, UpdateProductDto request);
    Task<bool> DeleteProductAsync(Guid id);
}
