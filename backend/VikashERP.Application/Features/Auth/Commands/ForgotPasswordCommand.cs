using MediatR;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Auth.Commands;

public class ForgotPasswordCommand : IRequest<bool>
{
    public string Email { get; set; } = string.Empty;
}

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly ISharedRepository _sharedRepository;
    private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

    public ForgotPasswordCommandHandler(
        IUserRepository userRepository,
        ISharedRepository sharedRepository,
        Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        _userRepository = userRepository;
        _sharedRepository = sharedRepository;
        _configuration = configuration;
    }

    public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        if (user is null)
            return true;

        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("/", "_", StringComparison.Ordinal)
            .Replace("+", "-", StringComparison.Ordinal)
            .TrimEnd('=');

        await _userRepository.SavePasswordResetTokenAsync(user.Id, token, DateTime.UtcNow.AddMinutes(15), cancellationToken);

        var frontendUrl = _configuration["ClientApp:BaseUrl"] ?? "https://localhost:7297";
        var resetLink = $"{frontendUrl.TrimEnd('/')}/reset-password?token={token}";

        await _sharedRepository.SendPasswordResetEmailAsync(user.Email, resetLink, cancellationToken);

        return true;
    }
}
