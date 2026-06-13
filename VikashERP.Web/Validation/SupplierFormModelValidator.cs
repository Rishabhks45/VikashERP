using FluentValidation;
using VikashERP.Web.Models.Forms;

namespace VikashERP.Web.Validation;

public class SupplierFormModelValidator : AbstractValidator<SupplierFormModel>
{
    public SupplierFormModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Supplier Name is required.")
            .MaximumLength(100);

        RuleFor(x => x.CompanyName)
            .MaximumLength(200);

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(20);

        RuleFor(x => x.Gstin)
            .MaximumLength(15);

        RuleFor(x => x.Address)
            .MaximumLength(500);
    }
}
