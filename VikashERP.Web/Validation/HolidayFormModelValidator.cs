using FluentValidation;
using VikashERP.Web.Models;

namespace VikashERP.Web.Validation;

public class HolidayFormModelValidator : AbstractValidator<HolidayFormModel>
{
    public HolidayFormModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Holiday name is required.")
            .MaximumLength(100).WithMessage("Holiday name cannot exceed 100 characters.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Holiday date is required.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}
