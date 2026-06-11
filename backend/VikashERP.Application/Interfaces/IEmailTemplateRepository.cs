using VikashERP.Domain.Entities;

namespace VikashERP.Application.Interfaces;

public interface IEmailTemplateRepository
{
    Task<IReadOnlyList<EmailTemplate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<EmailTemplate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<EmailTemplate?> GetByKeyAsync(string templateKey, CancellationToken cancellationToken = default);
    Task<bool> ExistsByKeyAsync(string templateKey, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    Task AddAsync(EmailTemplate template, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<EmailTemplate> templates, CancellationToken cancellationToken = default);
    void Update(EmailTemplate template);
    void Remove(EmailTemplate template);
}
