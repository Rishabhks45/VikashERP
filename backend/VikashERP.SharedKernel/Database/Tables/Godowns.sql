-- Table: godowns
-- Description: Warehouse/godown master records

CREATE TABLE godowns (
    id SERIAL PRIMARY KEY,
    name character varying(100) NOT NULL,
    location character varying(255) NULL,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE UNIQUE INDEX idx_godowns_name ON godowns (name);
