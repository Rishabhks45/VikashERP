namespace VikashERP.Domain.Entities;

public class InvoiceItem
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int VariantId { get; set; }
    public int QtyPcs { get; set; }
    public decimal WeightKg { get; set; }
    public decimal RatePerKg { get; set; }
    public decimal CgstRate { get; set; } = 9m;
    public decimal SgstRate { get; set; } = 9m;
    public decimal IgstRate { get; set; }
    public decimal TotalPrice { get; set; }

    public Invoice Invoice { get; set; } = null!;
    public ProductVariant Variant { get; set; } = null!;
}
