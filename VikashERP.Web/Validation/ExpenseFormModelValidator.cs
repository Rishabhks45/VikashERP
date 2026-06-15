using FluentValidation;
using VikashERP.Web.Models;

namespace VikashERP.Web.Validation;

public class ExpenseFormModelValidator : AbstractValidator<ExpenseFormModel>
{
    public ExpenseFormModelValidator()
    {
        RuleFor(x => x.ExpenseDate)
            .NotEmpty().WithMessage("Expense Date is required.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required.")
            .MaximumLength(100).WithMessage("Category cannot exceed 100 characters.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.PaymentMode)
            .NotEmpty().WithMessage("Payment Mode is required.")
            .MaximumLength(50).WithMessage("Payment Mode cannot exceed 50 characters.");

        RuleFor(x => x.Remarks)
            .MaximumLength(500).WithMessage("Remarks cannot exceed 500 characters.");
    }
}
