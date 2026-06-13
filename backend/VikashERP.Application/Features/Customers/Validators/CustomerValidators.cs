using FluentValidation;
using VikashERP.Application.Features.Customers.DTOs;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Application.Features.Customers.Validators;

public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerDtoValidator()
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
            .NotEmpty()
            .Must(mode => CustomerPaymentModeExtensions.FromString(mode) != null)
            .WithMessage("Payment mode must be Cash or A/C.");

        RuleFor(x => x.CreditLimit).GreaterThanOrEqualTo(0);
    }
}

public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
{
    public UpdateCustomerDtoValidator()
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
            .NotEmpty()
            .Must(mode => CustomerPaymentModeExtensions.FromString(mode) != null)
            .WithMessage("Payment mode must be Cash or A/C.");

        RuleFor(x => x.CreditLimit).GreaterThanOrEqualTo(0);
    }
}

public class UpdateCustomerShopDtoValidator : AbstractValidator<UpdateCustomerShopDto>
{
    public UpdateCustomerShopDtoValidator()
    {
        RuleFor(x => x.CompanyName).MaximumLength(255);
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(20);
        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
            .MaximumLength(255);
        RuleFor(x => x.Gstin).MaximumLength(15);
        RuleFor(x => x.DefaultPaymentMode)
            .NotEmpty()
            .Must(mode => CustomerPaymentModeExtensions.FromString(mode) != null)
            .WithMessage("Payment mode must be Cash or A/C.");
    }
}
