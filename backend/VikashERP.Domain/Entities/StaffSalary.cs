namespace VikashERP.Domain.Entities;

public class StaffSalary : BaseEntity
{

    public Guid StaffId { get; set; }
    public DateOnly PaymentDate { get; set; }
    public decimal AmountPaid { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public string? Remarks { get; set; }

    public Staff Staff { get; set; } = null!;
}
