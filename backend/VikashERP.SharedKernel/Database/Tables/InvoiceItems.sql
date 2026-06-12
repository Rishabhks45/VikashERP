-- Table: invoice_items
-- Description: Line items within an invoice

CREATE TABLE invoice_items (
    id SERIAL PRIMARY KEY,
    invoice_id integer NOT NULL,
    variant_id integer NOT NULL,
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
