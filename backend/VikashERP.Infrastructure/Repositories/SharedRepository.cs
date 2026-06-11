using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Infrastructure.Data;
using VikashERP.SharedKernel.Common.Interfaces;
using VikashERP.SharedKernel.Email;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Repositories;

/// <summary>
/// HE-style shared repo: load template from DB, replace placeholders, send email.
/// </summary>
public class SharedRepository : ISharedRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailSender _emailSender;
    private readonly IConfiguration _configuration;

    public SharedRepository(
        ApplicationDbContext context,
        IEmailSender emailSender,
        IConfiguration configuration)
    {
        _context = context;
        _emailSender = emailSender;
        _configuration = configuration;
    }

    public Task<EmailTemplate?> GetEmailTemplateAsync(EmailTemplateType templateType, CancellationToken cancellationToken = default) =>
        _context.EmailTemplates.AsNoTracking()
            .FirstOrDefaultAsync(
                t => t.TemplateKey == templateType.ToString() && t.IsActive,
                cancellationToken);

    public Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetLink, CancellationToken cancellationToken = default) =>
        SendEmailAsync(
            EmailTemplateType.ForgotPassword,
            toEmail,
            new Dictionary<string, string> { [EmailTemplateTokens.ResetLink] = resetLink },
            cancellationToken);

    public Task<bool> SendPasswordResetSuccessEmailAsync(string toEmail, string userName, CancellationToken cancellationToken = default) =>
        SendEmailAsync(
            EmailTemplateType.PasswordResetSuccess,
            toEmail,
            new Dictionary<string, string> { [EmailTemplateTokens.UserName] = userName },
            cancellationToken);

    public Task<bool> SendWelcomeEmailAsync(string toEmail, string userName, CancellationToken cancellationToken = default) =>
        SendEmailAsync(
            EmailTemplateType.Welcome,
            toEmail,
            new Dictionary<string, string> { [EmailTemplateTokens.UserName] = userName },
            cancellationToken);

    public Task<bool> SendTemplateEmailAsync(
        string templateKey,
        string toEmail,
        IReadOnlyDictionary<string, string>? placeholders = null,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<EmailTemplateType>(templateKey, ignoreCase: true, out var templateType))
            return Task.FromResult(false);

        return SendEmailAsync(templateType, toEmail, placeholders, cancellationToken);
    }

    private async Task<bool> SendEmailAsync(
        EmailTemplateType templateType,
        string toEmail,
        IReadOnlyDictionary<string, string>? placeholders,
        CancellationToken cancellationToken)
    {
        var template = await GetEmailTemplateAsync(templateType, cancellationToken);
        if (template is null)
            return false;

        var tokens = BuildPlaceholders(placeholders);
        var content = RenderTemplate(template, tokens);
        return await _emailSender.SendEmailAsync(toEmail.Trim(), content.Subject, content.HtmlBody);
    }

    private static EmailTemplateContent RenderTemplate(EmailTemplate template, Dictionary<string, string> tokens) =>
        EmailTemplateRenderer.Render(
            template.Subject,
            template.Headline,
            template.BodyHtml,
            template.Preheader,
            template.ButtonLabel,
            template.ButtonLinkToken,
            tokens);

    private Dictionary<string, string> BuildPlaceholders(IReadOnlyDictionary<string, string>? placeholders)
    {
        var tokens = new Dictionary<string, string>
        {
            [EmailTemplateTokens.LoginUrl] = GetLoginUrl(),
            [EmailTemplateTokens.ExpiryMinutes] = "15",
            [EmailTemplateTokens.ContactPhone] = _configuration["Company:ContactPhone"] ?? "+91 98765 43210",
            [EmailTemplateTokens.ContactEmail] = _configuration["Company:ContactEmail"] ?? "support@vikashironix.com"
        };

        if (placeholders is null)
            return tokens;

        foreach (var item in placeholders)
            tokens[item.Key] = item.Value ?? string.Empty;

        return tokens;
    }

    private string GetLoginUrl()
    {
        var baseUrl = _configuration["ClientApp:BaseUrl"] ?? "https://localhost:7297";
        return $"{baseUrl.TrimEnd('/')}/login";
    }
}
