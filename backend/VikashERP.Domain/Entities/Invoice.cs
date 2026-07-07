namespace VikashERP.Domain.Entities;

public class Invoice : BaseEntity
{

    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public decimal Subtotal { get; set; }
    public decimal FreightCharge { get; set; }
    public decimal LoadingCharge { get; set; }
    public decimal CgstAmount { get; set; }
    public decimal SgstAmount { get; set; }
    public decimal IgstAmount { get; set; }
    public decimal RoundingAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal CashAmount { get; set; }
    public decimal BankAmount { get; set; }
    public decimal DueAmount { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public string? VehicleNumber { get; set; }
    public string? Remarks { get; set; }
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
    public VikashERP.SharedKernel.Enums.SalesInvoiceStatus Status { get; set; } = VikashERP.SharedKernel.Enums.SalesInvoiceStatus.Draft;


    public Customer Customer { get; set; } = null!;
    public ICollection<InvoiceItem> Items { get; set; } = [];
    public ICollection<Delivery> Deliveries { get; set; } = [];
}
