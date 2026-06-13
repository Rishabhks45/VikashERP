using FluentValidation;

namespace VikashERP.Application.Features.Purchases.DTOs;

public class CreatePurchaseEntryDtoValidator : AbstractValidator<CreatePurchaseEntryDto>
{
    public CreatePurchaseEntryDtoValidator()
    {
        RuleFor(x => x.SupplierId).NotEmpty().WithMessage("Supplier is required.");
        RuleFor(x => x.InvoiceNumber).NotEmpty().WithMessage("Invoice Number is required.");
        RuleFor(x => x.InvoiceDate).NotEmpty().WithMessage("Invoice Date is required.");
        
        RuleFor(x => x.Items).NotEmpty().WithMessage("At least one item is required.");
        RuleForEach(x => x.Items).SetValidator(new CreatePurchaseEntryItemDtoValidator());
    }
}

public class CreatePurchaseEntryItemDtoValidator : AbstractValidator<CreatePurchaseEntryItemDto>
{
    public CreatePurchaseEntryItemDtoValidator()
    {
        RuleFor(x => x.ProductVariantId).NotEmpty().WithMessage("Product is required.");
        RuleFor(x => x.QuantityPcs).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        RuleFor(x => x.WeightKg).GreaterThan(0).WithMessage("Weight must be greater than zero.");
        RuleFor(x => x.Rate).GreaterThan(0).WithMessage("Rate must be greater than zero.");
    }
}
