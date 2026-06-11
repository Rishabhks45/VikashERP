using VikashERP.Application.Features.Email.DTOs;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Email;

namespace VikashERP.Infrastructure.Email;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly IEmailTemplateRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public EmailTemplateService(IEmailTemplateRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<EmailTemplateListItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var templates = await _repository.GetAllAsync(cancellationToken);
        return templates.Select(MapListItem).ToList();
    }

    public async Task<EmailTemplateDetailDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var template = await _repository.GetByIdAsync(id, cancellationToken);
        return template is null ? null : MapDetail(template);
    }

    public async Task<EmailTemplateDetailDto?> UpdateAsync(UpdateEmailTemplateRequest request, CancellationToken cancellationToken = default)
    {
        var template = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (template is null)
            return null;

        template.DisplayName = request.DisplayName.Trim();
        template.Description = request.Description.Trim();
        template.Subject = request.Subject.Trim();
        template.Headline = request.Headline.Trim();
        template.BodyHtml = request.BodyHtml;
        template.Preheader = string.IsNullOrWhiteSpace(request.Preheader) ? null : request.Preheader.Trim();
        template.ButtonLabel = string.IsNullOrWhiteSpace(request.ButtonLabel) ? null : request.ButtonLabel.Trim();
        template.ButtonLinkToken = string.IsNullOrWhiteSpace(request.ButtonLinkToken) ? null : request.ButtonLinkToken.Trim();
        template.IsActive = request.IsActive;
        template.CreatedAt = EnsureUtc(template.CreatedAt);
        template.UpdatedAt = DateTime.UtcNow;

        _repository.Update(template);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapDetail(template);
    }

    public async Task<EmailTemplateDetailDto?> CreateAsync(CreateEmailTemplateRequest request, CancellationToken cancellationToken = default)
    {
        var templateKey = request.TemplateKey.Trim();
        if (string.IsNullOrWhiteSpace(templateKey))
            throw new InvalidOperationException("Template key is required.");

        if (await _repository.ExistsByKeyAsync(templateKey, request.NotificationType, cancellationToken: cancellationToken))
            throw new InvalidOperationException($"Template key '{templateKey}' already exists for {request.NotificationType}.");

        var now = DateTime.UtcNow;
        var template = new EmailTemplate
        {
            TemplateKey = templateKey,
            NotificationType = request.NotificationType,
            DisplayName = request.DisplayName.Trim(),
            Description = request.Description.Trim(),
            Subject = request.Subject.Trim(),
            Headline = request.Headline.Trim(),
            BodyHtml = request.BodyHtml,
            Preheader = string.IsNullOrWhiteSpace(request.Preheader) ? null : request.Preheader.Trim(),
            ButtonLabel = string.IsNullOrWhiteSpace(request.ButtonLabel) ? null : request.ButtonLabel.Trim(),
            ButtonLinkToken = string.IsNullOrWhiteSpace(request.ButtonLinkToken) ? null : request.ButtonLinkToken.Trim(),
            AvailableTokens = NormalizeAvailableTokens(request.AvailableTokens),
            IsActive = request.IsActive,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _repository.AddAsync(template, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapDetail(template);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var template = await _repository.GetByIdAsync(id, cancellationToken);
        if (template is null)
            return false;

        _repository.Remove(template);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public EmailTemplateContent Preview(PreviewEmailTemplateRequest request) =>
        EmailTemplateRenderer.Render(
            request.Subject,
            request.Headline,
            request.BodyHtml,
            request.Preheader,
            request.ButtonLabel,
            request.ButtonLinkToken,
            request.Tokens);

    private static EmailTemplateDetailDto MapDetail(EmailTemplate template) =>
        new(
            template.Id,
            template.TemplateKey,
            template.NotificationType,
            template.DisplayName,
            template.Description,
            template.Subject,
            template.Headline,
            template.BodyHtml,
            template.Preheader,
            template.ButtonLabel,
            template.ButtonLinkToken,
            EmailTemplateRenderer.ParseAvailableTokens(template.AvailableTokens),
            template.IsActive,
            template.UpdatedAt);

    private static EmailTemplateListItemDto MapListItem(EmailTemplate template) =>
        new(
            template.Id,
            template.TemplateKey,
            template.NotificationType,
            template.DisplayName,
            template.Description,
            template.Subject,
            template.IsActive,
            template.UpdatedAt);

    private static string NormalizeAvailableTokens(string availableTokens)
    {
        if (string.IsNullOrWhiteSpace(availableTokens))
            return EmailTemplateRenderer.SerializeAvailableTokens([]);

        var tokens = availableTokens
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(token => !string.IsNullOrWhiteSpace(token))
            .Distinct(StringComparer.Ordinal)
            .ToList();

        return EmailTemplateRenderer.SerializeAvailableTokens(tokens);
    }

    private static DateTime EnsureUtc(DateTime value) =>
        value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
}
