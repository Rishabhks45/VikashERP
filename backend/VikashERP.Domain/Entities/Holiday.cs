using System;

namespace VikashERP.Domain.Entities;

public class Holiday : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public DateOnly Date { get; set; }

    public bool IsRecurring { get; set; }

    public string? Description { get; set; }
}