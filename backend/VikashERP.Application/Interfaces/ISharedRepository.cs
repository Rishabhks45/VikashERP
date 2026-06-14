using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Application.Interfaces;

public interface ISharedRepository
{
    Task<EmailTemplate?> GetEmailTemplateAsync(EmailTemplateType templateType, CancellationToken cancellationToken = default);
    Task<long> GetNextSequenceValueAsync(string sequenceName, CancellationToken cancellationToken = default);

    Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetLink, CancellationToken cancellationToken = default);

    Task<bool> SendPasswordResetSuccessEmailAsync(string toEmail, string userName, CancellationToken cancellationToken = default);

    Task<bool> SendWelcomeEmailAsync(
        string toEmail,
        string userName,
        string? temporaryPassword = null,
        CancellationToken cancellationToken = default);

    Task<bool> SendTemplateEmailAsync(
        string templateKey,
        string toEmail,
        IReadOnlyDictionary<string, string>? placeholders = null,
        CancellationToken cancellationToken = default);
}
