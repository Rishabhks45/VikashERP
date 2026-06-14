using System;

namespace VikashERP.Web.Models;

public class CategoryListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TotalProducts { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CategoryFormModel
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsNewCategory { get; set; } = true;
}

public class GodownStockDto
{
    public Guid VariantId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Thickness { get; set; } = string.Empty;
    
    public int RunningPcs { get; set; }
    public decimal RunningWeightKg { get; set; }
    
    public int AlertQtyPcs { get; set; }
    public string StockStatus { get; set; } = string.Empty; // "In Stock", "Low Stock", "Out of Stock"
    public DateTime? LastUpdateDate { get; set; }
}