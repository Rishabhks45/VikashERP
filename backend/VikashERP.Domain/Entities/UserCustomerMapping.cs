namespace VikashERP.Domain.Entities;

/// <summary>
/// Links a login account (<see cref="User"/>) to an ERP customer master record (<see cref="Customer"/>).
/// </summary>
public class UserCustomerMapping : BaseEntity
{

    public Guid UserId { get; set; }
    public Guid CustomerId { get; set; }




    public User User { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
}
