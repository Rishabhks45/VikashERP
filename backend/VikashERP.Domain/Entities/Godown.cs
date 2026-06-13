namespace VikashERP.Domain.Entities;

public class Godown : BaseEntity
{

    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }


    public ICollection<StockLedger> StockLedgerEntries { get; set; } = [];
}
