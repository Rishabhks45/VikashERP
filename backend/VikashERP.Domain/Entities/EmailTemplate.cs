namespace VikashERP.Domain.Entities;

using VikashERP.SharedKernel.Enums;

public class EmailTemplate
{
    public int Id { get; set; }
    public string TemplateKey { get; set; } = string.Empty;
    public NotificationType NotificationType { get; set; } = NotificationType.Email;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Headline { get; set; } = string.Empty;
    public string BodyHtml { get; set; } = string.Empty;
    public string? Preheader { get; set; }
    public string? ButtonLabel { get; set; }
    public string? ButtonLinkToken { get; set; }
    public string AvailableTokens { get; set; } = "[]";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
