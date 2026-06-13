namespace VikashERP.Domain.Entities;

public class Attendance : BaseEntity
{

    public Guid StaffId { get; set; }
    public DateOnly WorkDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public string Status { get; set; } = string.Empty;
    public TimeOnly? CheckIn { get; set; }
    public TimeOnly? CheckOut { get; set; }


    public Staff Staff { get; set; } = null!;
}
