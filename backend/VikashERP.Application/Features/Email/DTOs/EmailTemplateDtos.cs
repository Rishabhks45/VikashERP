using VikashERP.SharedKernel.Enums;

namespace VikashERP.Application.Features.Email.DTOs;

public record EmailTemplateListItemDto(
    Guid Id,
    string TemplateKey,
    NotificationType NotificationType,
    string DisplayName,
    string Description,
    string Subject,
    bool IsActive,
    DateTime UpdatedAt);

public record EmailTemplateDetailDto(
    Guid Id,
    string TemplateKey,
    NotificationType NotificationType,    string DisplayName,
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
    Guid Id,
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
    NotificationType NotificationType,
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
