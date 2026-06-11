-- Run once on existing DBs that only have template_key UNIQUE (allows Email + SMS per key).
-- psql -U postgres -d YourDb -f database/email_templates_add_notification_type.sql

ALTER TABLE email_templates
    ADD COLUMN IF NOT EXISTS notification_type INT NOT NULL DEFAULT 1;

ALTER TABLE email_templates
    DROP CONSTRAINT IF EXISTS email_templates_template_key_key;

DROP INDEX IF EXISTS ux_email_templates_key_channel;

CREATE UNIQUE INDEX ux_email_templates_key_channel
    ON email_templates (template_key, notification_type);
