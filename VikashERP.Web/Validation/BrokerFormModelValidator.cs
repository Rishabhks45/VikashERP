using FluentValidation;
using VikashERP.Web.Models.Brokers;

namespace VikashERP.Web.Validation;

public class BrokerFormModelValidator : AbstractValidator<BrokerFormModel>
{
    public BrokerFormModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Broker Name is required.")
            .MaximumLength(100).WithMessage("Broker Name cannot exceed 100 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");
    }
}
