using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikashERP.Web.Auth;

namespace VikashERP.Web.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly TokenValidator _tokenValidator;

    public AccountController(TokenValidator tokenValidator)
    {
        _tokenValidator = tokenValidator;
    }

    /// <summary>
    /// Browser redirect sign-in (sets auth cookie on the user's actual HTTP request).
    /// </summary>
    [HttpGet("signin")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn(
        [FromQuery] string token,
        [FromQuery] string? refreshToken,
        [FromQuery] bool rememberMe = false,
        [FromQuery] int expiresIn = 0,
        [FromQuery] string? returnUrl = null)
    {
        if (!await TrySignInAsync(token, refreshToken, rememberMe, expiresIn))
            return Redirect("/login");

        return Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
    }

    [HttpPost("signin")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> SignInPost([FromBody] SignInRequest request)
    {
        if (!await TrySignInAsync(request.Token, request.RefreshToken, request.RememberMe, request.ExpiresIn))
            return Unauthorized(new { Message = "Invalid or expired token." });

        return Ok(new { Message = "Signed in successfully." });
    }

    [HttpGet("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/login");
    }

    private async Task<bool> TrySignInAsync(
        string? token,
        string? refreshToken,
        bool rememberMe,
        int expiresIn)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;

        var principal = _tokenValidator.ValidateToken(token);
        if (principal is null)
            return false;

        var claims = principal.Claims.ToList();

        foreach (var roleClaim in claims.Where(c => c.Type == "roles").ToList())
        {
            if (!claims.Any(c => c.Type == ClaimTypes.Role && c.Value == roleClaim.Value))
                claims.Add(new Claim(ClaimTypes.Role, roleClaim.Value));
        }

        if (!claims.Any(c => c.Type == ClaimTypes.Name))
        {
            var nameClaim = claims.FirstOrDefault(c => c.Type is "name" or ClaimTypes.Name);
            if (nameClaim is not null)
                claims.Add(new Claim(ClaimTypes.Name, nameClaim.Value));
        }

        claims.Add(new Claim("access_token", token));

        if (!string.IsNullOrWhiteSpace(refreshToken))
            claims.Add(new Claim("refresh_token", refreshToken));

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme,
            ClaimTypes.Name,
            ClaimTypes.Role);

        var expiry = expiresIn > 0
            ? DateTimeOffset.UtcNow.AddSeconds(expiresIn)
            : DateTimeOffset.UtcNow.AddHours(24);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = expiry
            });

        return true;
    }
}

public class SignInRequest
{
    public string Token { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public int ExpiresIn { get; set; }
    public bool RememberMe { get; set; }
}
