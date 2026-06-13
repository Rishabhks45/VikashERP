using FluentValidation;
using VikashERP.Web.Models.Forms;

namespace VikashERP.Web.Validation;

public class CustomerFormModelValidator : AbstractValidator<CustomerFormModel>
{
    public CustomerFormModelValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100);

        RuleFor(x => x.CompanyName).MaximumLength(255);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(20);

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
            .MaximumLength(255);

        RuleFor(x => x.Gstin).MaximumLength(15);

        RuleFor(x => x.DefaultPaymentMode)
            .NotEmpty().WithMessage("Default payment mode is required.");

        RuleFor(x => x.CreditLimit)
            .GreaterThanOrEqualTo(0).WithMessage("Credit limit cannot be negative.");
    }
}
