using System.Net;
using System.Text.RegularExpressions;
using FluentValidation;
using VikashERP.SharedKernel.Enums;
using VikashERP.Web.Models.Forms;

namespace VikashERP.Web.Validation;

public class UpsertEmailTemplateFormModelValidator : AbstractValidator<UpsertEmailTemplateFormModel>
{
    public UpsertEmailTemplateFormModelValidator()
    {
        RuleFor(x => x.NotificationType)
            .IsInEnum()
            .Must(type => type != NotificationType.None)
            .WithMessage("Notification type is required.")
            .When(x => x.IsCreate);

        RuleFor(x => x.TemplateKey)
            .NotEmpty()
            .WithMessage("Template key is required.")
            .MaximumLength(50)
            .When(x => x.IsCreate);

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .WithMessage("Display name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500);

        RuleFor(x => x.Subject)
            .NotEmpty()
            .WithMessage("Subject is required.")
            .MaximumLength(255);

        RuleFor(x => x.Headline)
            .NotEmpty()
            .WithMessage("Header headline is required.")
            .MaximumLength(255);

        RuleFor(x => x.BodyHtml)
            .NotEmpty()
            .WithMessage("Body HTML is required.")
            .Must(HaveValidPlainTextLength)
            .WithMessage("Body must not exceed 5000 characters.");
    }

    private static bool HaveValidPlainTextLength(string body) =>
        GetPlainTextLength(body) <= 5000;

    private static int GetPlainTextLength(string body)
    {
        if (string.IsNullOrEmpty(body))
            return 0;

        var plainText = Regex.Replace(body, "<.*?>", string.Empty);
        plainText = WebUtility.HtmlDecode(plainText);
        return plainText.Length;
    }
}
