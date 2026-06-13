CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Table: product_variants
-- Description: Size/thickness variants for products

CREATE TABLE product_variants (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    product_id UUID NOT NULL,
    size character varying(50) NOT NULL,
    thickness character varying(50) NOT NULL,
    unit_pcs_to_kg numeric(12,4) NOT NULL DEFAULT 1,
    alert_qty_pcs integer NOT NULL DEFAULT 10,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_product_variants_Products" FOREIGN KEY (product_id) REFERENCES "Products" ("Id") ON DELETE CASCADE,
    created_by UUID NULL,
    updated_at timestamp with time zone NULL,
    updated_by UUID NULL,
    is_active boolean NOT NULL DEFAULT TRUE,
    is_deleted boolean NOT NULL DEFAULT FALSE
);

CREATE UNIQUE INDEX idx_product_variants_unique ON product_variants (product_id, size, thickness);
