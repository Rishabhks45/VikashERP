using VikashERP.Application.Features.Categories.DTOs;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;

namespace VikashERP.Infrastructure.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(ICategoryRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<CategoryListDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _repository.GetAllAsync(cancellationToken);
        return categories.Select(c => new CategoryListDto
        {
            Id = c.Id,
            Name = c.Name,
            TotalProducts = 0, // Mocked until Products are added
            CreatedAt = c.CreatedAt,
            IsActive = c.IsActive
        }).ToList();
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var c = await _repository.GetByIdAsync(id, cancellationToken);
        if (c == null) return null;

        return new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            IsActive = c.IsActive
        };
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto request, Guid? userId, CancellationToken cancellationToken = default)
    {
        request.Name = request.Name.Trim();
        var exists = await _repository.GetByNameAsync(request.Name, cancellationToken);
        if (exists != null)
            throw new InvalidOperationException("A category with this name already exists.");

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };

        await _repository.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            CreatedAt = category.CreatedAt,
            IsActive = category.IsActive
        };
    }

    public async Task<CategoryDto?> UpdateAsync(Guid id, UpdateCategoryDto request, Guid? userId, CancellationToken cancellationToken = default)
    {
        var category = await _repository.GetByIdAsync(id, cancellationToken);
        if (category == null) return null;

        request.Name = request.Name.Trim();
        var exists = await _repository.GetByNameAsync(request.Name, cancellationToken);
        if (exists != null && exists.Id != id)
            throw new InvalidOperationException("A category with this name already exists.");

        category.Name = request.Name;
        category.IsActive = request.IsActive;
        category.UpdatedAt = DateTime.UtcNow;
        category.UpdatedBy = userId;

        await _repository.UpdateAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt,
            IsActive = category.IsActive
        };
    }

    public async Task<bool> DeleteAsync(Guid id, Guid? userId, CancellationToken cancellationToken = default)
    {
        var category = await _repository.GetByIdAsync(id, cancellationToken);
        if (category == null) return false;

        category.IsDeleted = true;
        category.UpdatedAt = DateTime.UtcNow;
        category.UpdatedBy = userId;

        await _repository.UpdateAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
