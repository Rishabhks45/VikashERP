using System.Security.Claims;

namespace VikashERP.SharedKernel.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetAuthenticatedUserId(this ClaimsPrincipal user)
    {
        if (user is null)
            return null;

        var sub = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? user.FindFirst("sub")?.Value;
            
        return Guid.TryParse(sub, out var id) ? id : null;
    }
}