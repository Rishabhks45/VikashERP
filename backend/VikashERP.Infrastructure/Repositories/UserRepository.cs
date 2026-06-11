using Microsoft.EntityFrameworkCore;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Infrastructure.Data;

namespace VikashERP.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public UserRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        user.UpdatedAt = DateTime.UtcNow;
        _context.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<PasswordResetToken?> GetValidResetTokenAsync(string token, CancellationToken cancellationToken)
    {
        return await _context.PasswordResetTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(
                t => t.Token == token && !t.IsUsed && t.ExpiresAtUtc > DateTime.UtcNow,
                cancellationToken);
    }

    public async Task SavePasswordResetTokenAsync(Guid userId, string token, DateTime expiresAtUtc, CancellationToken cancellationToken)
    {
        _context.PasswordResetTokens.Add(new PasswordResetToken
        {
            UserId = userId,
            Token = token,
            ExpiresAtUtc = expiresAtUtc
        });

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkResetTokenUsedAsync(string token, CancellationToken cancellationToken)
    {
        var resetToken = await _context.PasswordResetTokens
            .FirstOrDefaultAsync(t => t.Token == token, cancellationToken);

        if (resetToken is null)
            return;

        resetToken.IsUsed = true;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePasswordAsync(Guid userId, string encryptedPassword, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user is null)
            return;

        user.PasswordHash = encryptedPassword;
        user.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
