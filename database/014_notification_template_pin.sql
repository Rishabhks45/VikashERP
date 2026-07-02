INSERT INTO notification_templates
(
    template_key,
    name,
    description,
    subject,
    preview_text,
    body,
    sms_preview,
    button_text,
    button_url,
    variables,
    is_active
)
VALUES
(
    'PinGeneration',
    'PIN Generation',
    'Sent when a user requests a verification PIN.',
    'Your VikashERP Verification PIN',
    'Your verification PIN',
    '<p style="margin:0 0 16px;font-size:15px;line-height:1.7;color:#334155;">Hi {{UserName}},</p>
<p style="margin:0 0 16px;font-size:15px;line-height:1.7;color:#334155;">Your verification PIN is:</p>
<div style="margin:20px 0;text-align:center;">
<span style="display:inline-block;padding:12px 24px;font-size:28px;font-weight:700;letter-spacing:6px;background:#f1f5f9;border:1px solid #cbd5e1;border-radius:8px;color:#0f172a;">{{Pin}}</span>
</div>
<p style="margin:0 0 16px;font-size:15px;line-height:1.7;color:#334155;">This PIN is valid for <strong>{{ExpiryMinutes}}</strong> minutes.</p>
<p style="margin:16px 0 0;font-size:13px;line-height:1.6;color:#64748b;">If you did not request this PIN, please ignore this email or contact your administrator.</p>',
    'Your VikashERP verification PIN is {{Pin}}. Valid for {{ExpiryMinutes}} minutes.',
    'Verify PIN',
    '{{VerificationUrl}}',
    '["{{UserName}}","{{Pin}}","{{ExpiryMinutes}}","{{VerificationUrl}}","{{ContactPhone}}","{{ContactEmail}}"]',
    TRUE
);
