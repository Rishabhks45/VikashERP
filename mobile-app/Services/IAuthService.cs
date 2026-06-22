using System.Threading.Tasks;

namespace MobileApp.Services;

public interface IAuthService
{
    bool IsLoggedIn { get; }
    string? AccessToken { get; }
    string? UserRole { get; }
    string? UserName { get; }

    Task<bool> LoginAsync(string email, string password);
    Task LogoutAsync();
    Task<bool> InitializeAsync();
}
