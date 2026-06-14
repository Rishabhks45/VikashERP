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
    private readonly IOrganizationRepository _organizationRepository;

    public SharedRepository(
        ApplicationDbContext context,
        IEmailSender emailSender,
        IConfiguration configuration,
        IOrganizationRepository organizationRepository)
    {
        _context = context;
        _emailSender = emailSender;
        _configuration = configuration;
        _organizationRepository = organizationRepository;
    }

    public Task<EmailTemplate?> GetEmailTemplateAsync(EmailTemplateType templateType, CancellationToken cancellationToken = default) =>
        _context.EmailTemplates.AsNoTracking()
            .FirstOrDefaultAsync(
                t => t.TemplateKey == templateType.ToString()
                     && t.NotificationType == NotificationType.Email
                     && t.IsActive,
                cancellationToken);

    public async Task<long> GetNextSequenceValueAsync(string sequenceName, CancellationToken cancellationToken = default)
    {
        using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = $"SELECT nextval('{sequenceName}')";
        await _context.Database.OpenConnectionAsync(cancellationToken);
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt64(result);
    }

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

    public Task<bool> SendWelcomeEmailAsync(
        string toEmail,
        string userName,
        string? temporaryPassword = null,
        CancellationToken cancellationToken = default) =>
        SendEmailAsync(
            EmailTemplateType.Welcome,
            toEmail,
            new Dictionary<string, string>
            {
                [EmailTemplateTokens.UserName] = userName,
                [EmailTemplateTokens.UserEmail] = toEmail,
                [EmailTemplateTokens.Password] = temporaryPassword ?? string.Empty
            },
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

        var tokens = await BuildPlaceholdersAsync(placeholders, cancellationToken);
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

    private async Task<Dictionary<string, string>> BuildPlaceholdersAsync(
        IReadOnlyDictionary<string, string>? placeholders,
        CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetOrCreateDefaultAsync(cancellationToken);

        var tokens = new Dictionary<string, string>
        {
            [EmailTemplateTokens.LoginUrl] = GetLoginUrl(),
            [EmailTemplateTokens.ExpiryMinutes] = "15",
            [EmailTemplateTokens.ContactPhone] = organization.Phone ?? string.Empty,
            [EmailTemplateTokens.ContactEmail] = organization.Email ?? string.Empty
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
