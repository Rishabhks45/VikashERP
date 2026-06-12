using VikashERP.SharedKernel.Enums;

namespace VikashERP.Web.Models.Forms;

public class UpsertEmailTemplateFormModel
{
    public int Id { get; set; }

    public NotificationType NotificationType { get; set; } = NotificationType.Email;

    public string TemplateKey { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Headline { get; set; } = string.Empty;

    public string BodyHtml { get; set; } = string.Empty;

    public string? Preheader { get; set; }

    public string? ButtonLabel { get; set; }

    public string? ButtonLinkToken { get; set; }

    public string AvailableTokensText { get; set; } = string.Empty;

    public List<string> AvailableTokens { get; set; } = [];

    public bool IsActive { get; set; } = true;

    public bool IsCreate => Id == 0;

    public IEnumerable<string> TokenList =>
        AvailableTokens.Count > 0
            ? AvailableTokens
            : AvailableTokensText.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
