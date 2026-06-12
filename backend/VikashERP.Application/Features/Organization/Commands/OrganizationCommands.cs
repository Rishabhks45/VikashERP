using FluentValidation;
using MediatR;
using VikashERP.Application.Features.Organization.DTOs;
using VikashERP.Application.Features.Organization.Validators;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Organization.Commands;

public class UpdateOrganizationCommand : IRequest<OrganizationDto?>
{
    public UpdateOrganizationRequest Request { get; set; } = null!;
}

public class UpdateOrganizationCommandHandler : IRequestHandler<UpdateOrganizationCommand, OrganizationDto?>
{
    private readonly IOrganizationService _organizationService;
    private readonly UpdateOrganizationRequestValidator _validator;

    public UpdateOrganizationCommandHandler(
        IOrganizationService organizationService,
        UpdateOrganizationRequestValidator validator)
    {
        _organizationService = organizationService;
        _validator = validator;
    }

    public async Task<OrganizationDto?> Handle(UpdateOrganizationCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command.Request, cancellationToken);
        return await _organizationService.UpdateAsync(command.Request, cancellationToken);
    }
}
