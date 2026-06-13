using FluentValidation;
using VikashERP.Web.Models.Forms;

namespace VikashERP.Web.Validation;

public class ProductFormModelValidator : AbstractValidator<ProductFormModel>
{
    public ProductFormModelValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100);

        RuleFor(x => x.HsnCode)
            .MaximumLength(20);

        RuleFor(x => x.Variants)
            .NotEmpty().WithMessage("At least one variant must be added.");

        RuleForEach(x => x.Variants).SetValidator(new ProductVariantFormModelValidator());
    }
}

public class ProductVariantFormModelValidator : AbstractValidator<ProductVariantFormModel>
{
    public ProductVariantFormModelValidator()
    {
        RuleFor(x => x.Size)
            .NotEmpty().WithMessage("Size is required.");

        RuleFor(x => x.Thickness)
            .NotEmpty().WithMessage("Thickness is required.");

        RuleFor(x => x.UnitPcsToKg)
            .GreaterThan(0).WithMessage("Unit conversion must be greater than zero.");

        RuleFor(x => x.AlertQtyPcs)
            .GreaterThanOrEqualTo(0).WithMessage("Alert quantity cannot be negative.");
    }
}
