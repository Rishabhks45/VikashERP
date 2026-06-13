CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Table: invoice_items
-- Description: Line items within an invoice

CREATE TABLE invoice_items (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    invoice_id UUID NOT NULL,
    variant_id UUID NOT NULL,
    qty_pcs integer NOT NULL DEFAULT 0,
    weight_kg numeric(12,3) NOT NULL DEFAULT 0,
    rate_per_kg numeric(12,2) NOT NULL DEFAULT 0,
    cgst_rate numeric(5,2) NOT NULL DEFAULT 9,
    sgst_rate numeric(5,2) NOT NULL DEFAULT 9,
    igst_rate numeric(5,2) NOT NULL DEFAULT 0,
    total_price numeric(12,2) NOT NULL DEFAULT 0,
    CONSTRAINT "FK_invoice_items_invoices" FOREIGN KEY (invoice_id) REFERENCES invoices (id) ON DELETE CASCADE,
    CONSTRAINT "FK_invoice_items_product_variants" FOREIGN KEY (variant_id) REFERENCES product_variants (id) ON DELETE RESTRICT
);
