using FluentValidation;
using VikashERP.Application.Features.Email.DTOs;

namespace VikashERP.Application.Features.Email.Validators;

public class CreateEmailTemplateRequestValidator : AbstractValidator<CreateEmailTemplateRequest>
{
    public CreateEmailTemplateRequestValidator()
    {
        RuleFor(x => x.TemplateKey)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Subject)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Headline)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.BodyHtml)
            .NotEmpty()
            .Must(HaveValidPlainTextLength)
            .WithMessage("Body must not exceed 5000 characters.");
    }

    private static bool HaveValidPlainTextLength(string body) =>
        EmailTemplateValidationRules.GetPlainTextLength(body) <= 5000;
}

public class UpdateEmailTemplateRequestValidator : AbstractValidator<UpdateEmailTemplateRequest>
{
    public UpdateEmailTemplateRequestValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Subject)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Headline)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.BodyHtml)
            .NotEmpty()
            .Must(HaveValidPlainTextLength)
            .WithMessage("Body must not exceed 5000 characters.");
    }

    private static bool HaveValidPlainTextLength(string body) =>
        EmailTemplateValidationRules.GetPlainTextLength(body) <= 5000;
}

internal static class EmailTemplateValidationRules
{
    internal static int GetPlainTextLength(string body)
    {
        if (string.IsNullOrEmpty(body))
            return 0;

        var plainText = System.Text.RegularExpressions.Regex.Replace(body, "<.*?>", string.Empty);
        plainText = System.Net.WebUtility.HtmlDecode(plainText);
        return plainText.Length;
    }
}
