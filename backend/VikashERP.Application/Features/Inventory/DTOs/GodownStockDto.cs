namespace VikashERP.Application.Features.Inventory.DTOs;

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
    
    // For sorting/grouping if needed
    public DateTime? LastUpdateDate { get; set; }
}
