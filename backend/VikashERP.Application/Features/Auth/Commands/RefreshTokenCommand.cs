using System.Security.Claims;
using MediatR;
using VikashERP.Application.Features.Auth.DTOs;
using VikashERP.Application.Interfaces;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Application.Features.Auth.Commands;

public class RefreshTokenCommand : IRequest<UserLoginResponse?>
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, UserLoginResponse?>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public RefreshTokenCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<UserLoginResponse?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = _jwtProvider.ValidateToken(request.AccessToken, validateLifetime: false);
        if (principal is null)
            return null;

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? principal.FindFirst("sub")?.Value;

        if (!Guid.TryParse(userIdClaim, out var userId))
            return null;

        var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
        if (user is null
            || string.IsNullOrEmpty(user.RefreshToken)
            || user.RefreshToken != request.RefreshToken
            || user.RefreshTokenExpiry is null
            || user.RefreshTokenExpiry <= DateTime.UtcNow)
        {
            return null;
        }

        var userName = $"{user.FirstName} {user.LastName}".Trim();
        var roleName = user.Role.ToFriendlyName();
        var token = _jwtProvider.GenerateToken(user.Id, user.Email, userName, roleName);
        var refreshToken = _jwtProvider.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _userRepository.UpdateUserAsync(user, cancellationToken);

        return new UserLoginResponse
        {
            UserId = user.Id,
            UserName = userName,
            Email = user.Email,
            Role = roleName,
            Token = token,
            RefreshToken = refreshToken,
            RefreshTokenExpiry = user.RefreshTokenExpiry.Value,
            TokenType = "Bearer",
            ExpiresIn = 24 * 3600
        };
    }
}
