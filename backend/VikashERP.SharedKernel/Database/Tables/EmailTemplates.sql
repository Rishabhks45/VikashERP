-- Table: email_templates
-- Description: Email/notification template configuration

CREATE TABLE email_templates (
    id SERIAL PRIMARY KEY,
    template_key character varying(50) NOT NULL,
    notification_type integer NOT NULL DEFAULT 0,
    display_name character varying(100) NOT NULL,
    description character varying(500) NOT NULL,
    subject character varying(255) NOT NULL,
    headline character varying(255) NOT NULL,
    body_html text NOT NULL,
    preheader character varying(255) NULL,
    button_label character varying(100) NULL,
    button_link_token character varying(100) NULL,
    available_tokens text NOT NULL DEFAULT '[]',
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE UNIQUE INDEX idx_email_templates_key_type ON email_templates (template_key, notification_type);
