-- Add login credentials to the Welcome email template.
-- psql -U postgres -d Test123 -f database/009_update_welcome_email_credentials.sql

UPDATE email_templates
SET
    body_html = '<p style="margin:0 0 16px;font-size:15px;line-height:1.7;color:#334155;">Welcome, {{UserName}}! Your Vikash Ironix ERP account is ready.</p><p style="margin:0 0 16px;font-size:15px;line-height:1.7;color:#334155;">Use the credentials below to sign in. We recommend changing your password after your first login.</p><p style="margin:0 0 16px;font-size:15px;line-height:1.7;color:#334155;"><strong>Email:</strong> {{UserEmail}}<br/><strong>Temporary password:</strong> {{Password}}</p>',
    available_tokens = '["{{UserName}}","{{UserEmail}}","{{Password}}","{{LoginUrl}}","{{ContactPhone}}","{{ContactEmail}}"]',
    updated_at = NOW()
WHERE template_key = 'Welcome'
  AND notification_type = 1;
