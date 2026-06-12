using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VikashERP.Infrastructure.Data;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;
using VikashERP.SharedKernel.Common.Interfaces;
using VikashERP.SharedKernel.Settings;
using VikashERP.Application.Features.Users.DTOs;
using VikashERP.Application.Features.Users.Validators;

namespace VikashERP.API.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEncryptionService _encryptionService;
    private readonly Microsoft.Extensions.Options.IOptions<EncryptionSettings> _options;
    private readonly CreateUserAccountDtoValidator _createUserValidator;
    private readonly UpdateUserAccountDtoValidator _updateUserValidator;

    public UsersController(
        ApplicationDbContext context, 
        IEncryptionService encryptionService,
        Microsoft.Extensions.Options.IOptions<EncryptionSettings> options,
        CreateUserAccountDtoValidator createUserValidator,
        UpdateUserAccountDtoValidator updateUserValidator)
    {
        _context = context;
        _encryptionService = encryptionService;
        _options = options;
        _createUserValidator = createUserValidator;
        _updateUserValidator = updateUserValidator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .OrderBy(u => u.Email)
            .Select(u => new UserAccountDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role.ToFriendlyName(),
                ProfilePictureUrl = u.ProfilePictureUrl,
                CreatedAt = u.CreatedAt,
                IsActive = u.IsActive,
                LastLoginAt = u.LastLoginAt
            })
            .ToListAsync(cancellationToken);

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserAccountDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _createUserValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)) });
        }

        var exists = await _context.Users.AnyAsync(u => u.Email.ToLower() == dto.Email.Trim().ToLower(), cancellationToken);
        if (exists)
            return BadRequest(new { Message = "A user with this email address already exists." });

        var role = UserRoleExtensions.FromString(dto.Role) ?? UserRole.Employee;
        var encryptedPassword = await _encryptionService.EncryptAsync(dto.Password, _options.Value.MasterKey);

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            Email = dto.Email.Trim().ToLowerInvariant(),
            PasswordHash = encryptedPassword,
            Role = role,
            CreatedAt = DateTime.UtcNow,
            IsActive = dto.IsActive
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserAccountDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateUserValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)) });
        }

        var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken);
        if (user is null)
            return NotFound();

        var exists = await _context.Users.AnyAsync(u => u.Email.ToLower() == dto.Email.Trim().ToLower() && u.Id != id, cancellationToken);
        if (exists)
            return BadRequest(new { Message = "A user with this email address already exists." });

        var role = UserRoleExtensions.FromString(dto.Role) ?? UserRole.Employee;

        user.FirstName = dto.FirstName.Trim();
        user.LastName = dto.LastName.Trim();
        user.Email = dto.Email.Trim().ToLowerInvariant();
        user.Role = role;
        user.IsActive = dto.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            user.PasswordHash = await _encryptionService.EncryptAsync(dto.Password, _options.Value.MasterKey);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken);
        if (user is null)
            return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return Ok();
    }
}
