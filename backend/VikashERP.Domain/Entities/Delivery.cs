namespace VikashERP.Domain.Entities;

public class Delivery
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public string VehicleNumber { get; set; } = string.Empty;
    public string DriverName { get; set; } = string.Empty;
    public string? DriverPhone { get; set; }
    public string DeliveryStatus { get; set; } = "PENDING";
    public string? DeliveryChallanNumber { get; set; }
    public decimal LoadingCharge { get; set; }
    public decimal FreightCharge { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Invoice Invoice { get; set; } = null!;
}
