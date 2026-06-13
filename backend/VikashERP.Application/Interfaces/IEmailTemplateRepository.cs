using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Application.Interfaces;

public interface IEmailTemplateRepository
{
    Task<IReadOnlyList<EmailTemplate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<EmailTemplate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<EmailTemplate?> GetByKeyAsync(string templateKey, NotificationType notificationType = NotificationType.Email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByKeyAsync(string templateKey, NotificationType notificationType, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    Task AddAsync(EmailTemplate template, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<EmailTemplate> templates, CancellationToken cancellationToken = default);
    void Update(EmailTemplate template);
    void Remove(EmailTemplate template);
}
