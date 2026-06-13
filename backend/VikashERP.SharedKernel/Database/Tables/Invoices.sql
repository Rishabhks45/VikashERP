CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Table: invoices
-- Description: Sales invoice records

CREATE TABLE invoices (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    invoice_number character varying(100) NOT NULL,
    customer_id UUID NOT NULL,
    subtotal numeric(12,2) NOT NULL DEFAULT 0,
    cgst_amount numeric(12,2) NOT NULL DEFAULT 0,
    sgst_amount numeric(12,2) NOT NULL DEFAULT 0,
    igst_amount numeric(12,2) NOT NULL DEFAULT 0,
    total_amount numeric(12,2) NOT NULL DEFAULT 0,
    paid_amount numeric(12,2) NOT NULL DEFAULT 0,
    due_amount numeric(12,2) NOT NULL DEFAULT 0,
    payment_mode character varying(50) NOT NULL,
    invoice_date date NOT NULL DEFAULT CURRENT_DATE,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_invoices_Customers" FOREIGN KEY (customer_id) REFERENCES "Customers" ("Id") ON DELETE RESTRICT
);

CREATE UNIQUE INDEX idx_invoices_invoice_number ON invoices (invoice_number);
