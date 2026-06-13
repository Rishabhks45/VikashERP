using System;

namespace VikashERP.Web.Models;

public class CategoryListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TotalProducts { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CategoryFormModel
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsNewCategory { get; set; } = true;
}