using FluentValidation;

namespace VikashERP.Application.Features.Products.DTOs;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product Name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Variants)
            .NotEmpty().WithMessage("At least one variant must be added.");

        RuleForEach(x => x.Variants).SetValidator(new CreateProductVariantDtoValidator());
    }
}

public class CreateProductVariantDtoValidator : AbstractValidator<CreateProductVariantDto>
{
    public CreateProductVariantDtoValidator()
    {
        RuleFor(x => x.Size)
            .NotEmpty().WithMessage("Size is required.");

        RuleFor(x => x.Thickness)
            .NotEmpty().WithMessage("Thickness is required.");

        RuleFor(x => x.UnitPcsToKg)
            .GreaterThan(0).WithMessage("Unit conversion must be greater than zero.");
    }
}

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductDtoValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product Name is required.")
            .MaximumLength(100);

        RuleForEach(x => x.Variants).SetValidator(new UpdateProductVariantDtoValidator());
    }
}

public class UpdateProductVariantDtoValidator : AbstractValidator<UpdateProductVariantDto>
{
    public UpdateProductVariantDtoValidator()
    {
        RuleFor(x => x.Size)
            .NotEmpty().WithMessage("Size is required.");

        RuleFor(x => x.Thickness)
            .NotEmpty().WithMessage("Thickness is required.");

        RuleFor(x => x.UnitPcsToKg)
            .GreaterThan(0).WithMessage("Unit conversion must be greater than zero.");
    }
}
