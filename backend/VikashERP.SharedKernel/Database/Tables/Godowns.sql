CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Table: godowns
-- Description: Warehouse/godown master records

CREATE TABLE godowns (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name character varying(100) NOT NULL,
    location character varying(255) NULL,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    created_by UUID NULL,
    updated_at timestamp with time zone NULL,
    updated_by UUID NULL,
    is_active boolean NOT NULL DEFAULT TRUE,
    is_deleted boolean NOT NULL DEFAULT FALSE
);

CREATE UNIQUE INDEX idx_godowns_name ON godowns (name);
