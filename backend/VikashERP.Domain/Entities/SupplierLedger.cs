namespace VikashERP.Domain.Entities;

public class SupplierLedger : BaseEntity
{

    public Guid SupplierId { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public string TransactionType { get; set; } = string.Empty;
    public Guid? ReferenceId { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal RunningBalance { get; set; }
    public string? Remarks { get; set; }

    public Supplier Supplier { get; set; } = null!;
}
