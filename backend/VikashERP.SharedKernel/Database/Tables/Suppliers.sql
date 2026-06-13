CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Table: suppliers
-- Description: Supplier master records

CREATE TABLE suppliers (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name character varying(255) NOT NULL,
    company_name character varying(255) NULL,
    phone character varying(20) NOT NULL,
    gstin character varying(15) NULL,
    address text NULL,
    current_balance numeric(12,2) NOT NULL DEFAULT 0,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP
);
