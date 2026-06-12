using MediatR;
using VikashERP.Application.Features.Auth.DTOs;
using VikashERP.Application.Interfaces;
using VikashERP.SharedKernel.Common.Interfaces;
using VikashERP.SharedKernel.Settings;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<UserLoginResponse?>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, UserLoginResponse?>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserCustomerMappingRepository _userCustomerMappingRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly IJwtProvider _jwtProvider;
    private readonly EncryptionSettings _encryptionSettings;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IUserCustomerMappingRepository userCustomerMappingRepository,
        IEncryptionService encryptionService,
        IJwtProvider jwtProvider,
        Microsoft.Extensions.Options.IOptions<EncryptionSettings> options)
    {
        _userRepository = userRepository;
        _userCustomerMappingRepository = userCustomerMappingRepository;
        _encryptionService = encryptionService;
        _jwtProvider = jwtProvider;
        _encryptionSettings = options.Value;
    }

    public async Task<UserLoginResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        if (user is null)
            return null;

        var decryptedStoredPassword = await _encryptionService.DecryptAsync(user.PasswordHash, _encryptionSettings.MasterKey);

        if (request.Password != decryptedStoredPassword)
            return null;

        var userName = $"{user.FirstName} {user.LastName}".Trim();
        var roleName = user.Role.ToFriendlyName();
        var mapping = await _userCustomerMappingRepository.GetByUserIdAsync(user.Id, cancellationToken);
        var customerId = mapping?.CustomerId;
        var token = _jwtProvider.GenerateToken(user.Id, user.Email, userName, roleName, user.ProfilePictureUrl, customerId);
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
            ProfilePictureUrl = user.ProfilePictureUrl,
            CustomerId = customerId,
            Token = token,
            RefreshToken = refreshToken,
            RefreshTokenExpiry = user.RefreshTokenExpiry.Value,
            TokenType = "Bearer",
            ExpiresIn = 24 * 3600
        };
    }
}
