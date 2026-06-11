using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikashERP.Application.Features.Auth.Commands;
using VikashERP.Application.Features.Auth.DTOs;

namespace VikashERP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new LoginCommand
        {
            Email = request.Email,
            Password = request.Password
        }, cancellationToken);

        if (result is null)
            return Unauthorized(new { Message = "Invalid email or password" });

        return Ok(result);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authHeader = Request.Headers.Authorization.FirstOrDefault();
        var accessToken = request.AccessToken;

        if (string.IsNullOrEmpty(accessToken)
            && !string.IsNullOrEmpty(authHeader)
            && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            accessToken = authHeader["Bearer ".Length..].Trim();
        }

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(request.RefreshToken))
            return Unauthorized(new { Message = "Access token and refresh token are required" });

        var result = await _mediator.Send(new RefreshTokenCommand
        {
            AccessToken = accessToken,
            RefreshToken = request.RefreshToken
        }, cancellationToken);

        if (result is null)
            return Unauthorized(new { Message = "Invalid or expired refresh token" });

        return Ok(result);
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ForgotPasswordCommand { Email = request.Email }, cancellationToken);
        return Ok(new { Message = "If the email exists, a reset link has been sent." });
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var success = await _mediator.Send(new ResetPasswordCommand
        {
            Token = request.Token,
            NewPassword = request.NewPassword
        }, cancellationToken);

        if (!success)
            return BadRequest(new { Message = "Invalid or expired reset token." });

        return Ok(new { Message = "Password reset successfully." });
    }
}

public class RefreshTokenRequest
{
    public string? AccessToken { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}
