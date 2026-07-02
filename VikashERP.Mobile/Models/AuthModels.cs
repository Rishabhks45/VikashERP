using System.Text.Json.Serialization;

namespace VikashERP.Mobile.Models;

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Matches backend UserLoginResponse exactly:
/// UserId, UserName, Email, Role, ProfilePictureUrl, CustomerId,
/// Token, RefreshToken, RefreshTokenExpiry, TokenType, ExpiresIn
/// </summary>
public class LoginResponse
{
    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }

    [JsonPropertyName("userName")]
    public string UserName { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("profilePictureUrl")]
    public string? ProfilePictureUrl { get; set; }

    [JsonPropertyName("customerId")]
    public Guid? CustomerId { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;

    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = string.Empty;

    [JsonPropertyName("refreshTokenExpiry")]
    public DateTime RefreshTokenExpiry { get; set; }

    [JsonPropertyName("tokenType")]
    public string TokenType { get; set; } = "Bearer";

    [JsonPropertyName("expiresIn")]
    public int ExpiresIn { get; set; }
}
