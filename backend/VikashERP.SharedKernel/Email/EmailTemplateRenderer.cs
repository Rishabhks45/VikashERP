using System.Text.Json;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.SharedKernel.Email;

public sealed record EmailTemplateContent(string Subject, string HtmlBody);

public static class EmailTemplateTokens
{
    public const string UserName = "{{UserName}}";
    public const string UserEmail = "{{UserEmail}}";
    public const string Password = "{{Password}}";
    public const string ResetLink = "{{ResetLink}}";
    public const string LoginUrl = "{{LoginUrl}}";
    public const string ExpiryMinutes = "{{ExpiryMinutes}}";
    public const string ContactPhone = "{{ContactPhone}}";
    public const string ContactEmail = "{{ContactEmail}}";
}

public static class EmailTemplateRenderer
{
    public static EmailTemplateContent Render(
        string subject,
        string headline,
        string bodyHtml,
        string? preheader,
        string? buttonLabel,
        string? buttonLinkToken,
        IReadOnlyDictionary<string, string> tokens)
    {
        var resolvedSubject = ReplaceTokens(subject, tokens);
        var resolvedHeadline = ReplaceTokens(headline, tokens);
        var resolvedBody = ReplaceTokens(bodyHtml, tokens);
        var resolvedPreheader = string.IsNullOrWhiteSpace(preheader) ? null : ReplaceTokens(preheader, tokens);

        if (!string.IsNullOrWhiteSpace(buttonLabel) && !string.IsNullOrWhiteSpace(buttonLinkToken))
        {
            var href = ResolveButtonHref(buttonLinkToken, tokens);
            resolvedBody += EmailTemplateLayout.PrimaryButton(
                ReplaceTokens(buttonLabel, tokens),
                href);
        }

        return new EmailTemplateContent(
            resolvedSubject,
            ReplaceTokens(
                EmailTemplateLayout.Wrap(resolvedHeadline, resolvedBody, resolvedPreheader),
                tokens));
    }

    public static string ReplaceTokens(string template, IReadOnlyDictionary<string, string> tokens)
    {
        if (string.IsNullOrEmpty(template))
            return template;

        var result = template;
        foreach (var token in tokens)
        {
            if (string.IsNullOrWhiteSpace(token.Key))
                continue;

            result = result.Replace(token.Key, token.Value ?? string.Empty, StringComparison.Ordinal);
        }

        return result;
    }

    public static IReadOnlyList<string> ParseAvailableTokens(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return [];

        try
        {
            return JsonSerializer.Deserialize<List<string>>(json) ?? [];
        }
        catch
        {
            return [];
        }
    }

    public static string SerializeAvailableTokens(IEnumerable<string> tokens) =>
        JsonSerializer.Serialize(tokens);

    public static EmailTemplateType ParseTemplateKey(string templateKey) =>
        Enum.TryParse<EmailTemplateType>(templateKey, ignoreCase: true, out var type)
            ? type
            : throw new InvalidOperationException($"Unknown email template key '{templateKey}'.");

    private static string ResolveButtonHref(string buttonLinkToken, IReadOnlyDictionary<string, string> tokens)
    {
        if (tokens.TryGetValue(buttonLinkToken, out var href) && !string.IsNullOrWhiteSpace(href))
            return href;

        return ReplaceTokens(buttonLinkToken, tokens);
    }
}
