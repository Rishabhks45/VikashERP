using FluentValidation;
using MediatR;
using VikashERP.Application.Features.Email.DTOs;
using VikashERP.Application.Features.Email.Validators;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Email.Commands;

public class UpdateEmailTemplateCommand : IRequest<EmailTemplateDetailDto?>
{
    public UpdateEmailTemplateRequest Request { get; set; } = null!;
}

public class UpdateEmailTemplateCommandHandler : IRequestHandler<UpdateEmailTemplateCommand, EmailTemplateDetailDto?>
{
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly UpdateEmailTemplateRequestValidator _validator;

    public UpdateEmailTemplateCommandHandler(
        IEmailTemplateService emailTemplateService,
        UpdateEmailTemplateRequestValidator validator)
    {
        _emailTemplateService = emailTemplateService;
        _validator = validator;
    }

    public async Task<EmailTemplateDetailDto?> Handle(UpdateEmailTemplateCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command.Request, cancellationToken);
        return await _emailTemplateService.UpdateAsync(command.Request, cancellationToken);
    }
}

public class CreateEmailTemplateCommand : IRequest<EmailTemplateDetailDto>
{
    public CreateEmailTemplateRequest Request { get; set; } = null!;
}

public class CreateEmailTemplateCommandHandler : IRequestHandler<CreateEmailTemplateCommand, EmailTemplateDetailDto>
{
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly CreateEmailTemplateRequestValidator _validator;

    public CreateEmailTemplateCommandHandler(
        IEmailTemplateService emailTemplateService,
        CreateEmailTemplateRequestValidator validator)
    {
        _emailTemplateService = emailTemplateService;
        _validator = validator;
    }

    public async Task<EmailTemplateDetailDto> Handle(CreateEmailTemplateCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command.Request, cancellationToken);
        var created = await _emailTemplateService.CreateAsync(command.Request, cancellationToken);
        return created ?? throw new InvalidOperationException("Failed to create email template.");
    }
}

public class DeleteEmailTemplateCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteEmailTemplateCommandHandler : IRequestHandler<DeleteEmailTemplateCommand, bool>
{
    private readonly IEmailTemplateService _emailTemplateService;

    public DeleteEmailTemplateCommandHandler(IEmailTemplateService emailTemplateService)
    {
        _emailTemplateService = emailTemplateService;
    }

    public Task<bool> Handle(DeleteEmailTemplateCommand command, CancellationToken cancellationToken) =>
        _emailTemplateService.DeleteAsync(command.Id, cancellationToken);
}

public class PreviewEmailTemplateCommand : IRequest<EmailTemplatePreviewDto>
{
    public PreviewEmailTemplateRequest Request { get; set; } = null!;
}

public class PreviewEmailTemplateCommandHandler : IRequestHandler<PreviewEmailTemplateCommand, EmailTemplatePreviewDto>
{
    private readonly IEmailTemplateService _emailTemplateService;

    public PreviewEmailTemplateCommandHandler(IEmailTemplateService emailTemplateService)
    {
        _emailTemplateService = emailTemplateService;
    }

    public Task<EmailTemplatePreviewDto> Handle(PreviewEmailTemplateCommand command, CancellationToken cancellationToken)
    {
        var content = _emailTemplateService.Preview(command.Request);
        return Task.FromResult(new EmailTemplatePreviewDto(content.Subject, content.HtmlBody));
    }
}
