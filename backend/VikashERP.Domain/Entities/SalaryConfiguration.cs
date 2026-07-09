using System;

namespace VikashERP.Domain.Entities;

public class SalaryConfiguration : BaseEntity
{
    public Guid UserId { get; set; }
    public decimal BasicSalary { get; set; }

    // Navigation property
    public User User { get; set; } = null!;
}
