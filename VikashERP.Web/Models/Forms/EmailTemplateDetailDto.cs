using VikashERP.SharedKernel.Enums;

namespace VikashERP.Web.Models.Forms;

public class EmailTemplateDetailDto
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

    public List<string> AvailableTokens { get; set; } = [];

    public bool IsActive { get; set; }

    public static UpsertEmailTemplateFormModel ToFormModel(EmailTemplateDetailDto detail) => new()
    {
        Id = detail.Id,
        NotificationType = detail.NotificationType,
        TemplateKey = detail.TemplateKey,
        DisplayName = detail.DisplayName,
        Description = detail.Description,
        Subject = detail.Subject,
        Headline = detail.Headline,
        BodyHtml = detail.BodyHtml,
        Preheader = detail.Preheader,
        ButtonLabel = detail.ButtonLabel,
        ButtonLinkToken = detail.ButtonLinkToken,
        AvailableTokens = detail.AvailableTokens,
        AvailableTokensText = string.Join(", ", detail.AvailableTokens),
        IsActive = detail.IsActive
    };
}
