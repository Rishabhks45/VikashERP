using VikashERP.SharedKernel.Enums;

namespace VikashERP.Application.Features.Purchases.DTOs;

public class PurchaseEntryDto
{
    public Guid Id { get; set; }
    public string EntryNumber { get; set; } = string.Empty;
    public Guid SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal LoadingCharge { get; set; }
    public decimal FreightCharge { get; set; }
    public decimal CgstAmount { get; set; }
    public decimal SgstAmount { get; set; }
    public decimal RoundingAmount { get; set; }
    public decimal NetAmount { get; set; }
    public PurchaseEntryStatus Status { get; set; }
    public string? Remarks { get; set; }
    public string? VehicleNumber { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<PurchaseEntryItemDto> Items { get; set; } = [];
}

public class PurchaseEntryItemDto
{
    public Guid Id { get; set; }
    public Guid ProductVariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string VariantSize { get; set; } = string.Empty;
    public int QuantityPcs { get; set; }
    public decimal WeightKg { get; set; }
    public decimal Rate { get; set; }
    public RateOn RateOn { get; set; } = RateOn.Kg;
    public decimal TotalPrice { get; set; }
}

public class CreatePurchaseEntryDto
{
    public Guid SupplierId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
    public decimal LoadingCharge { get; set; }
    public decimal FreightCharge { get; set; }
    public decimal CgstAmount { get; set; }
    public decimal SgstAmount { get; set; }
    public decimal RoundingAmount { get; set; }
    public string? Remarks { get; set; }
    public string? VehicleNumber { get; set; }

    public List<CreatePurchaseEntryItemDto> Items { get; set; } = [];
}

public class CreatePurchaseEntryItemDto
{
    public Guid ProductVariantId { get; set; }
    public int QuantityPcs { get; set; }
    public decimal WeightKg { get; set; }
    public decimal Rate { get; set; }
    public RateOn RateOn { get; set; } = RateOn.Kg;
    public decimal TotalPrice { get; set; }
}
