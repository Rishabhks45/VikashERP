using VikashERP.Domain.Entities;

namespace VikashERP.Application.Interfaces;

public interface IUserCustomerMappingRepository
{
    Task<UserCustomerMapping?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UserCustomerMapping>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<UserCustomerMapping> CreateAsync(UserCustomerMapping mapping, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserCustomerMapping mapping, CancellationToken cancellationToken = default);
}
