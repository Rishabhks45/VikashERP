using System.Net.Http.Json;
using System.Text.Json;

namespace VikashERP.Web.Services;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string email, string password);
    Task<bool> ForgotPasswordAsync(string email);
    Task<bool> ResetPasswordAsync(string token, string newPassword);
}

public class AuthService : IAuthService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly IHttpClientFactory _clientFactory;

    public AuthService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var client = _clientFactory.CreateClient("VikashERP.Api.Anonymous");
        var response = await client.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            return AuthResult.Fail("Invalid email or password.");

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            return AuthResult.Fail($"Login failed ({(int)response.StatusCode}). {body}");
        }

        var result = await response.Content.ReadFromJsonAsync<UserLoginResponse>(JsonOptions);
        if (result is null || string.IsNullOrWhiteSpace(result.Token))
            return AuthResult.Fail("Invalid response from server.");

        return AuthResult.Ok(result);
    }

    public async Task<bool> ForgotPasswordAsync(string email)
    {
        var client = _clientFactory.CreateClient("VikashERP.Api.Anonymous");
        var response = await client.PostAsJsonAsync("api/auth/forgot-password", new { Email = email });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        var client = _clientFactory.CreateClient("VikashERP.Api.Anonymous");
        var response = await client.PostAsJsonAsync("api/auth/reset-password", new { Token = token, NewPassword = newPassword });
        return response.IsSuccessStatusCode;
    }
}

public class AuthResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public UserLoginResponse? Response { get; init; }

    public static AuthResult Ok(UserLoginResponse response) => new() { Success = true, Response = response };
    public static AuthResult Fail(string message) => new() { Success = false, Message = message };
}

public class UserLoginResponse
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public Guid? CustomerId { get; set; }
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiry { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
}
