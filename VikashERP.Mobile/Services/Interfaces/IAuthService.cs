using VikashERP.Mobile.Models;

namespace VikashERP.Mobile.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(string email, string password);
    Task<bool> RefreshTokenAsync();
    Task LogoutAsync();
    Task<bool> HasPinSetupAsync();
    Task SavePinAsync(string pin);
    Task<bool> VerifyPinAsync(string pin);
    Task EnableBiometricAsync(bool enable);
    Task<bool> IsBiometricEnabledAsync();
}