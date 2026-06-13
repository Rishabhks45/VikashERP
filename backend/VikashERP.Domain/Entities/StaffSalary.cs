namespace VikashERP.Domain.Entities;

public class StaffSalary
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StaffId { get; set; }
    public DateOnly PaymentDate { get; set; }
    public decimal AmountPaid { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public string? Remarks { get; set; }

    public Staff Staff { get; set; } = null!;
}
