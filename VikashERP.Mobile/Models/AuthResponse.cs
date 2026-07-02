namespace VikashERP.Mobile.Models;

public class AuthResponse
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public bool Success { get; set; }
    public string[]? Errors { get; set; }
    public string? UserName { get; set; }
    public string? Role { get; set; }
}