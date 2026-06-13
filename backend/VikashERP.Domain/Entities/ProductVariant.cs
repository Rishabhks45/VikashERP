namespace VikashERP.Domain.Entities;

public class ProductVariant : BaseEntity
{

    public Guid ProductId { get; set; }
    public string Size { get; set; } = string.Empty;
    public string Thickness { get; set; } = string.Empty;
    public decimal UnitPcsToKg { get; set; } = 1m;
    public int AlertQtyPcs { get; set; } = 10;


    public Product Product { get; set; } = null!;
    public ICollection<StockLedger> StockLedgerEntries { get; set; } = [];
    public ICollection<InvoiceItem> InvoiceItems { get; set; } = [];
}
