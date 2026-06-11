using Microsoft.EntityFrameworkCore;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Infrastructure.Data;

namespace VikashERP.Infrastructure.Repositories;

public class EmailTemplateRepository : IEmailTemplateRepository
{
    private readonly ApplicationDbContext _context;

    public EmailTemplateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<EmailTemplate>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.EmailTemplates
            .AsNoTracking()
            .OrderBy(t => t.DisplayName)
            .ToListAsync(cancellationToken);

    public async Task<EmailTemplate?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _context.EmailTemplates.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<EmailTemplate?> GetByKeyAsync(string templateKey, CancellationToken cancellationToken = default) =>
        await _context.EmailTemplates.FirstOrDefaultAsync(
            t => t.TemplateKey == templateKey && t.IsActive,
            cancellationToken);

    public async Task<bool> ExistsByKeyAsync(string templateKey, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.EmailTemplates.Where(t => t.TemplateKey == templateKey);
        if (excludeId.HasValue)
            query = query.Where(t => t.Id != excludeId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default) =>
        await _context.EmailTemplates.AnyAsync(cancellationToken);

    public async Task AddAsync(EmailTemplate template, CancellationToken cancellationToken = default) =>
        await _context.EmailTemplates.AddAsync(template, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<EmailTemplate> templates, CancellationToken cancellationToken = default) =>
        await _context.EmailTemplates.AddRangeAsync(templates, cancellationToken);

    public void Update(EmailTemplate template)
    {
        var entry = _context.Entry(template);
        if (entry.State == EntityState.Detached)
            _context.EmailTemplates.Update(template);

        entry.Property(e => e.CreatedAt).IsModified = false;
    }

    public void Remove(EmailTemplate template) =>
        _context.EmailTemplates.Remove(template);
}
