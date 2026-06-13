using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VikashERP.Application.Interfaces;

namespace VikashERP.Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationHours;
    private readonly SymmetricSecurityKey _signingKey;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public JwtProvider(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:SecretKey"]
            ?? throw new InvalidOperationException("JWT SecretKey is not configured.");
        _issuer = configuration["Jwt:Issuer"] ?? "VikashERP";
        _audience = configuration["Jwt:Audience"] ?? "VikashERP";

        if (!int.TryParse(configuration["Jwt:ExpirationHours"], out _expirationHours))
            _expirationHours = 24;

        _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        _tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _signingKey,
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = true,
            ValidAudience = _audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            NameClaimType = "name",
            RoleClaimType = "roles"
        };
    }

    public string GenerateToken(Guid userId, string email, string userName, string role, string? profilePictureUrl = null, Guid? customerId = null)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new("name", userName),
            new("roles", role),
            new(ClaimTypes.Role, role),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (!string.IsNullOrWhiteSpace(profilePictureUrl))
            claims.Add(new Claim("picture", profilePictureUrl));

        if (customerId.HasValue)
            claims.Add(new Claim("customer_id", customerId.Value.ToString()));

        var credentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(_expirationHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public ClaimsPrincipal? ValidateToken(string token, bool validateLifetime = true)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        var parameters = _tokenValidationParameters.Clone();
        parameters.ValidateLifetime = validateLifetime;

        try
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ValidateToken(token, parameters, out _);
        }
        catch
        {
            return null;
        }
    }

    public TokenValidationParameters GetTokenValidationParameters() => _tokenValidationParameters;
}
