using System;

namespace VikashERP.Mobile.Models;

public class GodownStockDto
{
    public string CategoryName { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Thickness { get; set; } = string.Empty;
    
    public decimal RunningWeightKg { get; set; }
    public decimal RunningPcs { get; set; }
    
    public string StockStatus { get; set; } = string.Empty;
}

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public RateOn SellingUnit { get; set; } = RateOn.Kg;
    public System.Collections.Generic.List<ProductVariantDto> Variants { get; set; } = new();
}

public class ProductVariantDto
{
    public Guid Id { get; set; }
    public string Size { get; set; } = string.Empty;
    public string Thickness { get; set; } = string.Empty;
    public decimal DefaultMargin { get; set; }
    public decimal LastPurchaseRate { get; set; }

    // Computed property for UI binding
    public string Name => Size == Thickness ? Size : $"{Size} {Thickness}";
    public decimal SaleRate => LastPurchaseRate + DefaultMargin; // Assuming SaleRate is derived this way
}
