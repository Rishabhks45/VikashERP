using VikashERP.Domain.Entities;

namespace VikashERP.Application.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task AddVariantsAsync(IEnumerable<ProductVariant> variants, CancellationToken cancellationToken = default);
    Task UpdateVariantsAsync(IEnumerable<ProductVariant> variants, CancellationToken cancellationToken = default);
    Task DeleteVariantsAsync(IEnumerable<ProductVariant> variants, CancellationToken cancellationToken = default);
}
