using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VikashERP.Infrastructure.Data;
using VikashERP.Domain.Entities;
using VikashERP.Application.Features.Staff.DTOs;
using VikashERP.SharedKernel.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VikashERP.API.Controllers;

[Route("api/salary-configuration")]
[ApiController]
[Authorize(Roles = "Super Admin,Administrator")]
public class SalaryConfigurationController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SalaryConfigurationController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var configs = await _context.SalaryConfigurations
            .Include(c => c.User)
            .OrderBy(c => c.User.FirstName)
            .Select(c => new SalaryConfigDto
            {
                Id = c.Id,
                UserId = c.UserId,
                UserName = $"{c.User.FirstName} {c.User.LastName}".Trim(),
                UserRole = c.User.Role.ToFriendlyName(),
                BasicSalary = c.BasicSalary,
                IsActive = c.IsActive
            })
            .ToListAsync(cancellationToken);

        return Ok(configs);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSalaryConfigDto dto, CancellationToken cancellationToken)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == dto.UserId, cancellationToken);
        if (!userExists) return NotFound(new { Message = "User not found." });

        var alreadyExists = await _context.SalaryConfigurations.AnyAsync(c => c.UserId == dto.UserId, cancellationToken);
        if (alreadyExists) return BadRequest(new { Message = "Salary configuration already exists for this user." });

        var config = new SalaryConfiguration
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            BasicSalary = dto.BasicSalary,
            IsActive = dto.IsActive
        };

        _context.SalaryConfigurations.Add(config);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(new { Message = "Configuration created successfully." });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSalaryConfigDto dto, CancellationToken cancellationToken)
    {
        var config = await _context.SalaryConfigurations.FindAsync(new object[] { id }, cancellationToken);
        if (config == null) return NotFound(new { Message = "Configuration not found." });

        config.BasicSalary = dto.BasicSalary;
        config.IsActive = dto.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
        return Ok(new { Message = "Configuration updated successfully." });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var config = await _context.SalaryConfigurations.FindAsync(new object[] { id }, cancellationToken);
        if (config == null) return NotFound(new { Message = "Configuration not found." });

        _context.SalaryConfigurations.Remove(config);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Ok(new { Message = "Configuration deleted successfully." });
    }

    [HttpGet("users-by-role/{roleName}")]
    public async Task<IActionResult> GetUsersByRole(string roleName, CancellationToken cancellationToken)
    {
        var role = UserRoleExtensions.FromString(roleName);
        if (role == null) return BadRequest(new { Message = "Invalid role." });

        var users = await _context.Users
            .Where(u => u.Role == role.Value && u.IsActive)
            .OrderBy(u => u.FirstName)
            .Select(u => new 
            {
                Id = u.Id,
                Name = $"{u.FirstName} {u.LastName}".Trim(),
                Email = u.Email
            })
            .ToListAsync(cancellationToken);

        return Ok(users);
    }
}
