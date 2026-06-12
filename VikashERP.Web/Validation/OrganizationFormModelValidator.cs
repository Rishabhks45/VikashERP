using FluentValidation;
using VikashERP.Web.Models.Forms;

namespace VikashERP.Web.Validation;

public class OrganizationFormModelValidator : AbstractValidator<OrganizationFormModel>
{
    public OrganizationFormModelValidator()
    {
        RuleFor(x => x.LegalName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.EmailFromAddress).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.EmailFromAddress));
        RuleFor(x => x.Gstin).MaximumLength(15);
        RuleFor(x => x.Pan).MaximumLength(10);
        RuleFor(x => x.DefaultCurrency).NotEmpty();
        RuleFor(x => x.DefaultWeightUnit).NotEmpty();
        RuleFor(x => x.Country).NotEmpty();
    }
}
