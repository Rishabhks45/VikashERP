namespace VikashERP.Web.Models;

public class TimezoneListItemDto
{
    public Guid TimezoneId { get; set; }
    public string IanaId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Abbreviation { get; set; }
}
