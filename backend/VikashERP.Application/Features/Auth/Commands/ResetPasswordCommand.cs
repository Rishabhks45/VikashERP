using MediatR;
using VikashERP.Application.Interfaces;
using VikashERP.SharedKernel.Common.Interfaces;
using VikashERP.SharedKernel.Settings;

namespace VikashERP.Application.Features.Auth.Commands;

public class ResetPasswordCommand : IRequest<bool>
{
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly ISharedRepository _sharedRepository;
    private readonly EncryptionSettings _encryptionSettings;

    public ResetPasswordCommandHandler(
        IUserRepository userRepository,
        IEncryptionService encryptionService,
        ISharedRepository sharedRepository,
        Microsoft.Extensions.Options.IOptions<EncryptionSettings> options)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
        _sharedRepository = sharedRepository;
        _encryptionSettings = options.Value;
    }

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var resetToken = await _userRepository.GetValidResetTokenAsync(request.Token, cancellationToken);
        if (resetToken is null)
            return false;

        var encryptedPassword = await _encryptionService.EncryptAsync(request.NewPassword, _encryptionSettings.MasterKey);
        await _userRepository.UpdatePasswordAsync(resetToken.UserId, encryptedPassword, cancellationToken);
        await _userRepository.MarkResetTokenUsedAsync(request.Token, cancellationToken);

        var user = await _userRepository.GetUserByIdAsync(resetToken.UserId, cancellationToken);
        if (user is not null)
        {
            var userName = string.IsNullOrWhiteSpace(user.FirstName)
                ? user.Email
                : $"{user.FirstName} {user.LastName}".Trim();

            await _sharedRepository.SendPasswordResetSuccessEmailAsync(user.Email, userName, cancellationToken);
        }

        return true;
    }
}
