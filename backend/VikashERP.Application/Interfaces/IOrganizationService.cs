using VikashERP.Application.Features.Organization.DTOs;

namespace VikashERP.Application.Interfaces;

public interface IOrganizationService
{
    Task<OrganizationDto> GetOrCreateAsync(CancellationToken cancellationToken = default);
    Task<OrganizationPublicDto> GetPublicAsync(CancellationToken cancellationToken = default);
    Task<OrganizationDto?> UpdateAsync(UpdateOrganizationRequest request, CancellationToken cancellationToken = default);
}
