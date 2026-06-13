using MediatR;
using VikashERP.Application.Features.Email.DTOs;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Email.Queries;

public record GetEmailTemplatesQuery : IRequest<IReadOnlyList<EmailTemplateListItemDto>>;

public class GetEmailTemplatesQueryHandler : IRequestHandler<GetEmailTemplatesQuery, IReadOnlyList<EmailTemplateListItemDto>>
{
    private readonly IEmailTemplateService _emailTemplateService;

    public GetEmailTemplatesQueryHandler(IEmailTemplateService emailTemplateService)
    {
        _emailTemplateService = emailTemplateService;
    }

    public Task<IReadOnlyList<EmailTemplateListItemDto>> Handle(GetEmailTemplatesQuery request, CancellationToken cancellationToken) =>
        _emailTemplateService.GetAllAsync(cancellationToken);
}

public record GetEmailTemplateByIdQuery(Guid Id) : IRequest<EmailTemplateDetailDto?>;

public class GetEmailTemplateByIdQueryHandler : IRequestHandler<GetEmailTemplateByIdQuery, EmailTemplateDetailDto?>
{
    private readonly IEmailTemplateService _emailTemplateService;

    public GetEmailTemplateByIdQueryHandler(IEmailTemplateService emailTemplateService)
    {
        _emailTemplateService = emailTemplateService;
    }

    public Task<EmailTemplateDetailDto?> Handle(GetEmailTemplateByIdQuery request, CancellationToken cancellationToken) =>
        _emailTemplateService.GetByIdAsync(request.Id, cancellationToken);
}
