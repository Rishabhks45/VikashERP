using System;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Application.Features.Staff.DTOs;

public class SalaryConfigDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;
    public decimal BasicSalary { get; set; }
    public bool IsActive { get; set; }
}

public class CreateSalaryConfigDto
{
    public Guid UserId { get; set; }
    public decimal BasicSalary { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateSalaryConfigDto
{
    public decimal BasicSalary { get; set; }
    public bool IsActive { get; set; }
}
