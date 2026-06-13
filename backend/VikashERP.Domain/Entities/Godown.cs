namespace VikashERP.Domain.Entities;

public class Godown
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<StockLedger> StockLedgerEntries { get; set; } = [];
}
