namespace VikashERP.SharedKernel.Email;

public static class EmailTemplateLayout
{
    public static string Wrap(string headline, string bodyHtml, string? preheader = null)
    {
        var hiddenPreheader = string.IsNullOrWhiteSpace(preheader)
            ? string.Empty
            : $"""
              <div style="display:none;max-height:0;overflow:hidden;opacity:0;color:transparent;">
                  {preheader}
              </div>
              """;

        return $"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="utf-8" />
                <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                <title>{headline}</title>
            </head>
            <body style="margin:0;padding:0;background:#f4f6f8;font-family:'Segoe UI',Roboto,Helvetica,Arial,sans-serif;color:#0f172a;">
                {hiddenPreheader}
                <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="background:#f4f6f8;padding:32px 16px;">
                    <tr>
                        <td align="center">
                            <table role="presentation" width="600" cellspacing="0" cellpadding="0" style="max-width:600px;width:100%;background:#ffffff;border:1px solid #e2e8f0;border-radius:12px;overflow:hidden;box-shadow:0 8px 24px rgba(15,23,42,0.08);">
                                <tr>
                                    <td style="background:linear-gradient(135deg,#111827 0%,#1e293b 100%);padding:24px 32px;">
                                        <table role="presentation" width="100%" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    <div style="font-size:11px;font-weight:800;letter-spacing:1.4px;text-transform:uppercase;color:#fca5a5;margin-bottom:6px;">Vikash Ironix ERP</div>
                                                    <div style="font-size:22px;font-weight:800;line-height:1.2;color:#ffffff;">{headline}</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding:32px;">
                                        {bodyHtml}
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background:#f8fafc;border-top:1px solid #e2e8f0;padding:20px 32px;font-size:12px;line-height:1.6;color:#64748b;">
                                        {FooterHtml()}
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>
            """;
    }

    public static string PrimaryButton(string label, string href) =>
        $"""
        <table role="presentation" cellspacing="0" cellpadding="0" style="margin:24px 0 8px;">
            <tr>
                <td style="border-radius:8px;background:#991b1b;">
                    <a href="{href}" target="_blank" style="display:inline-block;padding:14px 28px;font-size:14px;font-weight:700;color:#ffffff;text-decoration:none;border-radius:8px;">
                        {label}
                    </a>
                </td>
            </tr>
        </table>
        """;

    public static string MutedNote(string text) =>
        $"""<p style="margin:16px 0 0;font-size:13px;line-height:1.6;color:#64748b;">{text}</p>""";

    public static string Paragraph(string text) =>
        $"""<p style="margin:0 0 16px;font-size:15px;line-height:1.7;color:#334155;">{text}</p>""";

    private static string FooterHtml() =>
        """
        Need help? Call {{ContactPhone}} or email {{ContactEmail}}.
        <br /><br />
        This is an automated message from Vikash Ironix ERP.
        <br />
        """ + $"&copy; {DateTime.UtcNow.Year} Vikash Ironix. All rights reserved.";
}
