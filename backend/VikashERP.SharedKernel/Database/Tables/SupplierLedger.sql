CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Table: supplier_ledger
-- Description: Financial ledger tracking debits/credits for suppliers

CREATE TABLE supplier_ledger (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    supplier_id UUID NOT NULL,
    transaction_date timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    transaction_type character varying(50) NOT NULL,
    reference_id UUID NULL,
    debit numeric(12,2) NOT NULL DEFAULT 0,
    credit numeric(12,2) NOT NULL DEFAULT 0,
    running_balance numeric(12,2) NOT NULL DEFAULT 0,
    remarks text NULL,
    CONSTRAINT "FK_supplier_ledger_suppliers" FOREIGN KEY (supplier_id) REFERENCES suppliers (id) ON DELETE CASCADE,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    created_by UUID NULL,
    updated_at timestamp with time zone NULL,
    updated_by UUID NULL,
    is_active boolean NOT NULL DEFAULT TRUE,
    is_deleted boolean NOT NULL DEFAULT FALSE
);
