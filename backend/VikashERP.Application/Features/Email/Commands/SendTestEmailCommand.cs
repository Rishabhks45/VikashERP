using MediatR;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Email.Commands;

public class SendTestEmailCommand : IRequest<bool>
{
    public string ToEmail { get; set; } = string.Empty;
    public string TemplateKey { get; set; } = string.Empty;
    public Dictionary<string, string> Tokens { get; set; } = new();
}

public class SendTestEmailCommandHandler : IRequestHandler<SendTestEmailCommand, bool>
{
    private readonly ISharedRepository _sharedRepository;

    public SendTestEmailCommandHandler(ISharedRepository sharedRepository)
    {
        _sharedRepository = sharedRepository;
    }

    public Task<bool> Handle(SendTestEmailCommand request, CancellationToken cancellationToken) =>
        _sharedRepository.SendTemplateEmailAsync(
            request.TemplateKey,
            request.ToEmail,
            request.Tokens,
            cancellationToken);
}
