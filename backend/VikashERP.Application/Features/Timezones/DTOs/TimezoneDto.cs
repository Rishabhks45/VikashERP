using System;

namespace VikashERP.Application.Features.Timezones.DTOs;

public class TimezoneDto
{
    public Guid TimezoneId { get; set; }
    public string IanaId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Abbreviation { get; set; }
    public bool IsDefault { get; set; }
}
