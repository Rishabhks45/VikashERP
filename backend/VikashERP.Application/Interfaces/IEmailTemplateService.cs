using VikashERP.Application.Features.Email.DTOs;
using VikashERP.SharedKernel.Email;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Application.Interfaces;

public interface IEmailTemplateService
{
    Task<IReadOnlyList<EmailTemplateListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<EmailTemplateDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<EmailTemplateDetailDto?> CreateAsync(CreateEmailTemplateRequest request, CancellationToken cancellationToken = default);
    Task<EmailTemplateDetailDto?> UpdateAsync(UpdateEmailTemplateRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    EmailTemplateContent Preview(PreviewEmailTemplateRequest request);
}
