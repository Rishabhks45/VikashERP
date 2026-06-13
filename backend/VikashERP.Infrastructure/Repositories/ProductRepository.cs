using Microsoft.EntityFrameworkCore;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Infrastructure.Data;

namespace VikashERP.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Variants.Where(v => !v.IsDeleted))
            .Where(p => !p.IsDeleted)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Variants.Where(v => !v.IsDeleted))
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, cancellationToken);
    }

    public async Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower() && !p.IsDeleted, cancellationToken);
    }

    public Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Add(product);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(product);
        return Task.CompletedTask;
    }

    public Task AddVariantsAsync(IEnumerable<ProductVariant> variants, CancellationToken cancellationToken = default)
    {
        _context.ProductVariants.AddRange(variants);
        return Task.CompletedTask;
    }

    public Task UpdateVariantsAsync(IEnumerable<ProductVariant> variants, CancellationToken cancellationToken = default)
    {
        _context.ProductVariants.UpdateRange(variants);
        return Task.CompletedTask;
    }

    public Task DeleteVariantsAsync(IEnumerable<ProductVariant> variants, CancellationToken cancellationToken = default)
    {
        _context.ProductVariants.UpdateRange(variants); // Since it's soft delete, we just update them
        return Task.CompletedTask;
    }
}
