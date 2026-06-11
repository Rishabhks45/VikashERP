using System.Security.Claims;

namespace VikashERP.Application.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(Guid userId, string email, string userName, string role);
    string GenerateRefreshToken();
    ClaimsPrincipal? ValidateToken(string token, bool validateLifetime = true);
}
