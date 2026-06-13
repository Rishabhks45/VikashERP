namespace VikashERP.Domain.Entities;

public class InvoiceItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid InvoiceId { get; set; }
    public Guid VariantId { get; set; }
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
