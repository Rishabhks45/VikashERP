using FluentValidation;
using VikashERP.Web.Models.Brokers;

namespace VikashERP.Web.Validation;

public class BrokerTransactionFormModelValidator : AbstractValidator<BrokerTransactionFormModel>
{
    public BrokerTransactionFormModelValidator()
    {
        RuleFor(x => x.SelectedBroker)
            .NotEmpty().WithMessage("Broker selection is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.TransactionType)
            .NotEmpty().WithMessage("Transaction type is required.");

        RuleFor(x => x.PaymentMode)
            .NotEmpty().WithMessage("Payment mode is required.");

        RuleFor(x => x.TransactionDate)
            .NotEmpty().WithMessage("Transaction date is required.");

        RuleFor(x => x.Remarks)
            .MaximumLength(500).WithMessage("Remarks cannot exceed 500 characters.");
    }
}
