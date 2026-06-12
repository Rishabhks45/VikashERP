using FluentValidation;
using VikashERP.Application.Features.Organization.DTOs;

namespace VikashERP.Application.Features.Organization.Validators;

public class UpdateOrganizationRequestValidator : AbstractValidator<UpdateOrganizationRequest>
{
    public UpdateOrganizationRequestValidator()
    {
        RuleFor(x => x.LegalName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Tagline).MaximumLength(500);
        RuleFor(x => x.LogoUrl).MaximumLength(500);
        RuleFor(x => x.FaviconUrl).MaximumLength(500);
        RuleFor(x => x.LoginBackgroundUrl).MaximumLength(500);
        RuleFor(x => x.PrimaryColor).MaximumLength(20);
        RuleFor(x => x.SecondaryColor).MaximumLength(20);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.EmailFromAddress).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.EmailFromAddress));
        RuleFor(x => x.Gstin).MaximumLength(15);
        RuleFor(x => x.Pan).MaximumLength(10);
        RuleFor(x => x.IfscCode).MaximumLength(20);
        RuleFor(x => x.DefaultCurrency).NotEmpty().MaximumLength(10);
        RuleFor(x => x.DefaultWeightUnit).NotEmpty().MaximumLength(10);
        RuleFor(x => x.TimeZone).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DateFormat).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
    }
}
