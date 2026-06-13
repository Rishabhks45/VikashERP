using VikashERP.SharedKernel.Enums;

namespace VikashERP.Domain.Entities;

public class User : BaseEntity
{

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public UserRole Role { get; set; } = UserRole.Customer;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }



    public DateTime? LastLoginAt { get; set; }

    public UserCustomerMapping? CustomerMapping { get; set; }
}