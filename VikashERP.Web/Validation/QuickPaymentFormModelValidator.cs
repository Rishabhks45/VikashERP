using FluentValidation;
using VikashERP.Web.Models;

namespace VikashERP.Web.Validation;

public class QuickPaymentFormModelValidator : AbstractValidator<QuickPaymentFormModel>
{
    public QuickPaymentFormModelValidator()
    {
        RuleFor(x => x.SelectedCustomer)
            .NotEmpty().WithMessage("Customer selection is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Payment amount must be greater than zero.");

        RuleFor(x => x.PaymentMode)
            .NotEmpty().WithMessage("Payment mode is required.");

        RuleFor(x => x.PaymentDate)
            .NotEmpty().WithMessage("Payment date is required.");

        RuleFor(x => x.Remarks)
            .MaximumLength(500).WithMessage("Remarks cannot exceed 500 characters.");
    }
}
