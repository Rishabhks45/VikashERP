using VikashERP.Domain.Entities;

namespace VikashERP.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task UpdateUserAsync(User user, CancellationToken cancellationToken);
    Task<PasswordResetToken?> GetValidResetTokenAsync(string token, CancellationToken cancellationToken);
    Task SavePasswordResetTokenAsync(Guid userId, string token, DateTime expiresAtUtc, CancellationToken cancellationToken);
    Task MarkResetTokenUsedAsync(string token, CancellationToken cancellationToken);
    Task UpdatePasswordAsync(Guid userId, string encryptedPassword, CancellationToken cancellationToken);
}
