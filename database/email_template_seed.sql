-- Run once after creating email_templates table (HE-style manual seed, not C# seeder).
-- psql -U postgres -d Test123 -f database/email_template_seed.sql

INSERT INTO email_templates (
    template_key, display_name, description, subject, headline, body_html,
    preheader, button_label, button_link_token, available_tokens, is_active
) VALUES
(
    'ForgotPassword',
    'Forgot Password',
    'Sent when a user requests a password reset link.',
    'Reset Your VikashERP Password',
    'Password Reset Request',
    '<p style="margin:0 0 16px;font-size:15px;line-height:1.7;color:#334155;">We received a request to reset your Vikash Ironix ERP password.</p><p style="margin:0 0 16px;font-size:15px;line-height:1.7;color:#334155;">Use the button below to choose a new password. If you did not request this, you can safely ignore this email.</p><p style="margin:16px 0 0;font-size:13px;line-height:1.6;color:#64748b;">This link expires in {{ExpiryMinutes}} minutes.</p>',
    'Reset your Vikash Ironix password. Link expires in {{ExpiryMinutes}} minutes.',
    'Reset Password',
    '{{ResetLink}}',
    '["{{ResetLink}}","{{ExpiryMinutes}}","{{ContactPhone}}","{{ContactEmail}}"]',
    TRUE
),
(
    'Welcome',
    'Welcome',
    'Sent when a new user account is created.',
    'Welcome to VikashERP',
    'Welcome aboard',
    '<p style="margin:0 0 16px;font-size:15px;line-height:1.7;color:#334155;">Welcome, {{UserName}}! Your Vikash Ironix ERP account is ready.</p><p style="margin:0 0 16px;font-size:15px;line-height:1.7;color:#334155;">Sign in to manage inventory, invoices, ledgers, and daily operations from one place.</p>',
    'Your Vikash Ironix ERP account is ready, {{UserName}}.',
    'Sign In to VikashERP',
    '{{LoginUrl}}',
    '["{{UserName}}","{{LoginUrl}}","{{ContactPhone}}","{{ContactEmail}}"]',
    TRUE
),
(
    'PasswordResetSuccess',
    'Password Reset Success',
    'Sent after a user successfully changes their password.',
    'Your VikashERP Password Was Reset',
    'Password updated',
    '<p style="margin:0 0 16px;font-size:15px;line-height:1.7;color:#334155;">Hi {{UserName}}, your password was changed successfully.</p><p style="margin:0 0 16px;font-size:15px;line-height:1.7;color:#334155;">You can now sign in with your new password.</p>',
    'Your Vikash Ironix ERP password was updated successfully.',
    'Sign In',
    '{{LoginUrl}}',
    '["{{UserName}}","{{LoginUrl}}","{{ContactPhone}}","{{ContactEmail}}"]',
    TRUE
)
ON CONFLICT (template_key) DO NOTHING;
