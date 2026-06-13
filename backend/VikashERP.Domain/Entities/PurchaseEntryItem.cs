namespace VikashERP.Domain.Entities;

public class PurchaseEntryItem : BaseEntity
{
    public Guid PurchaseEntryId { get; set; }
    public Guid ProductVariantId { get; set; }
    public int QuantityPcs { get; set; }
    public decimal WeightKg { get; set; }
    public decimal Rate { get; set; }
    public decimal TotalPrice { get; set; }

    public PurchaseEntry PurchaseEntry { get; set; } = null!;
    public ProductVariant ProductVariant { get; set; } = null!;
}
