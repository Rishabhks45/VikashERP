CREATE EXTENSION IF NOT EXISTS pgcrypto;
CREATE SEQUENCE IF NOT EXISTS sales_invoice_seq START 1;

-- Table: invoices
-- Description: Sales invoice records

CREATE TABLE invoices (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    invoice_number character varying(100) NOT NULL,
    customer_id UUID NOT NULL,
    subtotal numeric(12,2) NOT NULL DEFAULT 0,
    freight_charge numeric(12,2) NOT NULL DEFAULT 0,
    loading_charge numeric(12,2) NOT NULL DEFAULT 0,
    cgst_amount numeric(12,2) NOT NULL DEFAULT 0,
    sgst_amount numeric(12,2) NOT NULL DEFAULT 0,
    igst_amount numeric(12,2) NOT NULL DEFAULT 0,
    rounding_amount numeric(12,2) NOT NULL DEFAULT 0,
    total_amount numeric(12,2) NOT NULL DEFAULT 0,
    paid_amount numeric(12,2) NOT NULL DEFAULT 0,
    cash_amount numeric(12,2) NOT NULL DEFAULT 0,
    bank_amount numeric(12,2) NOT NULL DEFAULT 0,
    due_amount numeric(12,2) NOT NULL DEFAULT 0,
    payment_mode character varying(50) NOT NULL,
    vehicle_number character varying(50) NULL,
    remarks character varying(1000) NULL,
    status integer NOT NULL DEFAULT 0,
    invoice_date timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_invoices_Customers" FOREIGN KEY (customer_id) REFERENCES "Customers" ("Id") ON DELETE RESTRICT,
    created_by UUID NULL,
    updated_at timestamp with time zone NULL,
    updated_by UUID NULL,
    is_active boolean NOT NULL DEFAULT TRUE,
    is_deleted boolean NOT NULL DEFAULT FALSE
);

CREATE UNIQUE INDEX idx_invoices_invoice_number ON invoices (invoice_number);
