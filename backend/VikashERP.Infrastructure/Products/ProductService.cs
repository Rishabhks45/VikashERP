using VikashERP.Application.Features.Products.DTOs;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;

namespace VikashERP.Infrastructure.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IProductRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ProductListDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await _repository.GetAllAsync(cancellationToken);
        return products.Select(p => new ProductListDto
        {
            Id = p.Id,
            Name = p.Name,
            CategoryName = p.Category?.Name ?? "Unknown",
            HsnCode = p.HsnCode,
            TotalVariants = p.Variants?.Count(v => !v.IsDeleted) ?? 0,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt
        }).ToList();
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllWithVariantsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _repository.GetAllAsync(cancellationToken);
        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name ?? "Unknown",
            Name = p.Name,
            HsnCode = p.HsnCode,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            Variants = p.Variants.Where(v => !v.IsDeleted).Select(v => new ProductVariantDto
            {
                Id = v.Id,
                ProductId = v.ProductId,
                Size = v.Size,
                Thickness = v.Thickness,
                UnitPcsToKg = v.UnitPcsToKg,
                AlertQtyPcs = v.AlertQtyPcs,
                LastPurchaseRate = v.LastPurchaseRate,
                LastPurchaseRateOn = v.LastPurchaseRateOn,
                DefaultMargin = v.DefaultMargin,
                IsActive = v.IsActive
            }).ToList()
        }).ToList();
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var p = await _repository.GetByIdAsync(id, cancellationToken);
        if (p == null) return null;

        return new ProductDto
        {
            Id = p.Id,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name ?? "Unknown",
            Name = p.Name,
            HsnCode = p.HsnCode,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            Variants = p.Variants.Where(v => !v.IsDeleted).Select(v => new ProductVariantDto
            {
                Id = v.Id,
                ProductId = v.ProductId,
                Size = v.Size,
                Thickness = v.Thickness,
                UnitPcsToKg = v.UnitPcsToKg,
                AlertQtyPcs = v.AlertQtyPcs,
                LastPurchaseRate = v.LastPurchaseRate,
                LastPurchaseRateOn = v.LastPurchaseRateOn,
                DefaultMargin = v.DefaultMargin,
                IsActive = v.IsActive
            }).ToList()
        };
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto request, Guid? userId, CancellationToken cancellationToken = default)
    {
        request.Name = request.Name.Trim();
        var exists = await _repository.GetByNameAsync(request.Name, cancellationToken);
        if (exists != null)
            throw new InvalidOperationException("A product with this name already exists.");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            CategoryId = request.CategoryId,
            Name = request.Name,
            HsnCode = request.HsnCode,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };

        var variants = request.Variants.Select(v => new ProductVariant
        {
            Id = Guid.NewGuid(),
            ProductId = product.Id,
            Size = v.Size.Trim(),
            Thickness = v.Thickness.Trim(),
            UnitPcsToKg = v.UnitPcsToKg,
            AlertQtyPcs = v.AlertQtyPcs,
            DefaultMargin = v.DefaultMargin,
            IsActive = v.IsActive,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        }).ToList();

        await _repository.AddAsync(product, cancellationToken);
        await _repository.AddVariantsAsync(variants, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(product.Id, cancellationToken) ?? throw new Exception("Failed to retrieve created product.");
    }

    public async Task<ProductDto?> UpdateAsync(Guid id, UpdateProductDto request, Guid? userId, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        if (product == null) return null;

        request.Name = request.Name.Trim();
        var exists = await _repository.GetByNameAsync(request.Name, cancellationToken);
        if (exists != null && exists.Id != id)
            throw new InvalidOperationException("A product with this name already exists.");

        product.CategoryId = request.CategoryId;
        product.Name = request.Name;
        product.HsnCode = request.HsnCode;
        product.IsActive = request.IsActive;
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdatedBy = userId;

        // Handle Variants
        var existingVariants = product.Variants.Where(v => !v.IsDeleted).ToList();
        var updatedVariantIds = request.Variants.Where(v => v.Id.HasValue).Select(v => v.Id!.Value).ToList();

        // 1. Mark missing variants as deleted
        var variantsToDelete = existingVariants.Where(v => !updatedVariantIds.Contains(v.Id)).ToList();
        foreach (var v in variantsToDelete)
        {
            v.IsDeleted = true;
            v.UpdatedAt = DateTime.UtcNow;
            v.UpdatedBy = userId;
        }

        // 2. Update existing variants
        var variantsToUpdate = new List<ProductVariant>();
        foreach (var existing in existingVariants.Where(v => updatedVariantIds.Contains(v.Id)))
        {
            var updateReq = request.Variants.First(v => v.Id == existing.Id);
            existing.Size = updateReq.Size.Trim();
            existing.Thickness = updateReq.Thickness.Trim();
            existing.UnitPcsToKg = updateReq.UnitPcsToKg;
            existing.AlertQtyPcs = updateReq.AlertQtyPcs;
            existing.DefaultMargin = updateReq.DefaultMargin;
            existing.IsActive = updateReq.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedBy = userId;
            variantsToUpdate.Add(existing);
        }

        // 3. Add new variants
        var newVariants = request.Variants.Where(v => !v.Id.HasValue).Select(v => new ProductVariant
        {
            Id = Guid.NewGuid(),
            ProductId = product.Id,
            Size = v.Size.Trim(),
            Thickness = v.Thickness.Trim(),
            UnitPcsToKg = v.UnitPcsToKg,
            AlertQtyPcs = v.AlertQtyPcs,
            DefaultMargin = v.DefaultMargin,
            IsActive = v.IsActive,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        }).ToList();

        await _repository.UpdateAsync(product, cancellationToken);
        if (variantsToDelete.Any()) await _repository.DeleteVariantsAsync(variantsToDelete, cancellationToken);
        if (variantsToUpdate.Any()) await _repository.UpdateVariantsAsync(variantsToUpdate, cancellationToken);
        if (newVariants.Any()) await _repository.AddVariantsAsync(newVariants, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(product.Id, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid? userId, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        if (product == null) return false;

        product.IsDeleted = true;
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdatedBy = userId;

        // Also soft delete all variants
        foreach (var variant in product.Variants.Where(v => !v.IsDeleted))
        {
            variant.IsDeleted = true;
            variant.UpdatedAt = DateTime.UtcNow;
            variant.UpdatedBy = userId;
        }

        await _repository.UpdateAsync(product, cancellationToken);
        await _repository.DeleteVariantsAsync(product.Variants, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
