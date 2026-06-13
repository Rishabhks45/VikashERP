using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace VikashERP.Web.Auth;

public class JwtTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHttpClientFactory _clientFactory;
    private readonly TokenValidator _tokenValidator;

    public JwtTokenHandler(
        IHttpContextAccessor httpContextAccessor,
        IHttpClientFactory clientFactory,
        TokenValidator tokenValidator)
    {
        _httpContextAccessor = httpContextAccessor;
        _clientFactory = clientFactory;
        _tokenValidator = tokenValidator;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.RequestUri?.AbsolutePath.Contains("refresh-token", StringComparison.OrdinalIgnoreCase) == true)
            return await base.SendAsync(request, cancellationToken);

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.User?.Identity?.IsAuthenticated == true)
        {
            var accessToken = httpContext.User.FindFirst("access_token")?.Value;
            var refreshToken = httpContext.User.FindFirst("refresh_token")?.Value;

            if (!string.IsNullOrEmpty(accessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized && !string.IsNullOrEmpty(refreshToken))
            {
                var refreshed = await TryRefreshTokenAsync(accessToken!, refreshToken, cancellationToken);
                if (refreshed is not null)
                {
                    await SignInWithTokensAsync(httpContext, refreshed, cancellationToken);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshed.Token);
                    response.Dispose();
                    return await base.SendAsync(request, cancellationToken);
                }
            }

            return response;
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<UserLoginResponse?> TryRefreshTokenAsync(
        string accessToken,
        string refreshToken,
        CancellationToken cancellationToken)
    {
        var client = _clientFactory.CreateClient("VikashERP.Api.Anonymous");
        var payload = new { AccessToken = accessToken, RefreshToken = refreshToken };
        var result = await client.PostAsJsonAsync("api/auth/refresh-token", payload, cancellationToken);

        if (!result.IsSuccessStatusCode)
            return null;

        return await result.Content.ReadFromJsonAsync<UserLoginResponse>(cancellationToken: cancellationToken);
    }

    private async Task SignInWithTokensAsync(
        HttpContext httpContext,
        UserLoginResponse login,
        CancellationToken cancellationToken)
    {
        var principal = _tokenValidator.ValidateToken(login.Token);
        if (principal is null)
            return;

        var claims = principal.Claims
            .Where(c => c.Type is not "access_token" and not "refresh_token")
            .ToList();

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

        claims.Add(new Claim("access_token", login.Token));
        claims.Add(new Claim("refresh_token", login.RefreshToken));

        var newIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme,
            ClaimTypes.Name,
            ClaimTypes.Role);

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(newIdentity),
            new AuthenticationProperties { IsPersistent = true });
    }

    private sealed class UserLoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
