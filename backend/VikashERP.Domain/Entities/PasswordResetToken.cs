namespace VikashERP.Domain.Entities;

public class PasswordResetToken : BaseEntity
{

    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
    public bool IsUsed { get; set; }


    public User User { get; set; } = null!;
}
