namespace VikashERP.Domain.Entities;

public class StockLedger
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid VariantId { get; set; }
    public Guid GodownId { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public Guid? ReferenceId { get; set; }
    public int QtyPcs { get; set; }
    public decimal WeightKg { get; set; }
    public int RunningPcs { get; set; }
    public decimal RunningWeightKg { get; set; }
    public string? Remarks { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ProductVariant Variant { get; set; } = null!;
    public Godown Godown { get; set; } = null!;
}
