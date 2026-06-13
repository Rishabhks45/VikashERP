namespace VikashERP.Domain.Entities;

public class Supplier
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Gstin { get; set; }
    public string? Address { get; set; }
    public decimal CurrentBalance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<SupplierLedger> LedgerEntries { get; set; } = [];
}
