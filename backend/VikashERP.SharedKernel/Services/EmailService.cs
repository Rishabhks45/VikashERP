using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using VikashERP.SharedKernel.Common.Interfaces;
using VikashERP.SharedKernel.Settings;

namespace VikashERP.SharedKernel.Services;

public class EmailService : IEmailSender
{
    private readonly SendGridSettings _sendGridSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<SendGridSettings> sendGridSettings, ILogger<EmailService> logger)
    {
        _sendGridSettings = sendGridSettings.Value;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string body, string? cc = null, string? bcc = null)
    {
        try
        {
            var mimeMessage = CreateMimeMessage(toEmail, subject, body, cc, bcc);

            if (!ValidateConfiguration(out var apiKey, out var fromEmail, out var fromName))
            {
                return await SaveToDiskAsync(mimeMessage);
            }

            return await SendViaSendGridAsync(mimeMessage, apiKey, fromEmail, fromName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Email sending failed: {ex.Message}. Email to: {toEmail} | Subject: {subject}");
            await SaveToDiskAsync(toEmail, subject, body);
            return false;
        }
    }

    private MimeMessage CreateMimeMessage(string toEmail, string subject, string body, string? cc, string? bcc)
    {
        var emailMessage = new MimeMessage();

        emailMessage.To.Add(MailboxAddress.Parse(toEmail));

        if (!string.IsNullOrWhiteSpace(cc))
        {
            foreach (var address in cc.Split(';', ',').Where(a => !string.IsNullOrWhiteSpace(a)))
            {
                emailMessage.Cc.Add(MailboxAddress.Parse(address.Trim()));
            }
        }

        if (!string.IsNullOrWhiteSpace(bcc))
        {
            foreach (var address in bcc.Split(';', ',').Where(a => !string.IsNullOrWhiteSpace(a)))
            {
                emailMessage.Bcc.Add(MailboxAddress.Parse(address.Trim()));
            }
        }

        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = body,
            TextBody = StripHtml(body)
        };

        emailMessage.Body = bodyBuilder.ToMessageBody();
        return emailMessage;
    }

    private string StripHtml(string html)
    {
        if (string.IsNullOrEmpty(html)) return string.Empty;
        return System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty).Trim();
    }

    private bool ValidateConfiguration(out string apiKey, out string fromEmail, out string fromName)
    {
        apiKey = string.Empty;
        fromEmail = string.Empty;
        fromName = string.Empty;

        if (_sendGridSettings == null)
        {
            _logger.LogError("SendGridSettings is null.");
            return false;
        }

        apiKey = _sendGridSettings.APIKey?.Trim() ?? "";
        fromEmail = _sendGridSettings.FromEmail;
        fromName = _sendGridSettings.FromName;

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _logger.LogError("SendGrid API Key is missing.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(fromEmail))
        {
            _logger.LogError("FromEmail is missing in settings.");
            return false;
        }

        return true;
    }

    private async Task<bool> SendViaSendGridAsync(MimeMessage mimeMessage, string apiKey, string fromEmail, string fromName)
    {
        try
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, fromName);

            var sendGridMessage = new SendGridMessage
            {
                From = from,
                Subject = mimeMessage.Subject,
                PlainTextContent = mimeMessage.TextBody,
                HtmlContent = mimeMessage.HtmlBody
            };

            sendGridMessage.AddTos(mimeMessage.To.Mailboxes.Select(x => new EmailAddress(x.Address, x.Name)).ToList());

            if (mimeMessage.Cc.Count > 0)
            {
                sendGridMessage.AddCcs(mimeMessage.Cc.Mailboxes.Select(x => new EmailAddress(x.Address, x.Name)).ToList());
            }

            if (mimeMessage.Bcc.Count > 0)
            {
                sendGridMessage.AddBccs(mimeMessage.Bcc.Mailboxes.Select(x => new EmailAddress(x.Address, x.Name)).ToList());
            }

            foreach (var attachment in mimeMessage.Attachments.OfType<MimePart>())
            {
                using var ms = new MemoryStream();
                attachment.Content.DecodeTo(ms);
                sendGridMessage.AddAttachment(
                    attachment.FileName,
                    Convert.ToBase64String(ms.ToArray()),
                    attachment.ContentType.MimeType,
                    "attachment"
                );
            }

            var response = await client.SendEmailAsync(sendGridMessage);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Email sent successfully. Subject: {mimeMessage.Subject}");
                return true;
            }
            else
            {
                var responseBody = await response.Body.ReadAsStringAsync();
                _logger.LogError($"SendGrid API Error: {response.StatusCode}. Details: {responseBody}");

                await SaveToDiskAsync(mimeMessage);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"SendGrid Exception: {ex.Message}");
            await SaveToDiskAsync(mimeMessage);
            return false;
        }
    }

    private async Task<bool> SaveToDiskAsync(MimeMessage mimeMessage)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"--- EMAIL SENT AT {DateTime.Now} ---");
        sb.AppendLine($"To: {string.Join(", ", mimeMessage.To)}");
        sb.AppendLine($"Subject: {mimeMessage.Subject}");
        sb.AppendLine("--- BODY ---");
        sb.AppendLine(mimeMessage.TextBody ?? mimeMessage.HtmlBody);

        return await WriteToFileAsync(sb.ToString());
    }

    private async Task<bool> SaveToDiskAsync(string to, string subject, string body)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"--- EMAIL SENT AT {DateTime.Now} ---");
        sb.AppendLine($"To: {to}");
        sb.AppendLine($"Subject: {subject}");
        sb.AppendLine("--- BODY ---");
        sb.AppendLine(body);

        return await WriteToFileAsync(sb.ToString());
    }

    private async Task<bool> WriteToFileAsync(string content)
    {
        try
        {
            var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sent_emails");
            Directory.CreateDirectory(logDirectory);

            var filename = $"email_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid()}.txt";
            var filePath = Path.Combine(logDirectory, filename);

            await File.WriteAllTextAsync(filePath, content);
            _logger.LogWarning($"[DevFallback] Email saved to: {filePath}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to save email to disk: {ex.Message}");
            return false;
        }
    }
}
