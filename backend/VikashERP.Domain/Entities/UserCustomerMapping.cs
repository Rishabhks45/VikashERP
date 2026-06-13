namespace VikashERP.Domain.Entities;

/// <summary>
/// Links a login account (<see cref="User"/>) to an ERP customer master record (<see cref="Customer"/>).
/// </summary>
public class UserCustomerMapping
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid CustomerId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public User User { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
}
