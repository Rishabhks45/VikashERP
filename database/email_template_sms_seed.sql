-- SMS templates (notification_type = 2). Run after email_templates_add_notification_type.sql
-- psql -U postgres -d Test123 -f database/email_template_sms_seed.sql

INSERT INTO email_templates (
    template_key, notification_type, display_name, description, subject, headline, body_html,
    preheader, button_label, button_link_token, available_tokens, is_active
) VALUES
(
    'ForgotPassword',
    2,
    'Forgot Password SMS',
    'SMS sent when a user requests a password reset link.',
    'Password Reset SMS',
    'Password Reset',
    'Vikash Ironix: Reset your password: {{ResetLink}} (valid {{ExpiryMinutes}} min). Ignore if not you. Help: {{ContactPhone}}',
    NULL,
    NULL,
    NULL,
    '["{{ResetLink}}","{{ExpiryMinutes}}","{{ContactPhone}}","{{ContactEmail}}"]',
    TRUE
),
(
    'Welcome',
    2,
    'Welcome SMS',
    'SMS sent when a new user account is created.',
    'Welcome SMS',
    'Welcome',
    'Welcome to Vikash Ironix ERP, {{UserName}}! Sign in: {{LoginUrl}}. Support: {{ContactPhone}}',
    NULL,
    NULL,
    NULL,
    '["{{UserName}}","{{LoginUrl}}","{{ContactPhone}}","{{ContactEmail}}"]',
    TRUE
),
(
    'PasswordResetSuccess',
    2,
    'Password Reset Success SMS',
    'SMS sent after a user successfully changes their password.',
    'Password Reset Success SMS',
    'Password Updated',
    'Hi {{UserName}}, your Vikash Ironix ERP password was updated. Sign in: {{LoginUrl}}. Help: {{ContactPhone}}',
    NULL,
    NULL,
    NULL,
    '["{{UserName}}","{{LoginUrl}}","{{ContactPhone}}","{{ContactEmail}}"]',
    TRUE
)
ON CONFLICT (template_key, notification_type) DO NOTHING;
