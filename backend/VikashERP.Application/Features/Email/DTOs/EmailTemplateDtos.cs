namespace VikashERP.Application.Features.Email.DTOs;

public record EmailTemplateListItemDto(
    int Id,
    string TemplateKey,
    string DisplayName,
    string Description,
    string Subject,
    bool IsActive,
    DateTime UpdatedAt);

public record EmailTemplateDetailDto(
    int Id,
    string TemplateKey,
    string DisplayName,
    string Description,
    string Subject,
    string Headline,
    string BodyHtml,
    string? Preheader,
    string? ButtonLabel,
    string? ButtonLinkToken,
    IReadOnlyList<string> AvailableTokens,
    bool IsActive,
    DateTime UpdatedAt);

public record UpdateEmailTemplateRequest(
    int Id,
    string DisplayName,
    string Description,
    string Subject,
    string Headline,
    string BodyHtml,
    string? Preheader,
    string? ButtonLabel,
    string? ButtonLinkToken,
    bool IsActive);

public record CreateEmailTemplateRequest(
    string TemplateKey,
    string DisplayName,
    string Description,
    string Subject,
    string Headline,
    string BodyHtml,
    string? Preheader,
    string? ButtonLabel,
    string? ButtonLinkToken,
    string AvailableTokens,
    bool IsActive);

public record PreviewEmailTemplateRequest(
    string Subject,
    string Headline,
    string BodyHtml,
    string? Preheader,
    string? ButtonLabel,
    string? ButtonLinkToken,
    Dictionary<string, string> Tokens);

public record EmailTemplatePreviewDto(string Subject, string HtmlBody);
