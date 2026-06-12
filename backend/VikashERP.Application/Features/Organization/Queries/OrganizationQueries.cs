using MediatR;
using VikashERP.Application.Features.Organization.DTOs;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Organization.Queries;

public class GetOrganizationQuery : IRequest<OrganizationDto>;

public class GetOrganizationQueryHandler : IRequestHandler<GetOrganizationQuery, OrganizationDto>
{
    private readonly IOrganizationService _organizationService;

    public GetOrganizationQueryHandler(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    public Task<OrganizationDto> Handle(GetOrganizationQuery request, CancellationToken cancellationToken) =>
        _organizationService.GetOrCreateAsync(cancellationToken);
}

public class GetPublicOrganizationQuery : IRequest<OrganizationPublicDto>;

public class GetPublicOrganizationQueryHandler : IRequestHandler<GetPublicOrganizationQuery, OrganizationPublicDto>
{
    private readonly IOrganizationService _organizationService;

    public GetPublicOrganizationQueryHandler(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    public Task<OrganizationPublicDto> Handle(GetPublicOrganizationQuery request, CancellationToken cancellationToken) =>
        _organizationService.GetPublicAsync(cancellationToken);
}
