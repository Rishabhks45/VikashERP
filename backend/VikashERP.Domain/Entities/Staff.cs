namespace VikashERP.Domain.Entities;

public class Staff
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public decimal SalaryPerMonth { get; set; }
    public string Phone { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public ICollection<Attendance> AttendanceRecords { get; set; } = [];
    public ICollection<StaffSalary> SalaryPayments { get; set; } = [];
}
