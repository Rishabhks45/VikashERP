using Microsoft.EntityFrameworkCore;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Infrastructure.Data;

namespace VikashERP.Infrastructure.Repositories;

public class UserCustomerMappingRepository : IUserCustomerMappingRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public UserCustomerMappingRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public Task<UserCustomerMapping?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _context.UserCustomerMappings
            .Include(m => m.Customer)
            .FirstOrDefaultAsync(m => m.UserId == userId && m.IsActive, cancellationToken);
    }

    public async Task<IReadOnlyList<UserCustomerMapping>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await _context.UserCustomerMappings
            .Include(m => m.User)
            .Where(m => m.CustomerId == customerId && m.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<UserCustomerMapping> CreateAsync(UserCustomerMapping mapping, CancellationToken cancellationToken = default)
    {
        mapping.CreatedAt = DateTime.UtcNow;
        _context.UserCustomerMappings.Add(mapping);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return mapping;
    }

    public async Task UpdateAsync(UserCustomerMapping mapping, CancellationToken cancellationToken = default)
    {
        mapping.UpdatedAt = DateTime.UtcNow;
        _context.UserCustomerMappings.Update(mapping);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
