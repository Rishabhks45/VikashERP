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
