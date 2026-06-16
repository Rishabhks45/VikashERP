using System;

namespace VikashERP.Web.Models;

public class HolidayDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool IsRecurring { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateHolidayDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool IsRecurring { get; set; }
    public string? Description { get; set; }
}

public class UpdateHolidayDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool IsRecurring { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class HolidayFormModel
{
    public string Name { get; set; } = string.Empty;
    public DateTime? Date { get; set; } = DateTime.Today;
    public bool IsRecurring { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
