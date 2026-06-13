using VikashERP.SharedKernel.Enums;

namespace VikashERP.Domain.Entities;

public class PurchaseEntry : BaseEntity
{
    public string EntryNumber { get; set; } = string.Empty;
    public Guid SupplierId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public decimal LoadingCharge { get; set; }
    public decimal FreightCharge { get; set; }
    public decimal NetAmount => TotalAmount + LoadingCharge + FreightCharge;
    public PurchaseEntryStatus Status { get; set; } = PurchaseEntryStatus.Draft;
    public string? Remarks { get; set; }
    public string? VehicleNumber { get; set; }

    public Supplier Supplier { get; set; } = null!;
    public ICollection<PurchaseEntryItem> Items { get; set; } = [];
}
