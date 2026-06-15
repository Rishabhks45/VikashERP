using VikashERP.Application.Features.Products.DTOs;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Infrastructure.Data;

namespace VikashERP.Infrastructure.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;

    public ProductService(IProductRepository repository, IUnitOfWork unitOfWork, ApplicationDbContext context)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _context = context;
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
            SellingUnit = p.SellingUnit,
            ProductImageUrl = p.ProductImageUrl,
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
            SellingUnit = p.SellingUnit,
            ProductImageUrl = p.ProductImageUrl,
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
            }).ToList(),
            SubImages = p.SubImages.Select(s => new ProductSubImageDto
            {
                Id = s.Id,
                ProductId = s.ProductId,
                ImageUrl = s.ImageUrl,
                Description = s.Description,
                DisplayOrder = s.DisplayOrder
            }).OrderBy(s => s.DisplayOrder).ToList()
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
            SellingUnit = p.SellingUnit,
            ProductImageUrl = p.ProductImageUrl,
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
            }).ToList(),
            SubImages = p.SubImages.Select(s => new ProductSubImageDto
            {
                Id = s.Id,
                ProductId = s.ProductId,
                ImageUrl = s.ImageUrl,
                Description = s.Description,
                DisplayOrder = s.DisplayOrder
            }).OrderBy(s => s.DisplayOrder).ToList()
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
            SellingUnit = request.SellingUnit,
            ProductImageUrl = request.ProductImageUrl,
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

        var subImages = request.SubImages.Select(s => new ProductSubImage
        {
            Id = Guid.NewGuid(),
            ProductId = product.Id,
            ImageUrl = s.ImageUrl,
            Description = s.Description,
            DisplayOrder = s.DisplayOrder,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        }).ToList();

        await _repository.AddAsync(product, cancellationToken);
        await _repository.AddVariantsAsync(variants, cancellationToken);
        if (subImages.Any())
        {
            _context.ProductSubImages.AddRange(subImages);
        }
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
        product.SellingUnit = request.SellingUnit;
        product.ProductImageUrl = request.ProductImageUrl;
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

        // Handle SubImages
        var existingSubImages = product.SubImages.ToList();
        var updatedSubImageIds = request.SubImages.Where(s => s.Id.HasValue).Select(s => s.Id!.Value).ToList();

        // 1. Mark missing subImages as deleted
        var subImagesToDelete = existingSubImages.Where(s => !updatedSubImageIds.Contains(s.Id)).ToList();
        foreach (var s in subImagesToDelete)
        {
            s.IsDeleted = true;
            s.IsActive = false;
            s.UpdatedAt = DateTime.UtcNow;
            s.UpdatedBy = userId;
        }

        // 2. Add new subImages directly to DbContext
        var newSubImagesReq = request.SubImages.Where(s => !s.Id.HasValue).ToList();
        foreach (var s in newSubImagesReq)
        {
            _context.ProductSubImages.Add(new ProductSubImage
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                ImageUrl = s.ImageUrl,
                Description = s.Description,
                DisplayOrder = s.DisplayOrder,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            });
        }

        // 3. Update existing subImages
        var subImagesToUpdate = existingSubImages.Where(s => updatedSubImageIds.Contains(s.Id)).ToList();
        foreach (var s in subImagesToUpdate)
        {
            var updateReqImg = request.SubImages.First(req => req.Id == s.Id);
            s.ImageUrl = updateReqImg.ImageUrl;
            s.Description = updateReqImg.Description;
            s.DisplayOrder = updateReqImg.DisplayOrder;
            s.UpdatedAt = DateTime.UtcNow;
            s.UpdatedBy = userId;
        }

        // 4. Add new variants directly to DbContext
        var newVariants = request.Variants.Where(v => !v.Id.HasValue).ToList();
        foreach (var v in newVariants)
        {
            _context.ProductVariants.Add(new ProductVariant
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
            });
        }

        // We do NOT need to call UpdateAsync, DeleteVariantsAsync, etc., 
        // because EF Core's ChangeTracker automatically detects changes to the loaded 'product' and its collections.

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException ex)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var entry in ex.Entries)
            {
                sb.AppendLine(entry.Entity.GetType().Name);
            }
            throw new Exception("DB CONCURRENCY ON: " + sb.ToString(), ex);
        }

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
