-- Migration: user ↔ customer mapping + profile picture on Users
-- Run against an existing VikashERP database (PostgreSQL).

ALTER TABLE "Users"
    ADD COLUMN IF NOT EXISTS "ProfilePictureUrl" character varying(500) NULL;

CREATE TABLE IF NOT EXISTS user_customer_mappings (
    id SERIAL PRIMARY KEY,
    user_id UUID NOT NULL,
    customer_id INT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NULL,
    CONSTRAINT "FK_user_customer_mappings_Users" FOREIGN KEY (user_id) REFERENCES "Users" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_user_customer_mappings_customers" FOREIGN KEY (customer_id) REFERENCES customers (id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_user_customer_mappings_user_id ON user_customer_mappings (user_id);
CREATE INDEX IF NOT EXISTS idx_user_customer_mappings_customer_id ON user_customer_mappings (customer_id);
