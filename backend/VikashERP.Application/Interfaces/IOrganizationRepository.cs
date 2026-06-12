using VikashERP.Domain.Entities;

namespace VikashERP.Application.Interfaces;

public interface IOrganizationRepository
{
    Task<Organization?> GetAsync(CancellationToken cancellationToken = default);
    Task<Organization> GetOrCreateDefaultAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Organization organization, CancellationToken cancellationToken = default);
}
